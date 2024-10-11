using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReinforceSpread : MonoBehaviour {

	public Collider[] colliders;
	public List<GameObject> wallList;
	private int i;
	public GameSessionManager gameManager;
	private int alert;
	private Vector3 startArea;
	private Vector3 OrgArea;
	private Vector3 StopArea;
	private Vector3 addBy;

	public float xpos;

	public float ypos;

	public float zpos;

	private Transform trns;

	void Start() {
        //where the area will stop expanding
		StopArea = new Vector3 (100f, 4f, 100f);
		trns = this.gameObject.GetComponent<Transform>();
		trns.localScale = new Vector3 (xpos, ypos, zpos);
		startArea = trns.localScale;
		StartCoroutine (Reinforce ());
		OrgArea = startArea;
		gameManager = GameObject.FindWithTag("GameController").GetComponent<GameSessionManager>();
	}

	void Update() {
		alert = gameManager.currentSecurity;
	} 

	IEnumerator Reinforce () {
		while (OrgArea.z < StopArea.z && OrgArea.x < StopArea.x) {

			yield return new WaitForSeconds(7f);
			if (alert < 33f) {
				colliders = Physics.OverlapBox (this.gameObject.transform.position, OrgArea);
				sortObjects ();
			} else if (alert >= 33f && alert < 66f) {
				OrgArea = Vector3.Lerp(OrgArea, StopArea, .1f);
				colliders = Physics.OverlapBox (this.gameObject.transform.position, OrgArea);
				sortObjects ();
			} else if (alert >= 66f) {
				OrgArea = Vector3.Lerp(OrgArea, StopArea,.5f);
				colliders = Physics.OverlapBox (this.gameObject.transform.position, OrgArea);
				sortObjects ();
			} 
		} 
		Debug.Log("Growing Done");
		yield break;
	}

	void sortObjects () {
		i = 0;
		while (i < colliders.Length) {
			foreach (Collider collider in colliders) {
				if (collider.gameObject.tag == "wallContainer") {
					GameObject wall = collider.gameObject;
					if (!wallList.Contains (wall)) {
						wallList.Add (wall);
						if (!wallList.Contains (wall)) {
						} else {
							changeWalls ();
						}
					}
				}
				i++;
			}
		}
	}

	void changeWalls() {
		foreach (GameObject wall in wallList) {
			GameObject standard = wall.GetComponent<wallIdentifier> ().standard;
			wallStatus wallStat = standard.GetComponent<wallStatus> ();
			if (wallStat.standardActive && wallStat.wallDestroyed != true) {
				wallStat.standardActive = false;
			}
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireCube (this.transform.position, OrgArea * 2);
	}
}
