﻿using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public float lifetime = 20f;
	Light pointLight;
	float lightStartIntensity;

	void Start(){
		pointLight = GetComponentInChildren<Light> ();
		lightStartIntensity = pointLight.intensity;
	}

	void Update () {
		lifetime -= Time.deltaTime;
		transform.localScale = Vector3.Lerp (Vector3.zero, Vector3.one, lifetime / 20);
		pointLight.intensity = Mathf.Lerp (0, lightStartIntensity, lifetime / 20);

		if (lifetime <= 0){
			Destroy(this.gameObject);
		}
	}

	void SetLifetime(float lt){
		lifetime = lt;
	}
	
}
