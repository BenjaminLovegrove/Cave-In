using UnityEngine;
using System.Collections;

public class Playtest_LightManager : MonoBehaviour {

	public static int numLightsActive = 2; //players starts it at 2
	private int maxLights = 8; // 2 player + 6 level lights at a time
	public static int spawnNext = 0;

	public GameObject 
		caveLight01,
		caveLight02,
		caveLight03,
		caveLight04,
		caveLight05,
		caveLight06,
		caveLight07,
		caveLight08,
		caveLight09,
		caveLight10,
		caveLight11,
		caveLight12,
		caveLight13,
		caveLight14,
		caveLight15,
		caveLight16,
		caveLight17,
		caveLight18,
		caveLight19;


	// Use this for initialization
	void Start () {
		//Always load the first six lights
		caveLight01.SetActive (true);
		caveLight02.SetActive (true);
		caveLight03.SetActive (true);
		caveLight04.SetActive (true);
		caveLight05.SetActive (true);
		caveLight06.SetActive (true);
	}
	
	// Update is called once per frame
void LateUpdate () {
		//Debug.Log("LightsActive: " +numLightsActive);

		//spawn them as one is destroyed
		if (spawnNext == 1 && maxLights < 9){caveLight07.SetActive (true);}
		if (spawnNext == 2 && maxLights < 9){caveLight08.SetActive (true);}
		if (spawnNext == 3 && maxLights < 9){caveLight09.SetActive (true);}
		if (spawnNext == 4 && maxLights < 9){caveLight10.SetActive (true);}
		if (spawnNext == 5 && maxLights < 9){caveLight11.SetActive (true);}
		if (spawnNext == 6 && maxLights < 9){caveLight12.SetActive (true);}
		if (spawnNext == 7 && maxLights < 9){caveLight13.SetActive (true);}
		if (spawnNext == 8 && maxLights < 9){caveLight14.SetActive (true);}
		if (spawnNext == 9 && maxLights < 9){caveLight15.SetActive (true);}
		if (spawnNext == 10 && maxLights < 9){caveLight16.SetActive (true);}
		if (spawnNext == 11 && maxLights < 9){caveLight17.SetActive (true);}
		if (spawnNext == 12 && maxLights < 9){caveLight18.SetActive (true);}
		if (spawnNext == 13 && maxLights < 9){caveLight19.SetActive (true);}
	
	



	

		
	}
}
