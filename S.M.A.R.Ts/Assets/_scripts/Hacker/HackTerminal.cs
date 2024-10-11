using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

public class HackTerminal : MonoBehaviour {

    private PointsManager psm;

    private GameObject door;
	private GameObject door2;
	private GameObject drone;
	private float difficulty;
	public Slider hackSlider;
	private float value;
	private bool touch;
	public GameObject techAction;
	private PlayerActions plyrAct;
	public GameObject background;
	private Image backImg;
	private bool hacking;
    public AudioClip hackSound;
    private AudioSource audioSor;

	// ben stuff
	public GameObject sliderObj;
	public int alertRaise = 5;
	public GameObject hackFx;

	// cooldown to prevent hacking spam
	public float hackCooldown = 0.33f;
	private float timeStamp = 0.0f;

	//this will hold the random range to hack the AI
	private float goal = 0f;

	private float numOhacks;
	private float maxHacks = 3f;

 

	// Use this for initialization
	void Start () {
		hacking = false;
		value = hackSlider.value;
		plyrAct = techAction.GetComponent<PlayerActions> ();
		backImg = background.GetComponent<Image> ();
		sliderObj.SetActive (false);
        psm = GameObject.FindWithTag("GameController").GetComponent<PointsManager>();
        audioSor = this.gameObject.GetComponent<AudioSource>();

    }

	void FixedUpdate () {
		touch = plyrAct.touching;
        
	}

 

	//check if player is currently hacking, also get terminal
	public void checkHack(GameObject Terminal) {
		if (Terminal.tag == "hackable") {
			GameObject wallTerminal = Terminal.gameObject;
			if (hacking == false) { //if not hacking then start hack
				startHack (wallTerminal);
			} else if (hacking == true) { //if hacking then finish hack
				finishHack (wallTerminal);
			}
		} else if (Terminal.tag == "AI") {
			GameObject AI = Terminal.gameObject;
			if (hacking == false) { //if not hacking then start hack
				AIhack (AI);
			} else if (hacking == true) { //if hacking then finish hack
				FinishAI (AI);
			}
		}
	}

	void startHack(GameObject Terminal) {
		if (Time.time >= timeStamp) {
			timeStamp = Time.time + hackCooldown;
            audioSor.clip = hackSound;
            audioSor.loop = false;
            audioSor.Play();
			// UI feedback
			FloatingTextController.CreateFloatingText ("Hacking...", transform);
			sliderObj.SetActive (true);

			hackSlider.enabled = true;
			hacking = true;
			difficulty = Terminal.GetComponent<WallTerminal> ().difficulty;
			door = Terminal.GetComponent<WallTerminal> ().door;
			door2 = Terminal.GetComponent<WallTerminal> ().door2;
			drone = Terminal.GetComponent<WallTerminal> ().droneDispensor;

			if (difficulty == 3) {
				StartCoroutine (SliderValue ());
			} else if (difficulty == 2) {
				StartCoroutine (SliderValue ());
			} else if (difficulty == 1) {
				StartCoroutine (SliderValue ());
			} else if (difficulty <= 0 || difficulty >= 4) {
				Debug.Log ("Difficulty level" + " " + difficulty + " " + "does not exist");
				return;
			}
		}
	}

	void AIhack(GameObject AiTerminal) {
		if (Time.time >= timeStamp && numOhacks < maxHacks) {
			timeStamp = Time.time + hackCooldown;

			// UI feedback
			FloatingTextController.CreateFloatingText ("Hacking Firewall " + (numOhacks + 1f).ToString() + " out of " + maxHacks.ToString(), transform);
			sliderObj.SetActive (true);

			hackSlider.enabled = true;
			hacking = true;
			difficulty = AiTerminal.GetComponent<WallTerminal> ().difficulty;

			if (difficulty == 4) {
                audioSor.clip = hackSound;
                audioSor.loop = false;
                audioSor.Play();
                goal = Random.Range (1f, 80f);
				StartCoroutine (SliderValue ());
			}else if (difficulty < 4 || difficulty >= 5) {
				Debug.Log ("Difficulty level" + " " + difficulty + " " + "does not exist for AI");
				return;
			}
		}
	}

	IEnumerator SliderValue() {
		while (value > hackSlider.minValue) {
			value = value - 1f;
			hackSlider.value = value;
			yield return new WaitForFixedUpdate();

			if (difficulty == 4) {
				//change slider color to indicate when to press shift again
				if (value <= (goal + 20f) && value > goal) {
					backImg.color = new Color32 (0, 255, 44, 255);
				} else { 
					backImg.color = new Color32 (255, 0, 0, 255);
				}
			}	else if (difficulty == 3) {
				//change slider color to indicate when to press shift again
				if (value <= 45f && value > 30f) {
					backImg.color = new Color32 (0, 255, 44, 255);
				} else { 
					backImg.color = new Color32 (255, 0, 0, 255);
				}
			} else if (difficulty == 2) {
				//change slider color to indicate when to press shift again
				if (value <= 70f && value > 40f) {
					backImg.color = new Color32 (0, 255, 44, 255);
				} else { 
					backImg.color = new Color32 (255, 0, 0, 255);
				}
			} else if (difficulty == 1) {
				//change slider color to indicate when to press shift again
				if (value <= 80f && value > 20f) {
					backImg.color = new Color32 (0, 255, 44, 255);
				} else { 
					backImg.color = new Color32 (255, 0, 0, 255);
				}
			}

			if (value <= 0f || touch == false || hacking == false) {
				value = 100f;
				hackSlider.value = value;
				hacking = false;
				sliderObj.SetActive (false);
				yield break;
			}
		}
	}

