using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	public float destroyTimer;
	public GameObject lightSpark;
	public bool sparkSpawn = false;

	// Use this for initialization
	void Start () {
		Playtest_LightManager.numLightsActive ++;
		Destroy (this.gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
		destroyTimer -= Time.deltaTime;
		if (destroyTimer <= 2 && !sparkSpawn)
		{
			sparkSpawn = true;
			Instantiate (lightSpark, this.transform.position, Quaternion.identity);
		}
	
	
	}

	void OnDestroy()
	{
		Playtest_LightManager.numLightsActive --; //remove from the max allowed at one time
		Playtest_LightManager.spawnNext ++; //spawn next one in light manager
	}
}
