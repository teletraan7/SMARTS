using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room_Center : MonoBehaviour {

	public string RoomSize;
	//this string will be the room dimesnsions like 6x3
	public bool Filled;
	//this bool will check if the room has already or needs to be filled with a prefab

	public Transform ThisCenter;
	//center of the room

	public GameObject ChossenPrefab;
	//prefab chossen

	public GameObject Room_Door;

	public GameObject Room_Door_2;

	public GameObject ProcGenerator;

	public GameObject Instace_Of_Prefab;

	public bool has_AI_Prefab;


	// Use this for initialization
	void Start () {
		ThisCenter = this.gameObject.transform;
		//set the transform as the center point
	}
	
	public void GeneratePrefab () {
		GameObject Prefab_Instance = Instantiate(ChossenPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
		//spawn the prefab at this rooms center
		Instace_Of_Prefab = Prefab_Instance.gameObject;
		Filled = true;
		//set this room to being filled
		ProcGenerator.GetComponent<ProcGenManager>().Count_Rooms();
		//add to room count
	}

	
}
