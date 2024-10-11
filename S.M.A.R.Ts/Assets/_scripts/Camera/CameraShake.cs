using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour {

	// static reference
	public static CameraShake cameraShake;

	// drag cinemachine cam into here from inspector
	public CinemachineVirtualCamera vcam;

	// cinemachine attributes
	private CinemachineBasicMultiChannelPerlin noise;
	private float originalAmp;
	private float originalFreq;

	void Awake() {
		CameraShake.cameraShake = GetComponent<CameraShake> ();
	}

	void Start() {
		noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin> ();

		// save original values for perlin noise if you use something that isn't 0 and 0 for ambience
		originalAmp = noise.m_AmplitudeGain;
		originalFreq = noise.m_FrequencyGain;
	}

	// shake the camera when called
	public static void ShakeCamera(float ampGain, float freqGain, float duration) {
		cameraShake.noise.m_AmplitudeGain = ampGain;
		cameraShake.noise.m_FrequencyGain = freqGain;

		cameraShake.StartCoroutine (ResetCamera (duration));
	}

	// return camera position back to regular settings
	public static IEnumerator ResetCamera(float duration) {
		yield return new WaitForSeconds (duration);
		cameraShake.noise.m_AmplitudeGain = cameraShake.originalAmp;
		cameraShake.noise.m_FrequencyGain = cameraShake.originalFreq;
	}
}