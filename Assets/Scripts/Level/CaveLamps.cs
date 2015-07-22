﻿using UnityEngine;
using System.Collections;

public class CaveLamps : MonoBehaviour {

	public float destroyTimer;

	// Use this for initialization
	void Start () {
		Playtest_LightManager.numLightsActive ++;
		Destroy (this.gameObject, destroyTimer);
	}
	
	// Update is called once per frame
	void Update () {
	
	
	}

	void OnDestroy()
	{
		Playtest_LightManager.numLightsActive --; //remove from the max allowed at one time
		Playtest_LightManager.spawnNext ++; //spawn next one in light manager
	}
}
