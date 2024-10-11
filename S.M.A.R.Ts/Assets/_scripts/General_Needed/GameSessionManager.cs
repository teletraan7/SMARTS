using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using UnityEngine.Animations;

public class GameSessionManager : MonoBehaviour {


	// player swapping behaviour
	// only matters if there is one player
	public PlayerBehaviour[] players;
    public List<InfoManager.PNum> pNums = new List<InfoManager.PNum>();
	public GameObject[] playerUILabels;
	private bool swapLeft = false;
    private bool swapRight = false;
	private float timeStamp = 0.0f;
	private float swapCooldown = 0.5f;

	// security alert system stuff
	// not the best way to handle this, but easiest
	// if any changes are made to the security thresholds, need to update the Animator parameters and transitions
	public int currentSecurity;
	public int maxSecurity = 100;
	public int securityYellow = 33;
	public int securityRed = 66;
	public Animator alertAnim;
	public GameObject botGen;
	public RobotPooler botPooler;

	public GameObject ProcGen;

    public List<PlayerBehaviour> OnTheBench = new List<PlayerBehaviour>();
    public GameObject LeftPlayer;
    public GameObject RightPlayer;

    public int PlayerCount;
    public bool SwapDisabled;


	void Awake() {
        Time.timeScale = 1f;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
		currentSecurity = 0;
		// initialize ftc
		FloatingTextController.Initialize();

		// Get the Rewired Player object for this player and keep it for the duration for the character's lifetime
		// Code will need to be added in addition for 2-player and 4-player, with booleans to gate which control type is active
		// You can set up new control schemes by following the first couple of steps in this guide http://guavaman.com/projects/rewired/docs/QuickStart.html
	
        pNums = InfoManager.Info.pNum;
		
		

        PlayerCount = pNums.Count;
        
        GetPlayerInfo();
		// Initialize UI
		// counter to loop through player UI panels
		StartCoroutine (SlightDelay ());
	}

    private void GetPlayerInfo ()
    {
        for (int i = 0; i < pNums.Count; i++)
        {
            /*Debug.Log("Rewired ID " + pNums[i].ID + " then player id " + pNums[i].PlyrNum);
            foreach (int c in pNums[i].CharacterIDs)
            {
                Debug.Log("CharacterID is " + c.ToString());
            }
            foreach (bool b in pNums[i].Benched)
            {
                Debug.Log("Benched is " + b.ToString());
            }
            foreach (bool l in pNums[i].isLeft)
            {
                Debug.Log("isLeft is " + l.ToString());
            }
            */
            int counter = 0;
            foreach (int ID in pNums[i].CharacterIDs)
            {
                //Debug.Log("Character ID " + ID + " the benched " + pNums[i].Benched[counter] + " then the isleft " + pNums[i].isLeft[counter]);
                players[ID].playerId = pNums[i].ID;
                players[ID].PlayerBenched = pNums[i].Benched[counter];
                players[ID].isLeft = pNums[i].isLeft[counter];
                if (players[ID].PlayerBenched)
                {
                    players[ID].BenchThisPlayer(true);
                    OnTheBench.Add(players[ID]);
                }
                if (!players[ID].PlayerBenched)
                {
                    players[ID].SetPlayer();
                    if (players[ID].isLeft)
                    {
                        LeftPlayer = players[ID].gameObject;
                    }
                    else
                    {
                        RightPlayer = players[ID].gameObject;
                    }
                }
                counter++;
            }
        }
    }


	IEnumerator SlightDelay() {
		yield return new WaitForEndOfFrame ();

		// IMPORTANT NOTE: when 2 player and 4 player mode are implemented, just set every character's isPairOn to true, disable the Swap Control inputs wherever applicable
		// IMPORTANT NOTE: when 2 player and 4 player mode are implemented, just set every character's isPairOn to true, disable the Swap Control inputs wherever applicable
		// IMPORTANT NOTE: when 2 player and 4 player mode are implemented, just set every character's isPairOn to true, disable the Swap Control inputs wherever applicable
		int i = 0;

		foreach (PlayerBehaviour p in players) {
			Text pUIText = playerUILabels[i].GetComponentInChildren<Text>();
			pUIText.text = (p.playerId + 1) + "P";

			Animator pUIanim = playerUILabels [i].GetComponent<Animator> ();
			pUIanim.SetBool ("isActive", !p.PlayerBenched);

			i++;
		}
	}

