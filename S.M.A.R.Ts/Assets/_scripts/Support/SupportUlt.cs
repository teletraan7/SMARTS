using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SupportUlt : MonoBehaviour {

	private Collider[] colliders;
	public float chargeFor;
	public float chargeTime;
	private PlayerShocked pS;
    public Image Icon;

	public GameObject resFx;

    private void Update()
    {
        Icon.fillAmount += 1.0f / chargeFor * Time.deltaTime;
    }

    void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.gameObject.transform.position, 5f);
	}

	public void Rez() {
		if (chargeTime <= Time.time)
        {

            GameObject spawnRes = Instantiate(resFx, transform) as GameObject;
            spawnRes.transform.SetParent(null);
            Icon.fillAmount = 0.0f;

            //cast sphere
            colliders = Physics.OverlapSphere(this.gameObject.transform.position, 5f);
            int i = 0;

            //chug through all object is sphere 
            while (i < colliders.Length)
            {
                //if player
                if (colliders[i].gameObject.tag == "Player")
                {
                    GameObject plyr = colliders[i].gameObject;
                   
                    pS = plyr.GetComponent<PlayerShocked>();
                    //check if they are stunned
                    if (pS.shocked == true)
                    {                        
                        FloatingTextController.CreateFloatingText("Stun removed", plyr.transform);                         
                        StartCoroutine(pS.NaturalUnshock(0f));
                        pS.resTime = 0f;

                    }
                }
                i++;
            }
            //and set new charge time
            chargeTime = Time.time + chargeFor;
            
        }
	}
}
