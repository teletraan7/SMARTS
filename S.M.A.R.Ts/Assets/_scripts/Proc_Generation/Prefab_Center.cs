using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prefab_Center : MonoBehaviour {

	public string PrefabSize;
	//dimensions of this prefab

	public bool used;
	//check if this prefab is used already in this level

	public Transform CenterPosition;

	public GameObject[] Turrets;

	public GameObject[] Terminals;

	public bool isAI_Room;

	public GameObject[] dispensors;

	public GameObject Picked_Dispensor;

	// Use this for initialization
	void Awake () {
		CenterPosition = this.gameObject.transform;
		foreach (GameObject Terminal in Terminals) {
			Terminal.SetActive(false);
		}
		foreach (GameObject dispensor in dispensors) {
			dispensor.SetActive(false);
		}
	}

	public void Activate_Terminal (GameObject Room) {
		int J = Random.Range(0, Terminals.Length);
		GameObject terminal = Terminals[J].gameObject;
		terminal.SetActive(true);
		//activate
		WallTerminal Terminal_Script = terminal.GetComponent<WallTerminal>();
		Terminal_Script.difficulty = 3;
		//set difficulty to 4, which is the AI difficulty
		Terminal_Script.maxHacks = 3;
		//set the max number of hacks to 3
		Terminal_Script.door = Room.GetComponent<Room_Center>().Room_Door;
		//set target door for terminal to door for the AI room 
	}

	public void Dispensor_Terminal (GameObject Dispensor) {	
		int J = Random.Range(0, Terminals.Length);
		GameObject terminal = Terminals[J].gameObject;
		terminal.SetActive(true);
		//activate
		WallTerminal Terminal_Script = terminal.GetComponent<WallTerminal>();
		Terminal_Script.difficulty = 3;
		//set difficulty
		Terminal_Script.maxHacks = 1;
		if (Dispensor != null) {

		}
		terminal.GetComponent<WallTerminal>().droneDispensor = Dispensor;
	
	}
}
