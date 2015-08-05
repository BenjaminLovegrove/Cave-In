﻿using UnityEngine;
using System.Collections;

public class GampadConnected : MonoBehaviour {

	public static int connectedControllers;
	public int devControllers = 0; //use this if you only have one controller to use, type 1
	private int maxControllers = 2;
	//public Player playerScript;

	void Awake(){
		foreach(string joystick in Input.GetJoystickNames())
			print (joystick);
		//connectedControllers++;
	}
	// Use this for initialization
	void Start () {
		connectedControllers = Input.GetJoystickNames().Length;
		InvokeRepeating("CheckControllerConnections", 0, 1.0F); //run the check repeatedly every xseconds
	}


	//check that two controllers are plugged in
	void CheckControllerConnections()
	{
		connectedControllers = Input.GetJoystickNames().Length;
		print ("connected controllers: "+Input.GetJoystickNames().Length);
		if (connectedControllers == maxControllers && devControllers == 0)
		{
			Debug.Log ("Detected "+ connectedControllers +" Controller/s");
			PlayerV2.keyboardActive = false;
		}
		else if (connectedControllers < maxControllers && devControllers == 0)
		{
			Debug.LogWarning ("Not Enough Controllers Found for Maximum Players(" +maxControllers+ ")");
			PlayerV2.keyboardActive = true;
		}



		if (devControllers > 0)
		{
			Debug.LogWarning ("Using Developer Mode, "+(devControllers) +" Controller/s Added. Total Controllers = " + (connectedControllers));
			PlayerV2.keyboardActive = false;
		}
		

	}



	void OnGUI()
	{
		//announce that a controller was plugged in or unplugged
	}
}
