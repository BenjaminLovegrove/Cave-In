using UnityEngine;
using System.Collections;

public class LightSpline : MonoBehaviour {

	public GameObject[] lamps;
	public GameObject spark;
	public AudioClip lampExplode;
	public GameObject explodeParticle;

	int currentLamp = 0;
	float lampDistance;
	float sparkSpeed;
	float speedModifier;
	LineRenderer wire;

	
	void Start () {
		wire = GetComponent<LineRenderer> ();
		wire.SetVertexCount (lamps.Length);

		//Blow first lamp
		lamps[0].GetComponentInChildren<Light>().enabled = false;
		AudioSource.PlayClipAtPoint(lampExplode, transform.position);
		Instantiate (explodeParticle, transform.position, Quaternion.identity);
	}
	

	void Update () {

		//Set line renderer positions
		for (int i = 0; i < lamps.Length; i++){
			wire.SetPosition(i, new Vector3 (lamps[i].transform.position.x, lamps[i].transform.position.y + 0.5f, -0.2f));
		}

		lampDistance = Vector3.Distance(lamps[currentLamp].transform.position, lamps[currentLamp + 1].transform.position);
		speedModifier = lampDistance;

		//Disable script when out of lamps
		if (currentLamp + 1 == lamps.Length){
			this.enabled = false;
		} else {
			sparkSpeed += Time.deltaTime / (speedModifier * 0.4f);

			//Lerp spark to next pos
			wire.transform.position = Vector3.Lerp (new Vector3(lamps [currentLamp].transform.position.x, lamps [currentLamp].transform.position.y + 0.3f, -0.2f),new Vector3 (lamps [currentLamp + 1].transform.position.x, lamps [currentLamp + 1].transform.position.y + 0.3f, -0.2f), sparkSpeed);

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
