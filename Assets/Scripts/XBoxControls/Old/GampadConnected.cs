using UnityEngine;
using System.Collections;

public class GampadConnected : MonoBehaviour {

	public static int connectedControllers;
	public int devControllers = 0; //use this if you only have one controller to use, type 1
	private int maxControllers = 2;
	//public Player playerScript;


	// Use this for initialization
	void Start () {
		InvokeRepeating("CheckControllerConnections", 0, 3.0F); //run the check repeatedly every xseconds
	}


	//check that two controllers are plugged in
	void CheckControllerConnections()
	{
		connectedControllers = Input.GetJoystickNames().Length;
		if (connectedControllers == maxControllers && devControllers == 0)
		{
			Debug.Log ("Detected "+ connectedControllers +" Controller/s");
			Player.keyboardActive = false;
		}
		else if (connectedControllers < maxControllers && devControllers == 0)
		{
			Debug.LogWarning ("Not Enough Controllers Found for Maximum Players(" +maxControllers+ ")");
			Player.keyboardActive = true;
		}



		if (devControllers > 0)
		{
			Debug.LogWarning ("Using Developer Mode, "+(devControllers) +" Controller/s Added. Total Controllers = " + (connectedControllers));
			
		}
		

	}



	void OnGUI()
	{
		//announce that a controller was plugged in or unplugged
	}
}