	void FinishAI(GameObject AI) {
		numOhacks = AI.GetComponent<WallTerminal> ().numOhacks;
		maxHacks = AI.GetComponent<WallTerminal> ().maxHacks;

		if (value <= (goal + 20f) && value >= goal) {
			numOhacks++;
			if (numOhacks >= maxHacks) {
				FloatingTextController.CreateFloatingText ("AI Repaired", transform);
				hacking = false;
				value = 100f;
				hackSlider.value = value;
				AI.GetComponent<WallTerminal> ().AIrepaired ();
			} else if (numOhacks < maxHacks) {
				FloatingTextController.CreateFloatingText ("Firewall " + numOhacks.ToString() + " out of " + maxHacks.ToString() + " down..", transform);
				AI.GetComponent<WallTerminal> ().numOhacks++;
				hacking = false;
				value = 100f;
				hackSlider.value = value;
			} 
		} else if (value > (goal + 20f) || value < goal) {
			FloatingTextController.CreateFloatingText ("Hacking failed, Alert +" + alertRaise.ToString(), transform);

			// increase alert after failed hack
			GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
			gsm.AdjustSecurity (alertRaise);

			value = 100f;
			hackSlider.value = value;
			hacking = false;
		}
		sliderObj.SetActive (false);
	}

    IEnumerator removeDoor(GameObject door)
    {
        while (true)
        {
            yield return new WaitForSeconds(2);
            door.SetActive(false);
        }
    }
   

	void finishHack (GameObject Terminal) {
		difficulty = Terminal.GetComponent<WallTerminal> ().difficulty;
		door = Terminal.GetComponent<WallTerminal> ().door;
		door2 = Terminal.GetComponent<WallTerminal> ().door2;
		drone = Terminal.GetComponent<WallTerminal> ().droneDispensor;

		if (difficulty == 3) { //level 3
			if (value <= 45f && value > 30f) {
				if (door != null && door.activeInHierarchy) {
					GameObject spawnFx = Instantiate (hackFx, door.transform) as GameObject;
					spawnFx.transform.SetParent(null);
                    door.GetComponent<Animator>().SetBool("DoorOpen", true);
                    StartCoroutine(removeDoor(door));
                    if (door2 != null) {
                        door2.GetComponent<Animator>().SetBool("DoorOpen", true);
                        StartCoroutine(removeDoor(door2));
                    };
				}
				if (drone != null) {
                    psm.BotGeneratorShutDown = true;
                    GameObject spawnFx = Instantiate (hackFx, drone.transform) as GameObject;
					spawnFx.transform.SetParent(null);
					drone.SetActive (false);
				}

				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking successful, Alert -" + alertRaise.ToString(), transform);

				// subtract alert for successful hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (-alertRaise);

				hacking = false;
				value = 100f;
				hackSlider.value = value;
			} else {
				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking failed, Alert +" + alertRaise.ToString(), transform);

				// increase alert after failed hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (alertRaise);

				value = 100f;
				hackSlider.value = value;
				hacking = false;
			}
		} else if (difficulty == 2) { //level 2
			if (value <= 70f && value > 40f) {
				if (door != null && door.activeInHierarchy) {
					GameObject spawnFx = Instantiate (hackFx, door.transform) as GameObject;
					spawnFx.transform.SetParent(null);
                    door.GetComponent<Animator>().SetBool("DoorOpen", true);
                    StartCoroutine(removeDoor(door));
                    if (door2 != null)
                    {
                        door2.GetComponent<Animator>().SetBool("DoorOpen", true);
                        StartCoroutine(removeDoor(door2));
                    };
                }
				if (drone != null) {
                    psm.BotGeneratorShutDown = true;
                    GameObject spawnFx = Instantiate (hackFx, drone.transform) as GameObject;
					spawnFx.transform.SetParent(null);
					drone.SetActive (false);
				}

				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking successful, Alert -" + alertRaise.ToString(), transform);

				// subtract alert for successful hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (-alertRaise);

				hacking = false;
				value = 100f;
				hackSlider.value = value;
			} else {
				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking failed, Alert +" + alertRaise.ToString(), transform);

				// increase alert after failed hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (alertRaise);

				value = 100f;
				hackSlider.value = value;
				hacking = false;
			}
		} else if (difficulty == 1) { //level 1
			if (value <= 80f && value > 20f) {
				if (door != null && door.activeInHierarchy) {
					GameObject spawnFx = Instantiate (hackFx, door.transform) as GameObject;
					spawnFx.transform.SetParent(null);
                    door.GetComponent<Animator>().SetBool("DoorOpen", true);
                    StartCoroutine(removeDoor(door));
                    if (door2 != null)
                    {
                        door2.GetComponent<Animator>().SetBool("DoorOpen", true);
                        StartCoroutine(removeDoor(door2));
                    };
                }
				if (drone != null) {
                    psm.BotGeneratorShutDown = true;
                    GameObject spawnFx = Instantiate (hackFx, drone.transform) as GameObject;
					spawnFx.transform.SetParent(null);
					drone.SetActive (false);
				}

				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking successful, Alert -" + alertRaise.ToString(), transform);

				// subtract alert for successful hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (-alertRaise);

				hacking = false;
				value = 100f;
				hackSlider.value = value;
			} else {
				//adjust alert state
				FloatingTextController.CreateFloatingText ("Hacking failed, Alert +" + alertRaise.ToString(), transform);

				// increase alert after failed hack
				GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
				gsm.AdjustSecurity (alertRaise);

				value = 100f;
				hackSlider.value = value;
				hacking = false;
			}
		} 

		sliderObj.SetActive (false);
	}
}
