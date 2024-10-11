using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildWalls : MonoBehaviour {

	public GameObject wallPref;
	public float numWalls;
	public float maxWalls;
	public bool canBuild;

	// Use this for initialization
	void Start () {
		canBuild = true;
	}
	
	

	public void HalfWall() {
		if (numWalls < maxWalls) {
            Vector3 plyPos = this.gameObject.transform.position;
            Vector3 plyDir = this.gameObject.transform.forward;
            Vector3 spawnPos = plyPos + plyDir * 2;
			Instantiate (wallPref, spawnPos, this.gameObject.transform.rotation);
			numWalls++;
		}
	}
}
