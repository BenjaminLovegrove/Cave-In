using UnityEngine;
using System.Collections;

public class LightSpline : MonoBehaviour {

	public GameObject[] lamps;
	public GameObject spark;
	public AudioClip lampExplode;
	public GameObject explodeParticle;
	public bool triggered = false;

	int currentLamp = 0;
	float lampDistance;
	float sparkSpeed;
	float speedModifier;
	LineRenderer wire;
	bool firstBlown = false;

	
	void Start () {
		wire = GetComponent<LineRenderer> ();
		wire.SetVertexCount (lamps.Length);
	}
	

	void Update () {

		//Set line renderer positions
		for (int i = 0; i < lamps.Length; i++){
			wire.SetPosition(i, new Vector3 (lamps[i].transform.position.x, lamps[i].transform.position.y + 0.5f, -0.2f));
		}

		if (currentLamp + 1 == lamps.Length) {
			this.enabled = false;
		} else {
			lampDistance = Vector3.Distance (lamps [currentLamp].transform.position, lamps [currentLamp + 1].transform.position);
			speedModifier = lampDistance;
		}

		if (triggered == true){
			if (firstBlown == false){
				BlowFirst();
			}

			//Disable script when out of lamps
			if (currentLamp + 1 == lamps.Length){
				this.enabled = false;
			} else {
				sparkSpeed += (Time.deltaTime * 2.5f) / (speedModifier * 0.4f);

				//Lerp spark to next pos
				spark.transform.position = Vector3.Lerp (new Vector3(lamps [currentLamp].transform.position.x, lamps [currentLamp].transform.position.y + 0.3f, -0.2f),new Vector3 (lamps [currentLamp + 1].transform.position.x, lamps [currentLamp + 1].transform.position.y + 0.3f, -0.2f), sparkSpeed);

				//Increment lamps
				if (sparkSpeed >= 1) {
					sparkSpeed = 0f;
					currentLamp ++;
					lamps[currentLamp].GetComponentInChildren<Light>().enabled = false;
					AudioSource.PlayClipAtPoint(lampExplode, transform.position);
					Instantiate (explodeParticle, transform.position, Quaternion.identity);
				}
			}
		}

	}

	void BlowFirst(){
		lamps[0].GetComponentInChildren<Light>().enabled = false;
		AudioSource.PlayClipAtPoint(lampExplode, transform.position);
		Instantiate (explodeParticle, transform.position, Quaternion.identity);
		firstBlown = true;
	}

	void Trigger(){
		triggered = true;
	}
}
