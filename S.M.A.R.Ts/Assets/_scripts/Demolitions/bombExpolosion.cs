using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class bombExpolosion : MonoBehaviour {

    private PointsManager psm;

    public List<GameObject> walls;
	private MeshRenderer[] wallMesh;
	public List<GameObject> enemies;
	public GameObject demoPlyr;
    public List<GameObject> props;

	// explosion fx
	public float boomTime = 3.0f;
	public GameObject boom;
	private int uiTime = 0;

	// security rating
	public int alertRaise = 10;

	void Awake () {
		walls = new List<GameObject> ();
		enemies = new List<GameObject> ();
        props = new List<GameObject>();
        psm = GameObject.FindWithTag("GameController").GetComponent<PointsManager>();
    }


	void OnEnable() {
		walls.Clear ();
		enemies.Clear ();
		walls = new List<GameObject> ();
		enemies = new List<GameObject> ();
        props = new List<GameObject>();
        StartCoroutine (Boom ());
	}

	// insurance
	void OnDisable() {
		walls.Clear ();
		enemies.Clear ();
		walls = new List<GameObject> ();
		enemies = new List<GameObject> ();
        props = new List<GameObject>();
        CancelInvoke ();
	}

	public void CleanLists() {
		walls.Clear ();
		enemies.Clear ();
		walls = new List<GameObject> ();
		enemies = new List<GameObject> ();
        props = new List<GameObject>();
    }

	void OnTriggerEnter (Collider other) {
        if (other.gameObject.tag == "Wall" && !walls.Contains (other.gameObject)) { 	
			walls.Add (other.gameObject);
		} else if (other.gameObject.tag == "doorWay" && !walls.Contains (other.gameObject)) { 		
			walls.Add (other.gameObject);
		} else if (other.tag == "hackable") {
			walls.Add (other.gameObject);
		} else if (other.gameObject.tag == "drone") {
			enemies.Add (other.gameObject);
		} else if (other.gameObject.tag == "ReinforcedWall") {
			walls.Add (other.gameObject);
		} else if (other.gameObject.tag == "Props") {
            props.Add(other.gameObject);
        } else if (other.gameObject.tag == "HalfWall") {
            walls.Add(other.gameObject);
        } else if (other.gameObject.tag == "turret") {
            enemies.Add(other.gameObject);
        }
    }

	IEnumerator Boom() {
		yield return new WaitForSeconds (0.05f);
		if (this.gameObject.activeInHierarchy) {
			uiTime = 3;
			InvokeRepeating ("ShowTimer", 0f, 1f);
		}

		yield return new WaitForSeconds (boomTime);

		GameObject boomFx = Instantiate (boom, transform) as GameObject;
		boomFx.transform.SetParent (null);

        foreach (GameObject wall in walls) {
            BoxCollider bC = wall.GetComponent<BoxCollider>();
            NavMeshObstacle nMo = wall.GetComponent<NavMeshObstacle>();

       

			if (wall.gameObject.tag == "Wall" || wall.gameObject.tag == "doorWay") {
                psm.WallsDestroyed += 1;
                if (bC != null) {
					bC.isTrigger = true;
				}
				if (nMo != null) {
					nMo.enabled = false;
				}
				wallMesh = wall.GetComponentsInChildren<MeshRenderer> ();

				foreach (MeshRenderer mr in wallMesh) {
					Instantiate (boom, wall.transform);
					mr.enabled = false;
				}

				if (wall.gameObject.tag != "door" && wall.gameObject.tag != "hackable" && wall.gameObject.tag != "ReinforcedWall") {
					if (wall.GetComponent<wallStatus>() != false)
                    {
                        wall.gameObject.GetComponent<wallStatus>().wallDestroyed = true;
                    }
				}

			}

            if (wall.gameObject.tag == "HalfWall")
            {
                if (bC != null)
                {
                    bC.isTrigger = true;
                }
                if (nMo != null)
                {
                    nMo.enabled = false;
                }
                wallMesh = wall.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer mr in wallMesh)
                {
                    Instantiate(boom, wall.transform);
                    mr.enabled = false;
                }
                BuildWalls buildwalls = GameObject.Find("Repair").GetComponent<BuildWalls>();
                buildwalls.numWalls--;
            }

            if (wall.gameObject.tag == "ReinforcedWall") {
                psm.WallsDestroyed += 1;
                int hitsTaken = wall.gameObject.GetComponent<ReinforcedWalls> ().bombsHit;
				int hitsNeeded = wall.gameObject.GetComponent<ReinforcedWalls> ().bombsNeeded;

				if (hitsTaken < hitsNeeded) {
					wall.gameObject.GetComponent<ReinforcedWalls> ().bombsHit++;
				} else if (hitsTaken >= hitsNeeded) {
					//	wall.GetComponent<BoxCollider> ().isTrigger = true;
					//	wall.GetComponent<NavMeshObstacle> ().enabled = false;
					if (bC != null) {
						bC.isTrigger = true;
					}
					if (nMo != null) {
						nMo.enabled = false;
					}
					wallMesh = wall.GetComponentsInChildren<MeshRenderer> ();

					foreach (MeshRenderer mr in wallMesh) {
						Instantiate (boom, wall.transform);
						mr.enabled = false;
					}
				}
			}
		}

		foreach (GameObject enemy in enemies) {
			 if (enemy.tag == "drone" || enemy.tag == "turret") {
				botShock botshck = enemy.GetComponent<botShock>();
				if (botshck.shocked == true) {
                    psm.MachinesDestroyed += 1;
                    enemy.SetActive (false);
				}
			}
		}

        foreach (GameObject prop in props)
        {
            Destroy(prop.gameObject);
            psm.FurnitureDestroyed += 1;
        }

		walls = new List<GameObject> ();
        enemies = new List<GameObject>();
        props = new List<GameObject>();
		GameObject Obj = this.gameObject;
		CancelInvoke ();
		this.gameObject.SetActive (false);
	}

	void ShowTimer() {
		// UI feedback
		if (uiTime > 0) {
			FloatingTextController.CreateFloatingText (uiTime.ToString (), transform);
			uiTime -= 1;
		} else if (uiTime == 0) {
			FloatingTextController.CreateFloatingText ("Alert +" + alertRaise.ToString(), transform);

			// add to security alert after bomb use
			GameSessionManager gsm = GameObject.FindWithTag ("GameController").GetComponent<GameSessionManager> ();
			gsm.AdjustSecurity (alertRaise);
		}
	}
}
