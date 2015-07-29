using UnityEngine;
using System.Collections;

public class GampadConnected : MonoBehaviour {

	public int connectedControllers;
	private int maxControllers = 2;
	//public Player playerScript;


	// Use this for initialization
	void Start () {
		connectedControllers = Input.GetJoystickNames().Length;
		InvokeRepeating("CheckControllerConnections", 0, 3.0F);
	}


	//check that two controllers are plugged in
	void CheckControllerConnections()
	{
		if (connectedControllers <= maxControllers)
		{
			Debug.LogError ("2 Controllers Not plugged in, Enabling Keyboard controls");
			Player.keyboardActive = true;
		}
		
		if (connectedControllers == 2)
		{
			Debug.LogWarning ("Detected 2 Controller, Disabling keboard controls");
			Player.keyboardActive = false;
		}
	}
}
