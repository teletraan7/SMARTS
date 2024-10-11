using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadScript : MonoBehaviour {

	// freeze rotation of worldspace ui element to ignore player rotation
	Quaternion defaultRotation;

	void Awake() {
		defaultRotation = transform.rotation;
	}

	void Update () {
		transform.rotation = defaultRotation;
	}
}
