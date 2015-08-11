using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float lifetime = 30f;
	float startLifetime;
	public bool lampScript = false;
	Light pointLight;
	float lightStartIntensity;
	float fireVolMax;
	AudioSource fireAudio;

	void Start(){
		fireAudio = GetComponent<AudioSource> ();
		startLifetime = lifetime;
		pointLight = GetComponentInChildren<Light> ();
		lightStartIntensity = pointLight.intensity;
		fireVolMax = fireAudio.volume;
	}

	void Update () {
		lifetime -= Time.deltaTime;
		if (lampScript == false) {
			transform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, lifetime / 30);
			pointLight.intensity = Mathf.Lerp (0, lightStartIntensity, lifetime / 30);
			fireAudio.volume = Mathf.Lerp (0, fireVolMax, lifetime / 30);
		} else {
			transform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, lifetime / startLifetime);
			pointLight.intensity = Mathf.Lerp (0, lightStartIntensity, lifetime / startLifetime);
		}

		if (lifetime <= 0){
			Destroy(this.gameObject);
		}
	}

	void SetLifetime(float lt){
		if (lifetime < lt){
			lifetime = lt;
		}
	}
	
}
