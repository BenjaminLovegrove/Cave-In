using UnityEngine;
using System.Collections;

public class GampadConnected : MonoBehaviour {

	public static int connectedControllers;
	public int devControllers = 0; //use this if you only have one controller to use, type 1
	private int maxControllers = 2;
	public bool debugControllers = false;


	// Use this for initialization
	void Start () {
		InvokeRepeating("CheckControllerConnections", 0, 1.0F); //run the check repeatedly every xseconds
	}


	//check that two controllers are plugged in
	void CheckControllerConnections()
	{
		connectedControllers = Input.GetJoystickNames().Length;
		if (connectedControllers == maxControllers && devControllers == 0)
		{
			if (debugControllers){Debug.Log ("Detected "+ connectedControllers +" Controller/s");}
			PlayerV2.keyboardActive = false;
		}
		else if (connectedControllers < maxControllers && devControllers == 0)
		{
			if (debugControllers){	Debug.LogWarning ("Not Enough Controllers Found for Maximum Players(" +maxControllers+ ")");}
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
