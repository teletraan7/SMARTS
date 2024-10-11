using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WallTerminal : MonoBehaviour {

	public Material firewallDown;

	public GameObject door;
	public GameObject door2;
	public GameObject droneDispensor;
	public float difficulty;
	public GameObject AITerminal1;
	public GameObject AITerminal2;
	public GameObject AITerminal3;

	//AI will take multiple hacks to fix
	public float numOhacks = 0f;
	public float maxHacks;

 

    void Update() {
		if (numOhacks == 1) {
			AITerminal1.GetComponent<Renderer> ().material = firewallDown;
		}
		if (numOhacks == 2) {
			AITerminal2.GetComponent<Renderer> ().material = firewallDown;
		} 
	}

	public void AIrepaired () {
		AITerminal3.GetComponent<Renderer> ().material = firewallDown;
		Invoke ("Restart", 3f);
	}

	void Restart () {
        PointsManager psm = GameObject.FindWithTag("GameController").GetComponent<PointsManager>();
        psm.CalculateScore();
	}
}
