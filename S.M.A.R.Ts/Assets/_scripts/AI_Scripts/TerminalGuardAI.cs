using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerminalGuardAI : MonoBehaviour { //used specifically for the terminal guard drones. will be a new list of its own

    private NavMeshAgent navAgent;

    public float chargeTime;

    public bool plyrNRange;

    private bool isDisabled;
    private botShock botshock;

    public GameObject target;

    private PlayerShocked plyrShock;
    public bool targetShocked;
    
    public bool inTutorial;
    public GameObject Terminal;
    private bool ReadyToPatrol;
    private bool AtTerminal;
    public float PatrolSpeed;
    private int halfWallDamage = 0;
    

    // Use this for initialization
    void OnEnable()
    {
        botshock = this.gameObject.GetComponent<botShock>();
        Invoke("CheckIfGuard", 2);
    }

    private void CheckIfGuard()
    {
        if (this.gameObject.activeInHierarchy)
        {
            navAgent = this.gameObject.GetComponent<NavMeshAgent>();
            //so it doesn't stop
            navAgent.autoBraking = false;
            //start patroling
            GoToTerminal();
            //check if bot is disabled
            botshock = this.gameObject.GetComponent<botShock>();
            isDisabled = botshock.shocked;
            ReadyToPatrol = true;
            Debug.Log("TESTING");
            //check if bot is disabled
            botshock = this.gameObject.GetComponent<botShock>();
            isDisabled = botshock.shocked;
        }
    }

    private void GoToTerminal()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Debug.Log("Going to Terminal");
            navAgent.destination = Terminal.transform.position;

        }
    }

    void FixedUpdate()
    {
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
            
            if (AtTerminal && !isDisabled && !navAgent.pathPending)
            {
                
                this.gameObject.transform.RotateAround(Terminal.transform.position, Vector3.up, Time.deltaTime * PatrolSpeed);
            }
            if (isDisabled)
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
    void Resume()
    {
        isDisabled = botshock.shocked;
        navAgent.isStopped = true;
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject == target && !targetShocked && !isDisabled)
        { //check if what entered is the target, make sure its not shocked and disabled
            plyrShock.ShockPlayer(); //run shock script for player
            targetShocked = true; //this is used to prevent the bot from completely stunning the player
            return;
        }
        if (other.gameObject != target || targetShocked || isDisabled)
        {
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
        if (other.gameObject.tag == "AI")
        {
            navAgent.isStopped = true;
            AtTerminal = true;
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


