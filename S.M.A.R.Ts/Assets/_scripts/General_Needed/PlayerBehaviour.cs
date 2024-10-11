using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour {

	public int playerId = 0;	// 0 = player one, 1 = player two, 2 = player 3, so on and so forth
	public float moveSpeed = 3.0f;

	private Player player;		// for Rewired asset
	private Rigidbody rb;

	// START solo (one player only mode)
	public bool isLeft = false;	// false = player controlled by left stick, true = player controlled by right stick
	private Vector3 moveVectorLeft;
	private Vector3 moveVectorRight;
    public bool PlayerBenched = false;
	// END solo (one player only mode)

	// player specific actions like demolitions, repair, etc.
	private bool actionLeft = false;
	private bool actionRight = false;
	public string actionType;
	private PlayerActions playerAction;

	private bool actionLeft2 = false;
	private bool actionLeft3 = false;
	private bool actionRight2 = false;
    private bool actionRight3 = false;


	//will store all players for the quick res function - jon
	public GameObject[] players;
	//plyrShocked script

	// quick fix
	private float angle = 33;

	private PlayerShocked plyrshock;
	//get the ispairon bools for demo and hacking
	private Transform plyrTrns;

	// game session manager
	public GameSessionManager gsm;

	public Animator anim;

	void Awake() {
		// Get the Rewired Player object for this player and keep it for the duration for the character's lifetime
		// Code will need to be added in addition for 2-player and 4-player, with booleans to gate which control type is active
		// You can set up new control schemes by following the first couple of steps in this guide http://guavaman.com/projects/rewired/docs/QuickStart.html
		player = ReInput.players.GetPlayer(playerId);

		// reference to player's ability (demolitions, repair, etc.)
		playerAction = GetComponentInChildren<PlayerActions>();

		// Get the character controller
		rb = GetComponent<Rigidbody>();

		//get all of the players
		players = GameObject.FindGameObjectsWithTag ("Player");
		//get plyrshock
		plyrshock = gameObject.GetComponent<PlayerShocked>();
        plyrTrns = this.gameObject.GetComponent<Transform>();

		// get game session manager settings
		gsm = GameObject.FindWithTag("GameController").GetComponent<GameSessionManager>();

	}

	public void SetPlayer() {
		player = ReInput.players.GetPlayer(playerId);
	}

	// Rewired runs on Update, you can probably find a way to switch it to FixedUpdate but i'm too lazy to figure it out
	void Update() {

		if (!PlayerBenched) {
			GetInput ();
			ProcessInput ();
		}                

		
	}

	void FixedUpdate() {
		if (!PlayerBenched) {
			MovePlayer ();
		}
    }

	private void GetInput() {
		// Get the input from the Rewired Player.  All controllers that the Player owns will contribute, so it doesn't matter
		// whether the input is coming from a joystick, the keyboard, mouse, or custom controller
		// You can set up new control schemes by following the first couple of steps in this guide http://guavaman.com/projects/rewired/docs/QuickStart.html

		if (!PlayerBenched) {
			// player movement + player specific action (demolition, hacking, etc.)
			if (isLeft) {
				// player input controlled by left stick
				moveVectorLeft.x = player.GetAxis ("Move Horizontal Pair Left");
				moveVectorLeft.z = player.GetAxis ("Move Vertical Pair Left");
				actionLeft = player.GetButtonDown ("Action Pair Left");
				actionLeft2 = player.GetButtonSinglePressHold ("Secondary Action Left");
				actionLeft3 = player.GetButtonSinglePressUp ("Secondary Action Left");
			} else if (!isLeft) {
				// player input controlled by right stick
				moveVectorRight.x = player.GetAxis ("Move Horizontal Pair Right");
				moveVectorRight.z = player.GetAxis ("Move Vertical Pair Right");
				actionRight = player.GetButtonDown ("Action Pair Right");
                actionRight2 = player.GetButtonSinglePressHold("Secondary Action Right");
                actionRight3 = player.GetButtonSinglePressUp("Secondary Action Right");
            }
            if(gsm.PlayerCount == 3 )
            {
                if (player.GetButtonDown("SwapLeft") || player.GetButtonDown("SwapRight"))
                {
                    gsm.SwapControlThree(playerId, isLeft, this.gameObject);
                }
            }
		}
	}

    public void BenchThisPlayer(bool Benched)
    {
        if(Benched == true)
        {
            PlayerBenched = Benched;
            this.gameObject.SetActive(false);
        }
        if (Benched == false)
        {
            PlayerBenched = Benched;
            this.gameObject.SetActive(true);
        }
    }

    private void ProcessInput() {
		PerformAct ();
	}

	void MovePlayer() {
		if (isLeft) {
			if (moveVectorLeft.x != 0.0f || moveVectorLeft.z != 0.0f) {
				moveVectorLeft = Quaternion.Euler(0, -angle, 0) * moveVectorLeft;
				rb.velocity = moveVectorLeft * moveSpeed * Time.deltaTime;
				transform.rotation = Quaternion.LookRotation (moveVectorLeft, Vector3.up);
				anim.SetBool("Running", true);
			} else {
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				anim.SetBool("Running", false);
			}
		} else if (!isLeft) {
			if (moveVectorRight.x != 0.0f || moveVectorRight.z != 0.0f) {
				moveVectorRight = Quaternion.Euler(0, -angle, 0) * moveVectorRight;
				rb.velocity = moveVectorRight * moveSpeed * Time.deltaTime;
				transform.rotation = Quaternion.LookRotation (moveVectorRight, Vector3.up);
				anim.SetBool("Running", true);
			} else {
				rb.velocity = Vector3.zero;
				rb.angularVelocity = Vector3.zero;
				anim.SetBool("Running", false);
			}
		}
	}

	// perform action attached to character (demolitions, repair, etc.)
	void PerformAct() {
		if (actionLeft && isLeft) {
			playerAction.StuffHappens (actionType);
		} else if (actionLeft2 && isLeft) {
			playerAction.Ult (actionType);
		} else if (actionLeft3 && isLeft) {
			playerAction.endUlt (actionType);
		}

		if (actionRight && !isLeft) {
			playerAction.StuffHappens (actionType);
		} else if (actionRight2 && !isLeft) {
			playerAction.Ult (actionType);
		}else if (actionRight3 && !isLeft) {
			playerAction.endUlt (actionType);
		} 
	}
}
