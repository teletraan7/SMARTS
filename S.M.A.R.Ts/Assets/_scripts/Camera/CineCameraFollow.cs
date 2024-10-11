using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineCameraFollow : MonoBehaviour {

	public CinemachineVirtualCamera vcam;		// reference to Cinemachine camera
	public Transform followObj;					// reference to empty game object that will always be moved to the center position between all four players which is calculated in FindCenterPoint()
	public GameObject[] players;				// array of all players (as is, assumed that players will always be alive (instead of being destroyed, they get stunned or something), will need changes if that assumption is changed)

	void FixedUpdate() {
		// always move the empty game object that the camera is following in between all four players
		followObj.transform.position = FindCenterPoint (players);
	}

	// i took this from some shit i googled
	public Vector3 FindCenterPoint(GameObject[] pArray) {
		Bounds bounds = new Bounds (players [0].transform.position, Vector3.zero);

		for (int i = 1; i < pArray.Length; i++) {
			bounds.Encapsulate (pArray [i].transform.position);
		}

		return bounds.center;
	}
}
