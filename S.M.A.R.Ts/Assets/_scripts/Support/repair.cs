using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class repair : MonoBehaviour {

	//wall will be the wall demolition is touching
	private GameObject wall;
	//touching bool will be used to check if player is touching a destructable wall
	private bool touching;

	//if player collider enters a trigger collider
	void OnTriggerEnter (Collider other) {
		//and that objects tag is DestructWall
		if (other.gameObject.tag == "DestructWall") {
			//set wall
			wall = other.gameObject;
			//set touching
			touching = true;
		}
	}

	void OnTriggerExit (Collider other) {
		touching = false;
	}

	void Update () {
		Debug.Log (touching);

		//if U is hit - or right trigger on controller - and touching is true
		if (Input.GetKeyDown (KeyCode.U) && touching == true) {
			//render the wall
			wall.gameObject.GetComponent<MeshRenderer> ().enabled = true;
			//unset trigger so the collider works as normal
			wall.gameObject.GetComponent<BoxCollider> ().isTrigger = false;
		}
	}
}
