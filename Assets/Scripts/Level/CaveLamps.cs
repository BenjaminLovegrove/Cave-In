using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	float difficultyModifier = 0.8f;

	public float destroyTimer;
	public GameObject lightSpark;
	public bool sparkSpawn = false;
	public Player sorek;

	// Use this for initialization
	void Start () {
		Playtest_LightManager.numLightsActive ++;
		sorek = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
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

		if (destroyTimer < 0){
			Destroy (this.gameObject);
		}
	
	
	}

	void OnDestroy()
	{
		Playtest_LightManager.numLightsActive --; //remove from the max allowed at one time
		Playtest_LightManager.spawnNext ++; //spawn next one in light manager
	}
}
