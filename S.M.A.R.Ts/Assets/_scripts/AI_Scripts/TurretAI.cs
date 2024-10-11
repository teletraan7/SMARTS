using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour {
    /* This script is now out of date. Use RobotFoV script instead for all turrets and drones
	//the range of the ray
	public float detectRange;
	//transform of the turret
	private Transform trn;
	//start point of ray
	public GameObject lzrStart;
	//rotation speed of turret
	public float rotSpeed;
	//bool used to see if player is in site
	public bool plyrNsite = false;
	//transform of player hit by ray
	private Transform plyr;
	
	//this is all used to hit/shock player. we need a bool, the particle systems, the playershocked script, and a float
	public bool firing;
	public ParticleSystem charge;
	public ParticleSystem bolt;
	private PlayerShocked playershocked;
	private float cooldownTime;
    public GameObject TurretWhole;
	private botShock botshock;
	private bool isDisabled;

	public int alertRaise = 10;
	
	// Use this for initialization
	void Awake () {
		//get transform for turret
		trn = this.GetComponentInChildren<Transform>();
        botshock = TurretWhole.GetComponent<botShock>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		isDisabled = botshock.shocked;

		//if player is not in site then scan the area by rotating
		//however if the player is in site then continue to look at the player
		if (plyrNsite == false && isDisabled == false) {
			trn.Rotate (Vector3.up, Time.deltaTime * rotSpeed, Space.World);
		} else if (plyrNsite == true && isDisabled == false) {
			trn.LookAt (plyr);
		} else if (isDisabled) {
			trn.Rotate (new Vector3 (0f, 0f, 0f));
		}
		//what we will call to get the hits
		RaycastHit hit;
		//our ray, starts at lazerstart object and points forward at the set range
		Ray lzr = new Ray (lzrStart.transform.position,  -lzrStart.transform.up * detectRange);
		//draw the same ray for debugging
		Debug.DrawRay (lzrStart.transform.position, -lzrStart.transform.up * detectRange);
		//if the ray gets a hit and it is a player, then set plyrNsite bool to true, and set plyr to equal to transform of the hit object
		//if the player hides behind a wall the turret will stop following and continue to scan its area
		if (Physics.Raycast (lzr, out hit) && !isDisabled) {
			//make sure it taged a player, is within range, and the cooldown is off
			if (hit.collider.tag == "Player" && hit.distance <= detectRange && Time.time > cooldownTime && !isDisabled) {
				plyrNsite = true;
				plyr = hit.transform;
				playershocked = hit.collider.gameObject.GetComponent<PlayerShocked> ();
				if (firing == false) {
					Charge ();
				}
			} if (hit.collider.tag == "Wall" || hit.collider.tag == "ReinforcedWall" || isDisabled) {
				plyrNsite = false;
				firing = false;
			}
		}
	}
	
	//charge the shot and fire charge animation, firing true lets the Turret know it is firing already
	void Charge () {

		firing = true;
		FloatingTextController.CreateFloatingText ("Intruder detected, Alert + " + alertRaise.ToString(), transform);

		// add to security alert after bomb use
		GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();

		gsm.AdjustSecurity (alertRaise);
		charge.Play ();
		Invoke ("Fire", 3f);
	}
	
	//fire the shot, send for the shockplayer script in playershocked, start cooldown
	void Fire() {

		if (plyrNsite == true && !isDisabled) {
			bolt.Play ();
			playershocked.Invoke("ShockPlayerFromTurret", 0.25f);
			Invoke ("Cooldown", 5f);
		} else if (plyrNsite == false || isDisabled) {
			bolt.Stop ();
			charge.Stop ();
			return;
		}
	}
	
	//cooldown for turret of five seconds, also make the turn scan the area, and tell it it is no longer firing
	void Cooldown() {
		cooldownTime = Time.time + 5f;
		plyrNsite = false;
		firing = false;
	}
    */
}