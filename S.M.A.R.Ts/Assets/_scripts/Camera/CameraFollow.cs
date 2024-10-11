using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	//player
	private GameObject player;
	//will be used to keep the camera with the player
	private Vector3 offset;


	void Start () {
		//get player
		player = GameObject.FindGameObjectWithTag ("Player");
		//set offset to the difference between camera and player positions
		offset = transform.position - player.transform.position;
	}
	
	//late update makes sure that everything else has happened on this frame
	void LateUpdate () {
		//move camera with player but keep it a set distance from player
		transform.position = player.transform.position + offset;
	}
}