	void Update() {
		
		GetInput ();
		ProcessInput ();
	}

    private void FixedUpdate()
    {
        foreach (PlayerBehaviour p in OnTheBench)
        {
            Vector3 keepup = LeftPlayer.transform.position;
            p.gameObject.transform.position = keepup;
        }
    }

    private void GetInput() {
		if (PlayerCount == 1 && !SwapDisabled) {
			swapLeft = ReInput.players.GetPlayer(0).GetButtonDown ("SwapLeft");
            swapRight = ReInput.players.GetPlayer(0).GetButtonDown("SwapRight");
		}
	}

	private void ProcessInput() {
		if (PlayerCount == 1) {
			SwapControl ();
		}
	}

    void SwapControl()
    {
        if (swapLeft && Time.timeScale > 0)
        {
            if (timeStamp <= Time.time)
            {
                timeStamp = Time.time + swapCooldown;

                // counter to loop through player UI panels
                int UpToBat = 0;

                Vector3 spawnLocation = LeftPlayer.gameObject.transform.position;


                PlayerBehaviour p = OnTheBench[UpToBat];
                OnTheBench.Remove(p);
                p.PlayerBenched = !p.PlayerBenched;
                p.isLeft = true;
                p.gameObject.transform.position = spawnLocation;
                p.gameObject.SetActive(true);

                LeftPlayer.GetComponent<PlayerBehaviour>().PlayerBenched = true;
                LeftPlayer.SetActive(false);
                OnTheBench.Add(LeftPlayer.GetComponent<PlayerBehaviour>());
                

                //stop players from floating away on switch
                p.GetComponent<Rigidbody>().velocity = Vector3.zero;
                p.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                p.GetComponent<PlayerShocked>().shockFx.Stop();
                //stop run animation
                p.GetComponentInChildren<Animator>().SetBool("Running", false);

                int index = 0;
                int previousIndex = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].gameObject == p.gameObject)
                    {
                        index = i;
                    }
                    if (players[i].gameObject == LeftPlayer.gameObject)
                    {
                        previousIndex = i;
                    }

                }
                LeftPlayer = p.gameObject;
                
