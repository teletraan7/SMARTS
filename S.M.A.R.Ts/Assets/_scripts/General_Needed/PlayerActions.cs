using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Animations;

public class PlayerActions : MonoBehaviour {

	//stuff to interact with
	public GameObject wall;
	private GameObject security;
	private GameObject toolBox;
	//touching bool will be used to check if player is touching a destructable wall
	public bool touching;

	public MeshRenderer[] wallmesh; //list for walls meshs
	public GameObject techPlyr; //hack player
	private HackUlt hackUlt; //hack ult
	private HackTerminal hackTerminal; //hack standard
	public GameObject demoPlyr; //demolitions player
	private DemolitionBomb demobomb; //demo standard
	public GameObject supportPlyr; //support player
	private BuildWalls buildWall; //support standard
	private SupportUlt supUlt; //support ult
	public GameObject defenderPlyr; //security player
	private DefenderUlt defenUlt; //security ult

	void Awake() {
		hackUlt = techPlyr.gameObject.GetComponent<HackUlt> ();
		hackTerminal = techPlyr.gameObject.GetComponent<HackTerminal> ();
		demobomb = demoPlyr.gameObject.GetComponent<DemolitionBomb> ();
		buildWall = supportPlyr.GetComponent<BuildWalls> ();
		supUlt = supportPlyr.GetComponent<SupportUlt> ();
		defenUlt = defenderPlyr.GetComponent<DefenderUlt> ();

	}

	// repair & hacking
	void OnTriggerEnter (Collider other) {
		//if the object is a wall
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "ReinforcedWall" || other.gameObject.tag == "doorWay") {
			//set touching
			touching = true;
		}
		// detect hackable
		if (other.gameObject.tag == "hackable" || other.gameObject.tag == "AI") {
			//set security
			security = other.gameObject;
			//set touching
			touching = true;
            //make the terminal glow
            if (security.tag == "hackable" && this.gameObject.name == "Hacking Action")
            {
                MeshRenderer Mr = security.gameObject.transform.Find("Terminal").GetComponent<MeshRenderer>();
                Mr.material.SetFloat("_Intensity", .7f);
            }
		}
	}

	// repair & hacking
	void OnTriggerExit (Collider other) {
		// undetect wall
		if (other.gameObject.tag == "Wall" || other.gameObject.tag == "ReinforcedWall" || other.gameObject.tag == "doorWay") {
			touching = false;
		}
		// undetect hackable
		if (other.gameObject.tag == "hackable" || other.gameObject.tag == "AI") {
            //set terminial back to normal
            if (security.tag == "hackable" && this.gameObject.name == "Hacking Action")
            {

                MeshRenderer Mr = security.gameObject.transform.Find("Terminal").GetComponent<MeshRenderer>();
                Mr.material.SetFloat("_Intensity", 0f);
            }
            security = null;
			touching = false;             
        }
	}

	public void StuffHappens(string actionType) {
		if (actionType == "Demolitions") {
			demobomb.PlaceBomb ("bombs", transform.position, Quaternion.identity);
		} else if (actionType == "Repair") {
			if (touching && wall != null) {
               
			} else if (!touching) {
				buildWall.HalfWall ();
			}
		} else if (actionType == "Hacking") {
			if (touching && security != null) {
				hackTerminal.checkHack (security);
			}
		} else if (actionType == "Defender") {
			this.gameObject.GetComponent<DefenderHit> ().Hit ();
		}
	}

	public void Ult (string actionType) {
		if (actionType == "Demolitions") {
			return;
		} else if (actionType == "Repair") {
			supUlt.Rez ();
		} else if (actionType == "Hacking") {
			hackUlt.StartUlt ();
		} else if (actionType == "Defender") {
			defenUlt.Ult ();
		}
	}

	public void endUlt (string actionType) {
		hackUlt.ultStop ();
	}
}
