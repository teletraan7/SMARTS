using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DefenderUlt : MonoBehaviour {

	private Collider[] colliders;
	public float chargeFor;
	public float chargeTime;

	public GameObject aoeFx;
    public Image Icon;

    // Use this for initialization
    void Start () {
		chargeTime = 0f;
	}

	//draw the sphere for debugging
	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawWireSphere(this.gameObject.transform.position, 5f);
	}

    private void Update()
    {
        Icon.fillAmount += 1.0f / chargeFor * Time.deltaTime;
    }

    //ult start
    public void Ult () {
		//prevent spam
		if(Time.time > chargeTime) {
			// visual fx
			GameObject spawnedFx = Instantiate (aoeFx, transform) as GameObject;
			spawnedFx.transform.SetParent (null);
            Icon.fillAmount = 0f;

			//cast sphere
			colliders = Physics.OverlapSphere (this.gameObject.transform.position, 5f);
			int i = 0;

			//chug through all object is sphere 
			while (i < colliders.Length) {
				//if drone or turret then 
				if (colliders [i].gameObject.tag == "drone" || colliders [i].gameObject.tag == "turret") {
					//disable
					colliders [i].gameObject.GetComponentInChildren<botShock> ().DisableBot ();
					Debug.Log (colliders [i].name + i + " stun: " + colliders [i].gameObject.GetComponentInChildren<botShock> ().shocked);
				}
				i++;
			}
			//and set ne charge time
			chargeTime = Time.time + chargeFor;
		}
	}
}
