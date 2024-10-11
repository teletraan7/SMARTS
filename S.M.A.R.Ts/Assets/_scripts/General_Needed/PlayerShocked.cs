using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShocked : MonoBehaviour {

    private PointsManager psm;

    //for this we will need the rigidbody, a bool, and two floats
    private Rigidbody rb;
	public bool shocked;
	public float wait;
	public float resTime;
	public TurretAI turretai;
	private bool rangeBool;
	public ParticleSystem shockFx;
    private AudioSource audiosor;
    public AudioClip shock;

	public void Awake () {
		//get rigidbody
		rb = this.gameObject.GetComponent<Rigidbody> ();
		//set float to default of false
		shocked = false;
		shockFx.Stop ();
        psm = GameObject.FindWithTag("GameController").GetComponent<PointsManager>();
        audiosor = this.gameObject.GetComponent<AudioSource>();
    }
		
	// note from Ben: I'm going to create another ShockPlayer for turrets only because you can't use a parameter with Invoked functions, would be better to use Coroutines overall
	//this is what will "shock" the player, called from the TurretAI
	public void ShockPlayer () {
		// UI feedback
		FloatingTextController.CreateFloatingText("Stunned", transform);
		shockFx.Play ();
        psm.TimesStuned += 1;
        //set true so we know player is shocked
        shocked = true;
		//freeze movement
		rb.isKinematic = true;
		//set a cooldown timer
		resTime = Time.time + wait;
		StartCoroutine(NaturalUnshock(wait));
        audiosor.clip = shock;
        audiosor.Play();
        audiosor.loop = true;
	}

	public void ShockPlayerFromTurret () {
		// UI feedback
		FloatingTextController.CreateFloatingText("Stunned", transform);
		shockFx.Play ();
        psm.TimesStuned += 1;
        //set true so we know player is shocked
        shocked = true;
		//freeze movement
		rb.isKinematic = true;
		//set a cooldown timer
		resTime = Time.time + wait;
		StartCoroutine(NaturalUnshock(wait));
        audiosor.clip = shock;
        audiosor.Play();
    }

	public IEnumerator NaturalUnshock(float shockTime) {
		yield return new WaitForSeconds (shockTime);

		if (Time.time < resTime) {
			Debug.Log ("Probably have stuns stacked on top of stuns.");
			yield break;
		} else if (shocked) {
			// ui feedback
			if (shockTime != 0f) {
				FloatingTextController.CreateFloatingText ("Active", transform);
			}
			shockFx.Stop ();			
			shocked = false;
			rb.isKinematic = false;
            audiosor.Stop();
        } else {
			shockFx.Stop ();
			Debug.Log (gameObject.name + " already not shocked");
            audiosor.Stop();
        }
	}
}
