using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotAI : MonoBehaviour {
	//array for pathfinding
    [HideInInspector]
	public Transform[] patrolPoints;
	//wich point the AI is going to
	public int patrolDes = 0;
	//navmeshagent
	private NavMeshAgent navAgent;

	public float chargeTime;

	public bool plyrNRange;

	private bool isDisabled;
	private botShock botshock;

	public GameObject target;

	private PlayerShocked plyrShock;
	public bool targetShocked;
    private GameObject Home;
    public bool inTutorial;
    public GameObject Terminal;
    private int halfWallDamage = 0;
    
    private bool ReadyToPatrol = false;
    private AudioSource audioSor;

	// Use this for initialization
	void OnEnable () {
        botshock = this.gameObject.GetComponent<botShock>();
        Invoke("SpawnDelay", 2);
        audioSor.Play();
    }

    private void SpawnDelay ()
    {
       if (this.gameObject.activeInHierarchy)
        {

            Home = GameObject.FindGameObjectWithTag("Home");

            patrolPoints = Home.GetComponent<PatrolPoints>().Points;

            navAgent = this.gameObject.GetComponent<NavMeshAgent>();
            //so it doesn't stop
            navAgent.autoBraking = false;
            //start patroling
            StartCoroutine(StartPatrol());

            //check if bot is disabled
            botshock = this.gameObject.GetComponent<botShock>();
            isDisabled = botshock.shocked;
            ReadyToPatrol = true;
            if (inTutorial)
            {
                //check if bot is disabled
                botshock = this.gameObject.GetComponent<botShock>();
                isDisabled = botshock.shocked;
            }
        }
    }

	public IEnumerator StartPatrol () {
		//if AI has no path the return error
		if (patrolPoints.Length == 0) {
            // Debug.Log ("No Points");
            yield break;
		}

		if (this.gameObject.activeInHierarchy) {
			//set the target destination to the current point
			navAgent.destination = patrolPoints [patrolDes].position;             
		}

		//get next point
		patrolDes = (patrolDes + 1) % patrolPoints.Length;
    }

	void FixedUpdate () {
        if (this.gameObject.activeInHierarchy && !inTutorial && ReadyToPatrol) 
        {
            //check if bot is shocked
            isDisabled = botshock.shocked;
            //if there is a target then get the plyShock
            if (target != null)
            {
                plyrShock = target.GetComponent<PlayerShocked>();
            }
            //if there is a plyrshock then check if that target is shocked
            if (plyrShock != null)
            {
                targetShocked = plyrShock.shocked;
            }
            //if the AI has a path and it is within .5f of its destination then move to the next point
            if (!navAgent.pathPending && navAgent.remainingDistance < 2f && !isDisabled)
            {
                StartCoroutine(StartPatrol());
            }
            
            else if (isDisabled)
            { //freeze bot and initiate cooldown
                navAgent.isStopped = true;
                Invoke("Resume", botshock.cooldown);
            }
            
        }
        if (inTutorial)
        {
            isDisabled = botshock.shocked;
        }
	}
	//unstun bot
	void Resume() {
        isDisabled = botshock.shocked;
        navAgent.isStopped = true;
    }

	void OnCollisionEnter (Collision other) {
		if (other.gameObject == target && !targetShocked && !isDisabled) { //check if what entered is the target, make sure its not shocked and disabled
			plyrShock.ShockPlayer (); //run shock script for player
			targetShocked = true; //this is used to prevent the bot from completely stunning the player
			return;
		}
        if (other.gameObject != target || targetShocked || isDisabled){
			return;
		}
        if (other.gameObject.tag == "HalfWall" && !isDisabled)
        {
            halfWallDamage = 0;
            StartCoroutine(TearDownThatWall(other.gameObject));
        }
	}

    private IEnumerator TearDownThatWall(GameObject halfwall)
    {
        while (halfWallDamage < 3)
        {
            yield return new WaitForSeconds(2f);
            halfWallDamage++;
        }
        if (halfWallDamage >= 3)
        {
            if (halfwall.GetComponent<BoxCollider>() != null)
            {
                halfwall.GetComponent<BoxCollider>().isTrigger = true;
            }
            if (halfwall.GetComponent<NavMeshObstacle>() != null)
            {
                halfwall.GetComponent<NavMeshObstacle>().enabled = false;
            }
            MeshRenderer[] wallmesh;

            wallmesh = halfwall.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer mr in wallmesh)
            {           
                mr.enabled = false;
            }
            BuildWalls buildwalls = GameObject.Find("Repair").GetComponent<BuildWalls>();
            buildwalls.numWalls--;

            halfWallDamage = 0;
            yield break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "door")
        {
            other.gameObject.GetComponent<Animator>().SetBool("DoorOpen", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "door")
        {
            other.gameObject.GetComponent<Animator>().SetBool("DoorOpen", false);
        }
    }
}
