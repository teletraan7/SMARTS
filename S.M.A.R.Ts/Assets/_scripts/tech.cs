using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tech : MonoBehaviour {

	private GameObject toolBox;
	//touching bool will be used to check if player is touching a destructable wall
	private bool touching;

	//if player collider hits another collider
	void OnCollisionEnter (Collision other) {
		//and objects tag is toolNeeded
		if (other.gameObject.tag == "toolNeeded") {
			//set toolBox
			toolBox = other.gameObject;
			//set touching
			touching = true;
		} else {
			//otherwise touching is false
			touching = false;
		}
	}
	
	void Update () {
		//if U is hit - or right trigger on controller - and touching is true
		if (Input.GetKeyDown (KeyCode.U) && touching == true) {
			//dont render toobBox object
			toolBox.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			//shut off its collider
			toolBox.gameObject.GetComponent<BoxCollider> ().enabled = false;
		}
	}
}
