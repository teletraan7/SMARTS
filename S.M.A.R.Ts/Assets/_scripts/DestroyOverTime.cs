using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {

	public float delay = 1.0f;

	void Start() {
		Destroy (gameObject, delay);
	}
}
