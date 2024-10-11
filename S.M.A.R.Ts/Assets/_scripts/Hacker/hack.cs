using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hack : MonoBehaviour {

	//security object
	private GameObject security;
	//touching bool will be used to check if player is touching a destructable wall
	private bool touching;

	//if player collider hits another collider
	void OnCollisionEnter (Collision other) {
		//and the objects tag is hackable
		if (other.gameObject.tag == "hackable" || other.gameObject.tag == "AI") {
			//set security
			security = other.gameObject;
			//set touching
			touching = true;
		} else {
			//otherwise touching is false
			touching = false;
		}
	}

	void Update () {
		//if Q is hit - or left trigger on controller - and touching is true
		if (Input.GetKeyDown (KeyCode.Q) && touching == true) {
			//dont render security
			security.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			//shut off its collider
			security.gameObject.GetComponent<MeshCollider> ().enabled = false;
		}
	}
}
