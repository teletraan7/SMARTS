using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetsBehaviour : MonoBehaviour {

	// whole purpose of this script is to have the gameobject that the camera tracks always be somewhere far above and slightly behind the midpoint between all of the characters

	public Transform playerPos;
	public float yOffset = 10.0f;
	public float zOffset = 5.0f;

	void FixedUpdate() {
		Vector3 adjPos = playerPos.position;
		adjPos.y += yOffset;
		adjPos.z -= zOffset;

		transform.position = adjPos;
	}
}
