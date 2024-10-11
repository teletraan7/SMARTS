using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallStatus : MonoBehaviour {

	public bool wallDestroyed;
	public bool standardActive;
	public bool reinforced;
	public GameObject connectedWall;

	// Use this for initialization
	void Start () {
		wallDestroyed = false;
		standardActive = true;
		reinforced = false;
	}

	void Update () {
		if (standardActive == false && wallDestroyed != true) {
			connectedWall.SetActive (true);
			this.gameObject.SetActive (false);
		}

	}
}