                // animate & update ui panels
                Text pUIText = playerUILabels[index].GetComponentInChildren<Text>();
                pUIText.text = (p.playerId + 1) + "P";
                Animator pUIanim = playerUILabels[index].GetComponent<Animator>();
                pUIanim.SetBool("isActive", !p.PlayerBenched);
                Animator PreviousUIanim = playerUILabels[previousIndex].GetComponent<Animator>();
                PreviousUIanim.SetBool("isActive", false);

            }
        }

        if (swapRight && Time.timeScale > 0)
        {
            if (timeStamp <= Time.time)
            {
                timeStamp = Time.time + swapCooldown;

                // counter to loop through player UI panels
                int UpToBat = 0;

                Vector3 spawnLocation = RightPlayer.gameObject.transform.position;

                PlayerBehaviour p = OnTheBench[UpToBat];
                OnTheBench.Remove(p);
                p.PlayerBenched = !p.PlayerBenched;
                p.isLeft = false;
                p.gameObject.transform.position = spawnLocation;
                p.gameObject.SetActive(true);

                RightPlayer.GetComponent<PlayerBehaviour>().PlayerBenched = true;
                RightPlayer.SetActive(false);
                OnTheBench.Add(RightPlayer.GetComponent<PlayerBehaviour>());
                

                //stop players from floating away on switch
                p.GetComponent<Rigidbody>().velocity = Vector3.zero;
                p.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                p.GetComponent<PlayerShocked>().shockFx.Stop();
                //stop run animation
                p.GetComponentInChildren<Animator>().SetBool("Running", false);

                int index = 0;
                int previousIndex = 0;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].gameObject == p.gameObject)
                    {
                        index = i;
                    }
                    if (players[i].gameObject == RightPlayer.gameObject)
                    {
                        previousIndex = i;
                    }
                }
                RightPlayer = p.gameObject;
                // animate & update ui panels
                Text pUIText = playerUILabels[index].GetComponentInChildren<Text>();
                pUIText.text = (p.playerId + 1) + "P";
                Animator pUIanim = playerUILabels[index].GetComponent<Animator>();
                pUIanim.SetBool("isActive", !p.PlayerBenched);
                Animator PreviousUIanim = playerUILabels[previousIndex].GetComponent<Animator>();
                PreviousUIanim.SetBool("isActive", false);

            }
        }

    }


    public void SwapControlThree(int PlayerID, bool LeftorRight, GameObject PlayerSwapping)
    {
        if (Time.timeScale > 0)
        {
            if (timeStamp <= Time.time)
            {
                timeStamp = Time.time + swapCooldown;

                // counter to loop through player UI panels
                int UpToBat = 0;

                Vector3 spawnLocation = PlayerSwapping.transform.position;


                PlayerBehaviour p = OnTheBench[UpToBat];
                OnTheBench.Remove(p);
                p.playerId = PlayerID;
                p.PlayerBenched = !p.PlayerBenched;
                //so you are changing the PlayerId, but not assinging the controller to the reinput players[]
                p.isLeft = LeftorRight;
               
                p.gameObject.transform.position = spawnLocation;
                p.gameObject.SetActive(true);

                PlayerSwapping.GetComponent<PlayerBehaviour>().PlayerBenched = true;
                PlayerSwapping.SetActive(false);
                OnTheBench.Add(PlayerSwapping.GetComponent<PlayerBehaviour>());


                //stop players from floating away on switch
                p.GetComponent<Rigidbody>().velocity = Vector3.zero;
                p.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                p.GetComponent<PlayerShocked>().shockFx.Stop();
                //stop run animation
                p.GetComponentInChildren<Animator>().SetBool("Running", false);

                int index = 0;
                int previousIndex = 0;
                for (int i = 0; i < players.Length; i++)
                {                   
                    if (players[i].gameObject == p.gameObject)
                    {
                        index = i;
                       
                    }
                    if (players[i].gameObject == PlayerSwapping.gameObject)
                    {
                        previousIndex = i;
                    }

                }
                p.SetPlayer();
                // animate & update ui panels
                Text pUIText = playerUILabels[index].GetComponentInChildren<Text>();
                pUIText.text = (p.playerId + 1) + "P";
                Animator pUIanim = playerUILabels[index].GetComponent<Animator>();
                pUIanim.SetBool("isActive", !p.PlayerBenched);
                Animator PreviousUIanim = playerUILabels[previousIndex].GetComponent<Animator>();
                PreviousUIanim.SetBool("isActive", false);

                

            }
        }     

    }


    // security alert stuff
    public void AdjustSecurity(int amount) {
		currentSecurity += amount;

		if (currentSecurity <= 0) {
			currentSecurity = 0;
		}

		if (currentSecurity >= maxSecurity) {
			currentSecurity = maxSecurity;
		}
        botPooler = botGen.GetComponent<RobotPooler>();
		// check and update alert widget
		alertAnim.SetInteger("securityLevel", currentSecurity);
		//adjust max num of bots
		botPooler.AdjustBots (currentSecurity);

	}

	public void SetSecurity(int amount) {
		currentSecurity = amount;

		if (currentSecurity <= 0) {
			currentSecurity = 0;
		}

		if (currentSecurity >= maxSecurity) {
			currentSecurity = maxSecurity;
		}

		// check and update alert widget
		alertAnim.SetInteger("securityLevel", currentSecurity);
		//adjust max num of bots
		botPooler.AdjustBots (currentSecurity);
	}

}
