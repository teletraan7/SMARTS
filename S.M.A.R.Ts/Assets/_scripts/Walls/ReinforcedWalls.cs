using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforcedWalls : MonoBehaviour {

	public int bombsHit;
	public int bombsNeeded = 3;
	public GameObject panel1;
	public GameObject panel2;
	public GameObject panel3;

    // Update is called once per frame
    void Update () {
		if (bombsHit == 1) {
			panel1.SetActive (false);
            panel2.SetActive(false);
            panel3.SetActive(false);
        }
	}
}
