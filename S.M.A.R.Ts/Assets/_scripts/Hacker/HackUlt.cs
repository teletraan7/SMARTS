 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackUlt : MonoBehaviour {

	public GameObject TechCan;//tech ult canvas

	private Vector3 UltScale = new Vector3 (0f, 0f, 0f);//initial scale for ult circle
	private float expandRate = 0f;//rate of expanse
	private float ultRadius = 0f;//radius of circle
	public GameObject ultOutline;//the circle
	private Image ultImg;//the circle image

	private HackUlt hackUlt;
	private SphereCollider sphereTrigger;
	private Collider[] hackables;
	public float chargeTime;
	public float chargeFor;
    public Image Icon;

    void Awake () {
		ultImg = ultOutline.GetComponent<Image> ();
		hackUlt = this.gameObject.GetComponent<HackUlt> ();
		sphereTrigger = this.gameObject.GetComponent<SphereCollider> ();
		sphereTrigger.enabled = false;
		TechCan.SetActive (false);
		chargeTime = 0f;
	}

    private void Update()
    {
        Icon.fillAmount += 1.0f / chargeFor * Time.deltaTime;
    }

    public void StartUlt() {
		if (Time.time > chargeTime) {
			TechCan.SetActive (true);
			TechCan.transform.localScale = UltScale;
			while (ultRadius <= .17f && Time.time > expandRate) {
				UltScale = UltScale + new Vector3 (.004f, .004f, .004f);
				ultRadius = ultRadius + .004f;
				expandRate = Time.time + 0.03f;
			}
			if (ultRadius <= .13f) {
				ultImg.color = new Color32 (255, 255, 255, 40);
			} else if (ultRadius > .13f && ultRadius < .17f) {
				ultImg.color = new Color32 (0, 255, 12, 255);
			} else if (ultRadius >= .17f) {
				ultImg.color = new Color32 (255, 0, 0, 255);
			}
		}
	}


	public void ultStop () {
			TechCan.SetActive (false);
			if (ultRadius <= .13f) {
				UltScale = new Vector3 (0, 0, 0);
				ultRadius = 0f;             
                return;
			} else if (ultRadius > .13f && ultRadius < .17f) {
				generateSphere (ultRadius);
				UltScale = new Vector3 (0, 0, 0);
				ultRadius = 0f;
                Icon.fillAmount = 0.0f;
        } else if (ultRadius >= .17f) {
				UltScale = new Vector3 (0, 0, 0);
				ultRadius = 0f;
                Icon.fillAmount = 0.0f;                
			}
		chargeTime = Time.time + chargeFor;
	}

	void generateSphere (float ultRadius) {

		hackables = Physics.OverlapSphere (gameObject.transform.position, 7f);

		int i = 0;

		while (i < hackables.Length) {
			if (hackables [i].tag == "turret" || hackables[i].tag == "drone") {
				if (!hackables [i].name.Contains ("Terminal")) {
					hackables [i].gameObject.GetComponentInChildren<botShock> ().DisableBot ();
					Debug.Log (hackables [i].name + i + " stun: " + hackables [i].gameObject.GetComponentInChildren<botShock> ().shocked);
				}
			}
			i++;
		}
	}
		
	void OnDrawGizmos() {
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(this.gameObject.transform.position, 7f);
	}
}
