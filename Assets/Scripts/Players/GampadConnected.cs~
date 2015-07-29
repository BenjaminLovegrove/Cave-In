using UnityEngine;
using System.Collections;

public class GampadConnected : MonoBehaviour {

	public static int connectedControllers;
	public int devControllers = 0;
	private int maxControllers = 2;
	//public Player playerScript;


	// Use this for initialization
	void Start () {
		connectedControllers = Input.GetJoystickNames().Length;
		InvokeRepeating("CheckControllerConnections", 0, 3.0F); //run the check repeatedly every xseconds
	}


	//check that two controllers are plugged in
	void CheckControllerConnections()
	{
		connectedControllers += devControllers;
		if (connectedControllers < maxControllers)
		{
			Debug.LogError ("2 Controllers Not plugged in, Enabling Keyboard controls");
			Player.keyboardActive = true;
		}
		
		if (connectedControllers == maxControllers)
		{
			Debug.LogWarning ("Detected 2 Controller, Disabling keboard controls");
			Player.keyboardActive = false;
		}
	}

	void OnGUI()
	{
		//announce that a controller was plugged in or unplugged
	}
}
