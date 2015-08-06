using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	float difficultyModifier = 0.8f;

	public float destroyTimer;
	public GameObject lightSpark;
	public bool sparkSpawn = false;
	public PlayerV2 sorek;
	AudioSource lampExplode;
	Light lightSource;
	public GameObject lampExplodeVis;

	// Use this for initialization
	void Start () {
		Playtest_LightManager.numLightsActive ++;
		sorek = GameObject.FindGameObjectWithTag ("Player").GetComponent<PlayerV2> ();
		lightSource = GetComponentInChildren<Light> ();
		lampExplode = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (sorek.canMove){
			destroyTimer -= Time.deltaTime * difficultyModifier;
		}
		
		if (destroyTimer <= 2 && !sparkSpawn)
		{
			sparkSpawn = true;
			Instantiate (lightSpark, this.transform.position, Quaternion.identity);
		}

		if (destroyTimer < 0 && lightSource != null){
			lightSource.enabled = false;
			if (!lampExplode.isPlaying){
				Instantiate (lampExplodeVis, transform.position, Quaternion.identity);
				lampExplode.Play ();
				Destroy (this.gameObject, 1.5f);
			}
		}
	
	
	}

	void OnDestroy()
	{
		Playtest_LightManager.numLightsActive --; //remove from the max allowed at one time
		Playtest_LightManager.spawnNext ++; //spawn next one in light manager
	}
}
