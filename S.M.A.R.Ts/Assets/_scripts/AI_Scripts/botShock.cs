using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class botShock : MonoBehaviour {

	public float cooldown;
	public bool shocked;
	public bool isTurret = false;
	public ParticleSystem shockFx;
    public GameObject Cap;
    private AudioSource audioSor;
    public AudioClip RobotDisabled;
    public AudioClip Idle;

	// Use this for initialization
	void Awake () {
	
		shockFx.Stop ();
	}
		
	// need to do this because this asset is pooled, not instantiated (awake only happens once)
	void OnEnable() {
		
		shockFx.Stop ();
	}

	public void DisableBot() {
		// UI feedback
		FloatingTextController.CreateFloatingText ("Stunned", transform);
        audioSor.clip = RobotDisabled;
        audioSor.loop = true;
        audioSor.Play();
		shockFx.Play ();
		shocked = true;
        Vector3 stopMovement = new Vector3(0, 0, 0);
        if (this.gameObject.tag == "turret")
        {
            Cap.gameObject.GetComponent<Rigidbody>().angularVelocity = stopMovement;
            Cap.gameObject.GetComponent<Rigidbody>().velocity = stopMovement;
            Cap.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        } else if (this.gameObject.tag == "drone")
        {
            this.gameObject.GetComponent<Rigidbody>().angularVelocity = stopMovement;
            this.gameObject.GetComponent<Rigidbody>().velocity = stopMovement;
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        }
		StartCoroutine (Unshock (cooldown));
		Debug.Log (shocked + "shocked");
	}

	IEnumerator Unshock(float delay) {
		yield return new WaitForSeconds (delay);
        if (this.gameObject.tag == "turret")
        {
            Cap.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }
        // UI feedback
        if (!isTurret) {
			FloatingTextController.CreateFloatingText ("Patrolling", transform);
		}
        audioSor.clip = Idle;
        audioSor.Play();
		shockFx.Stop ();
		shocked = false;
	}
}
