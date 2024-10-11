using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenderHit : MonoBehaviour {

	public GameObject plyr;
	public float force;
	public float fireRate;
	public float reload;
	private GameObject targetDrone;
    private AudioSource audioSor;
    public AudioClip metalHit;
	//rate of fire

	public GameObject hitFx;

	// Use this for initialization
	void Start () {
		reload = 0f;
	}

	void OnTriggerEnter (Collider other) {
		if (other.gameObject.tag == "drone") {
			targetDrone = other.gameObject;
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.gameObject.tag == "drone") {
			targetDrone = null;
		}
	}

	public void Hit() {
		if (Time.time > reload && targetDrone != null) {
			Invoke ("Stop", 1f);
            audioSor.clip = metalHit;
            audioSor.Play();
			//add force or find some way to push back a few meters relative to players forward rotation
			Vector3 dir =  targetDrone.transform.position - plyr.gameObject.transform.position;

			dir = dir.normalized;
			Rigidbody droneRB = targetDrone.GetComponent<Rigidbody> ();
			droneRB.velocity = new Vector3 (0f, 0f, 0f);
			droneRB.angularVelocity = new Vector3 (0f, 0f, 0f);
			droneRB.AddForce (dir * force, ForceMode.Impulse);
			GameObject spawnedFx = Instantiate (hitFx, droneRB.transform) as GameObject;
			spawnedFx.transform.SetParent (null);
		} else if (Time.time < reload || targetDrone == null) {
		
			return;
		}
	}

	void Stop() {
		reload = Time.time + fireRate;
       
	}
}
