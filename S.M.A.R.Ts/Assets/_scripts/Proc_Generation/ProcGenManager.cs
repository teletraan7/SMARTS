using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ProcGenManager : MonoBehaviour {

	public GameObject[] Rooms;
	//all of the room center objects will be here
	public GameObject[] Prefabs;
	//the prefabs
	public List<GameObject> PrefabTemps = new List<GameObject>();
	//prefabs as new gameobjects so that they can be manipulated properly
	public List<GameObject> PossiblePrefabs = new List<GameObject>();
	//prefabs after they have been filtered through

	private List<GameObject> Room_Terminals = new List<GameObject>();
	//list will contain all of the terminals spawned in rooms

	private bool AI_Room_Filled;
	//check if an AI room needs to be spawned

	public bool AI_Door_Terminal_Filled;
	//checki if door terminal for the ai room is spawned

	private bool Dispensor_Terminal_Filled;
	//check if terminal to control dispensor needs to be spawned

	public List<GameObject> AI_Prefabs = new List<GameObject>();
	//used to sort through ai prefabs

	public int Room_Count = 0;

	public int Room_Amount;

	public GameObject GameManager;

	public GameObject Robot_Dispensor;
	private GameObject choosen_Dispens;

	private GameObject AI_Room;

	// Use this for initialization
	void Start () {
		foreach (GameObject Prefab in Prefabs) {
			Prefab.GetComponent<Prefab_Center>().used = false;
			//manipulating an instance of a prefab changes the base prefab. So when they are set to used they remain that way when restarted unless told to be false
			GameObject PrefabObj = Prefab.gameObject;
			if (PrefabObj.GetComponent<Prefab_Center>().isAI_Room == true) {
				AI_Prefabs.Add(PrefabObj);
			} else if (PrefabObj.GetComponent<Prefab_Center>().isAI_Room != true) {
				PrefabTemps.Add(PrefabObj);
			}
		}
		Spawn_AI_Room();
		foreach (GameObject Room in Rooms) {
			PossiblePrefabs = new List<GameObject>();
			//reset the list of possible prefabs in each room
			bool RoomFilled = Room.GetComponent<Room_Center>().Filled;
			//get filled bool from rooms 
			if (RoomFilled != true) { //check if filled
				Room_Center RoomScript = Room.GetComponent<Room_Center>();
				//make a reference to the script for ease of use
				string RoomSize  = RoomScript.RoomSize; 
				//make reference to room size for ease of use
				foreach (GameObject prefab in PrefabTemps) {
					if(prefab.GetComponent<Prefab_Center>().PrefabSize == RoomSize && prefab.GetComponent<Prefab_Center>().used != true) {
						//if the prefab is the proper size and not used then 
						PossiblePrefabs.Add(prefab);
						//add to the list of possible prefabs for this room
					}
				}
				int I = Random.Range(0, PossiblePrefabs.Count);
				//generate a random whole number between zero and the length of the list
				RoomScript.ChossenPrefab = PossiblePrefabs[I].gameObject;
				//set the choosenprefab to the prefab with the index of I
				RoomScript.GeneratePrefab(); 
				//generate the room
				PossiblePrefabs[I].gameObject.GetComponent<Prefab_Center>().used = true;
				//set the prefab use to true so that way it will be excluded from any possible prefab lists in the following rooms
				GenerateTurrets(Room);

			}
		}

		Terminal_Link_Hall();
	}

	void Spawn_AI_Room () {
		List<GameObject> Rooms_With_AI = new List<GameObject>();
		foreach (GameObject Room in Rooms) {
			if (Room.GetComponent<Room_Center>().has_AI_Prefab == true) {
				Rooms_With_AI.Add(Room);
			}
		}
		int A = Random.Range(0, Rooms_With_AI.Count);
		AI_Room = Rooms_With_AI[A].gameObject;
		//get a random room
		GameObject Prefab = null;
		foreach (GameObject AI_Prefab in AI_Prefabs) {
			if (AI_Prefab.GetComponent<Prefab_Center>().PrefabSize == AI_Room.GetComponent<Room_Center>().RoomSize) {
				Prefab = AI_Prefab.gameObject;
				//find its matching AI_Prefab
			}
		}

		AI_Room.GetComponent<Room_Center>().ChossenPrefab = Prefab.gameObject;
		//set the chossen prefab
		AI_Room.GetComponent<Room_Center>().GeneratePrefab();
		//spawn the AI room
		Prefab.GetComponent<Prefab_Center>().used = true;
		//mark prefab as used
		AI_Room_Filled = true;
		//mark room as filled
		GenerateTurrets(AI_Room);
		//spawn turrets for the room
	}

	void GenerateTurrets (GameObject room) {
        
		//places turrets in rooms
		GameObject choosen = room.GetComponent<Room_Center>().ChossenPrefab;
		//get the prefab that was picked
		GameObject [] turrets = choosen.GetComponent<Prefab_Center>().Turrets;
		//get the possible turret locations
		foreach (GameObject turret in turrets) {
			turret.SetActive(false);

        }
		//make sure that they are set to inactive
		int I = Random.Range(0, turrets.Length);
		turrets[I].SetActive(true);

		//activate which ever one has an index of I
	}

	public void Count_Rooms () {
		//this is the part that checks that all of the prefabs are spawned and placed before spawning and linking the terminals to the AI

		Room_Amount = Rooms.Length;

		Room_Count++;

		if (Room_Count >= Room_Amount) {
			GenerateTerminals();
			//once all roooms are filled then spawn the terminals
		}

	}

	void GenerateTerminals () {
		List<GameObject> Filtered_Rooms = new List<GameObject>();
		//list will be used to filter out the AI room
		GameObject AI_Room = null;

		foreach (GameObject Room in Rooms) {
			GameObject prefab = Room.GetComponent<Room_Center>().ChossenPrefab.gameObject;
			//get the prefab

			if (Room.GetComponent<Room_Center>().ChossenPrefab.GetComponent<Prefab_Center>().isAI_Room == false) {
				Filtered_Rooms.Add(Room);
				//add rooms that arent the AI room to the filtered list
			}

			if (Room.GetComponent<Room_Center>().ChossenPrefab.GetComponent<Prefab_Center>().isAI_Room == true) {
				AI_Room = Room.gameObject;

				//reference to the AI room 
			}
		}

		int I = Random.Range(0, Filtered_Rooms.Count);
		GameObject Room_Picked = Filtered_Rooms[I].gameObject;
		GameObject choosen = Filtered_Rooms[I].GetComponent<Room_Center>().Instace_Of_Prefab.gameObject;
		//choose a random room
		choosen.GetComponent<Prefab_Center>().Activate_Terminal(AI_Room);
		AI_Room_Filled = true;
		Filtered_Rooms.Remove(Room_Picked);
		Spawn_Robot_Dispensor (Filtered_Rooms);
	}

	void Terminal_Link_Hall () {
		//link terminals to their proper doors and set their needed values
		foreach(GameObject Room in Rooms) {
			if (Room.GetComponent<Room_Center>().Room_Door != null) {
				GameObject Door = Room.GetComponent<Room_Center>().Room_Door;
				//the door for that room
				if (Door.GetComponent<Door_Info>().Door_Terminal != null) {
					GameObject Terminal = Door.GetComponent<Door_Info>().Door_Terminal;
					//the terminal for that door

					if (Room.GetComponent<Room_Center>().ChossenPrefab.GetComponent<Prefab_Center>().isAI_Room) {
						//check if the room is a room with an AI terminal
						Terminal.SetActive(false);
						//if so set that door terminal to false so that the player will have to open a door with a terminal in another room
					} else {
						WallTerminal Terminal_Script = Terminal.GetComponent<WallTerminal>();
						//script for the terminal
						float I = Random.Range(1, 3);
						//get a random number
						Terminal_Script.difficulty = I;
						//apply that to the difficulty
						Terminal_Script.maxHacks = 1;
					}
				}
			}
		}
	}

	void Spawn_Robot_Dispensor (List<GameObject> Filtered_Rooms) {

		int D = Random.Range(0, Filtered_Rooms.Count);
		GameObject Dispensor_Room = Filtered_Rooms[D].gameObject;
		GameObject Dispensor_Prefab = Dispensor_Room.GetComponent<Room_Center>().Instace_Of_Prefab.gameObject;
		int E = Random.Range(0, Dispensor_Prefab.GetComponent<Prefab_Center>().dispensors.Length);
		GameObject Chossen_Dispensor = Dispensor_Prefab.GetComponent<Prefab_Center>().dispensors[E].gameObject;
		Chossen_Dispensor.SetActive(true);
		Robot_Dispensor = Chossen_Dispensor;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameSessionManager>().botGen = Chossen_Dispensor;
		Dispensor_Prefab.GetComponent<Prefab_Center>().Picked_Dispensor = Chossen_Dispensor;
		Filtered_Rooms.Remove(Dispensor_Room);
		int F = Random.Range(0, Filtered_Rooms.Count);
		GameObject Dispensor_Terminal_Room = Filtered_Rooms[F].gameObject;
		GameObject Dispensor_Terminal_Room_Prefab = Dispensor_Terminal_Room.GetComponent<Room_Center>().Instace_Of_Prefab;
		Dispensor_Terminal_Room_Prefab.GetComponent<Prefab_Center>().Dispensor_Terminal(Chossen_Dispensor);
	}

}
