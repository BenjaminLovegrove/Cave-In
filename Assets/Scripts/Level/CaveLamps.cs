using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	float difficultyModifier = 0.8f;
	
	AudioSource lampExplode;
	Light lightSource;
	public GameObject lampExplodeVis;

	// Use this for initialization
	void Start () {
		Playtest_LightManager.numLightsActive ++;
		lightSource = GetComponentInChildren<Light> ();
		lampExplode = GetComponent<AudioSource> ();
	}

	public void OnSplinePointReached()
	{
		lightSource.enabled = false;
		if (!lampExplode.isPlaying)
		{
			Instantiate (lampExplodeVis, transform.position, Quaternion.identity);
			lampExplode.Play ();
			Destroy (this.gameObject, 1.5f);
		}
	}

	void OnDestroy()
	{
		Playtest_LightManager.numLightsActive --; //remove from the max allowed at one time
		Playtest_LightManager.spawnNext ++; //spawn next one in light manager
	}
}
