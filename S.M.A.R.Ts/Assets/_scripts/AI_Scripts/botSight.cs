using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class botSight : MonoBehaviour {

    //this script is now outdated and the RobotFoV script should be used instead

	/*private Transform botTrans;
	private NavMeshAgent botNav;
	private float botFov = 90f;
	public float viewDist;
	public float angle = 20f;
	private RaycastHit hit;
	public float detectRadius;
	private Collider[] colliders;
	public RobotAI botAI;
	public bool targetAquired;
	public GameObject targetPlyr;
    public float MeshResolution;

	public List<GameObject> Possible_Targets = new List<GameObject>();

	private int Player_Mask;

	// Use this for initialization
	void Start () {
		botTrans = this.gameObject.GetComponent<Transform> ();
		//I used the transform of the bot frequently so a reference directly to it is made at the start
		botNav = this.gameObject.GetComponent<NavMeshAgent> ();
		//the bots nav agent
		botAI = this.gameObject.GetComponent<RobotAI> ();
		//the bots AI
		targetAquired = false;
		//reset target status of bot
		Player_Mask = LayerMask.GetMask("Players");
	}
	

	void FixedUpdate () {
		//make physics.overlapshpere
		colliders = Physics.OverlapSphere(botTrans.transform.position, detectRadius);
		Vector3 Direction = (transform.forward + transform.right).normalized;
		Vector3 InverseDirection = (transform.forward - transform.right).normalized;
		Debug.DrawRay(botTrans.position, (Direction * viewDist), Color.magenta);
		Debug.DrawRay(botTrans.position, (transform.forward * viewDist), Color.cyan);
		Debug.DrawRay(botTrans.position, (InverseDirection * viewDist), Color.yellow);
		//get the colliders with player tag
		int i = 0;
		while (i < colliders.Length) {
			if (colliders [i].tag == "Player") {
				GameObject plyr = colliders [i].gameObject;
				// Debug.Log (plyr.tag);

				RaycastHit hit;
				Debug.DrawRay (botTrans.transform.position, (plyr.transform.position - botTrans.transform.position) * viewDist, Color.magenta );
				Ray rayPlyr = new Ray (botTrans.transform.position, (plyr.transform.position - botTrans.transform.position) * detectRadius);
				if (Physics.Raycast (rayPlyr, out hit, viewDist, Player_Mask) && targetAquired == false) {
					Ray Postive_Angle = new Ray (botTrans.position, Direction * viewDist);
					Ray Ray_Forward = new Ray (botTrans.position, transform.forward * viewDist); 
					Ray Negative_Angle = new Ray (botTrans.position, InverseDirection * viewDist);
					//rays used to determine if the player is withing a 90 degree FoV of the bot
					Debug.DrawRay(botTrans.position, (Direction * viewDist), Color.red);
					Debug.DrawRay(botTrans.position, (transform.forward * viewDist), Color.cyan);
					Debug.DrawRay(botTrans.position, (InverseDirection * viewDist), Color.blue);
					//draw the rays
					if (hit.collider.gameObject.tag == "Player") {
						Possible_Targets.Add(hit.collider.gameObject);
						//gather any players withing the detect radius and put them in a list
					}
					int T = Random.Range(0, Possible_Targets.Count);
					GameObject Picked_Player = Possible_Targets[T].gameObject;
					//pick a player if there are multiple
					Vector3 Ply_Pos = Picked_Player.transform.position - botTrans.position;
					//the player pos relative to the bot
					float Angle_To_Player = Vector3.Angle(Ply_Pos, botTrans.forward);
					//get the angle the player is to the bots forward transform
					if (Angle_To_Player >= -45f && Angle_To_Player <= 45f) {
						//if player is within FoV
						Debug.DrawRay(botTrans.position, Ply_Pos, Color.green);
						Ray Ray_to_Target = new Ray(botTrans.position, Ply_Pos);
						RaycastHit target;
						//create new ray to check if target is visable 
						if (Physics.Raycast (Ray_to_Target, out target, viewDist) && target.collider.gameObject.tag == "Player") {
							//draw the ray to the target player and check if target can be seen
							GameObject Player_Targeted = hit.collider.gameObject;
							targetPlyr = Player_Targeted;
							//tell drone it has a player
							targetAquired = true;
							//set target bool so bot no longer looks for targets
							StartCoroutine (FollowTarget(Player_Targeted));
							//drone moves towards player
							Possible_Targets = new List<GameObject>();
							//clear the list
						} else {
							Possible_Targets = new List<GameObject>();
							targetAquired = false;
							//if not then reset the possible targets so the drone can continue looking for one
						}
					} else if (hit.collider.tag == "Wall") {
						targetAquired = false;
						//if drone ray hits a wall then call it off
					}
				}
			} 
			i++;
		}
	}

	IEnumerator FollowTarget (GameObject Target) {
		botAI.target = Target;
		while (targetAquired == true && !botAI.targetShocked) {
			botTrans.transform.LookAt (Target.transform.position);
			botNav.SetDestination (Target.transform.position);
			yield return new WaitForFixedUpdate ();
		} 

		if (targetAquired == false || botAI.targetShocked) {
			botNav.SetDestination (botAI.patrolPoints [botAI.patrolDes].position);
			yield break;
		}
	}*/

}
