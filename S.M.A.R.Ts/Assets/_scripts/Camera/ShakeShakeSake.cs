using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeShakeSake : MonoBehaviour {

	public float ampGain = 1.0f;
	public float freqGain = 1.0f;
	public float duration = 1.0f;

	// Use this for initialization
	void Start () {
		CameraShake.ShakeCamera (ampGain, freqGain, duration);
	}
}
