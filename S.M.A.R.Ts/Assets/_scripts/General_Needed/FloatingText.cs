using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour {

	public Animator anim;
	private Text popupText;

	void Start() {
		popupText = anim.GetComponent<Text>();

		// store default animation reference
		AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo (0);
		Destroy (gameObject, clipInfo [0].clip.length);
	}

	public void SetText(string text) {
		anim.GetComponent<Text>().text = text;
	}
}
