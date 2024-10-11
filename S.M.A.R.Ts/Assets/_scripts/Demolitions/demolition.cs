using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demolition : MonoBehaviour {

	//wall will be the wall demolition is touching
	private GameObject wall;
	//touching bool will be used to check if player is touching a destructable wall
	private bool touching;

	//if player collider hits another collider
	void OnCollisionEnter (Collision other) {
		//and that objects tag is DestructWall
		if (other.gameObject.tag == "DestructWall") {
			//set wall
			wall = other.gameObject;
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
			//dont render that wall
			wall.gameObject.GetComponent<MeshRenderer> ().enabled = false;
			//set its collider to trigger so it can be passed through and used by the repair class later
			wall.gameObject.GetComponent<BoxCollider> ().isTrigger = true;
		}
	}
}
