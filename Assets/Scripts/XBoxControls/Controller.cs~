using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

//Custom xInput by Lulz
public class Controller : MonoBehaviour {

	public static Controller xInput;

	#region Debug Types
	//This should just be controller debug, should be a script debug somewhere
	public enum Debug_Types 
	{
		NONE,
		MSTRDBUG,
		STICK,
		BUTTON,
		TRIGGERS

		//Put All at the end of the main debug types
		//MSTRDBUG_All
		//MSTRDBUG_ButtonUp
		//MSTRDBUG_ButtonDown
		//THumbStick_Left&Right
		//ThumbStick_Left
		//ThumbStick_Right
		//Button_ABXY
		//Button_Dpad
		//Button_Start&Back
		//Triggers_Bool
		//Triggers_Float

	};
	public Debug_Types currDebug;

	//Inspector Debug bool, used to toggle custom editor things
	//Triggers
	//Buttons
	//maybe just duplicate the above debugs
	#endregion

	#region Detect Controllers Connected and State
 //Use this only in the player scripts themselves to differentiate player controllers
	public  PlayerIndex playerIndex;
	public  GamePadState state;
	public  GamePadState prevState;

	public  int connectedControllers;
	public  int devControllers = 0;
	public  int maxControllers = 2;
	public  bool checkControllers = true;
	private float delayedStart = 0.0f;
	public float checkRateInSeconds = 3.0f;
	
	public  PlayerIndex pone = PlayerIndex.One;
	public  PlayerIndex ptwo = PlayerIndex.Two;
	public  PlayerIndex pthree = PlayerIndex.Three;
	public  PlayerIndex pfour = PlayerIndex.Four;
	#endregion
	


	#region xbox Controller Inputs
	public  float ThumbStickL_X = 0.0f;
	public  float ThumbStickL_Y = 0.0f;
	public  float ThumbStickR_X = 0.0f;
	public  float ThumbStickR_Y = 0.0f;

	public  bool  OnButton_L_Stick = false;
	public  bool  OnButton_R_Stick = false;
	public  bool  OnButtonUp_L_Stick = false;
	public  bool  OnButtonUp_R_Stick = false;
	public  bool  OnButtonDown_L_Stick = false;
	public  bool  OnButtonDown_R_Stick = false;

	public  bool  OnButtonA = false;
	public  bool  OnButtonB = false;
	public  bool  OnButtonX = false;
	public  bool  OnButtonY = false;
	public  bool  OnButtonUpA = false;
	public  bool  OnButtonUpB = false;
	public  bool  OnButtonUpX = false;
	public  bool  OnButtonUpY = false;
	public  bool  OnButtonDownA = false;
	public  bool  OnButtonDownB = false;
	public  bool  OnButtonDownX = false;
	public  bool  OnButtonDownY = false;

	public  bool  OnButton_DpadUp = false;
	public  bool  OnButton_DpadDown = false;
	public  bool  OnButton_DpadLeft = false;
	public  bool  OnButton_DpadRight = false;
	public  bool  OnButtonUp_DpadUp = false;
	public  bool  OnButtonUp_DpadDown = false;
	public  bool  OnButtonUp_DpadLeft = false;
	public  bool  OnButtonUp_DpadRight = false;
	public  bool  OnButtonDown_DpadUp = false;
	public  bool  OnButtonDown_DpadDown = false;
	public  bool  OnButtonDown_DpadLeft = false;
	public  bool  OnButtonDown_DpadRight = false;

	public  bool  OnButtonStart = false;
	public  bool  OnButtonUpStart = false;
	public  bool  OnButtonDownStart = false;
	public  bool  OnButtonBack = false;
	public  bool  OnButtonUpBack = false;
	public  bool  OnButtonDownBack = false;

	public  bool  shoulderL = false;
	public  bool  shoulderL_Up = false;
	public  bool  shoulderL_Down = false;
	public  bool  shoulderR = false;
	public  bool  shoulderR_Up = false;
	public  bool  shoulderR_Down = false;

	public  float triggerL = 0.0f;
	public  float triggerR = 0.0f;

	//Pad Vibration
	public float bigRumble;
	public float inspectorBRumble;
	public float smallRumble;
	public float inspectorSRumble;
	#endregion

		//---------------------------------------------------------------------------------//

	void Awake (){
		xInput = this; 
	}

	// Use this for initialization
	void Start () {
		#region Detect controllers connected
		connectedControllers = Input.GetJoystickNames().Length;
		connectedControllers = devControllers + connectedControllers;
		if (checkControllers) {InvokeRepeating("CheckControllerConnections", delayedStart, checkRateInSeconds);} //run the check repeatedly every xseconds
		#endregion
	}

	#region check that enough controllers are plugged in
	void CheckControllerConnections()
	{

		if (connectedControllers < maxControllers)
		{
			Debug.LogWarning ("Not Enough Controllers Found for Maximum Players(" +maxControllers+ ")");
			Player.keyboardActive = true;
		}

		if (devControllers > 0)
		{
			Debug.LogWarning ("Using Developer Mode, "+(devControllers) +" Controller/s Added. Total Controllers = " + (connectedControllers));

		}
		
		if (connectedControllers == maxControllers && devControllers == 0)
		{
			Debug.Log ("Detected "+ connectedControllers +" Controller/s");
			Player.keyboardActive = false;
		}
	}
	#endregion


	void LateUpdate () {

	}

	// Update is called once per frame
	void Update () {
		#region Function Calls

		#endregion

		if (!Application.isPlaying)
		{
			print ("resetting vibration");
			Controller.xInput.padVibration(Player.pone, 0,0);
			Controller.xInput.padVibration(Player.ptwo,0,0);
		}

	}

	//FixedUpdate will run with the physics engine	
	void FixedUpdate () {
		#region Debug Switch		
		switch(currDebug)
		{
		case Debug_Types.MSTRDBUG:
			MasterDebug();
			break;
		case Debug_Types.BUTTON:
			ButtonDebug();
			break;
		case Debug_Types.STICK:
			StickDebug();
			break;
		case Debug_Types.TRIGGERS:
			TriggerDebug();
			break;
		case Debug_Types.NONE:
			DebugOff();
			break;
		}
		#endregion



		#region Xbox controller inputs
		//Get last updates input states, then refresh to new
		prevState = state;
		state = GamePad.GetState(playerIndex);
		//state = GamePad.GetState ( playerIndex );

		ThumbStickL_X = state.ThumbSticks.Left.X;
		ThumbStickL_Y = state.ThumbSticks.Left.Y;
		ThumbStickR_X = state.ThumbSticks.Right.X;
		ThumbStickR_Y = state.ThumbSticks.Right.Y;

		OnButton_L_Stick = ( state.Buttons.LeftStick == ButtonState.Pressed );
		OnButton_R_Stick = ( state.Buttons.RightStick == ButtonState.Pressed );
		OnButtonUp_L_Stick = ( prevState.Buttons.LeftStick == ButtonState.Pressed && state.Buttons.LeftStick == ButtonState.Released );
		OnButtonUp_R_Stick = ( prevState.Buttons.RightStick == ButtonState.Pressed && state.Buttons.RightStick == ButtonState.Released );
		OnButtonDown_L_Stick = ( prevState.Buttons.LeftStick == ButtonState.Released && state.Buttons.LeftStick == ButtonState.Pressed );
		OnButtonDown_R_Stick = ( prevState.Buttons.LeftStick == ButtonState.Released && state.Buttons.RightStick == ButtonState.Pressed );
		
		OnButtonA = ( state.Buttons.A == ButtonState.Pressed );
		OnButtonB = ( state.Buttons.B == ButtonState.Pressed );
		OnButtonX = ( state.Buttons.X == ButtonState.Pressed );
		OnButtonY = ( state.Buttons.Y == ButtonState.Pressed );
		OnButtonUpA = ( prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released );
		OnButtonUpB = ( prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released );
		OnButtonUpX = ( prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released );
		OnButtonUpY = ( prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released );
		OnButtonDownA = ( prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed );
		OnButtonDownB = ( prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed );
		OnButtonDownX = ( prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed );
		OnButtonDownY = ( prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed );
		
		OnButton_DpadUp = ( state.DPad.Up == ButtonState.Pressed );
		OnButton_DpadDown = ( state.DPad.Down == ButtonState.Pressed );
		OnButton_DpadLeft = ( state.DPad.Left == ButtonState.Pressed );
		OnButton_DpadRight = ( state.DPad.Right == ButtonState.Pressed );
		OnButtonUp_DpadUp = ( prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up== ButtonState.Released );
		OnButtonUp_DpadDown = ( prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up== ButtonState.Released );
		OnButtonUp_DpadLeft = ( prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up== ButtonState.Released );
		OnButtonUp_DpadRight = ( prevState.DPad.Up == ButtonState.Pressed && state.DPad.Up== ButtonState.Released );
		OnButtonDown_DpadUp = ( prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed );
		OnButtonDown_DpadDown = ( prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed );
		OnButtonDown_DpadLeft = ( prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed );
		OnButtonDown_DpadRight = ( prevState.DPad.Up == ButtonState.Released && state.DPad.Up == ButtonState.Pressed );
		
		OnButtonStart = ( state.Buttons.Start == ButtonState.Pressed );
		OnButtonUpStart = ( prevState.Buttons.Start == ButtonState.Pressed && state.Buttons.Start == ButtonState.Released );
		OnButtonDownStart = ( prevState.Buttons.Start == ButtonState.Released && state.Buttons.Start == ButtonState.Pressed );
		OnButtonBack = ( state.Buttons.Back == ButtonState.Pressed );
		OnButtonUpBack = ( prevState.Buttons.Back == ButtonState.Pressed && state.Buttons.Back == ButtonState.Released );
		OnButtonDownBack = ( prevState.Buttons.Back == ButtonState.Released && state.Buttons.Back == ButtonState.Pressed );
			
		shoulderL = ( state.Buttons.LeftShoulder == ButtonState.Pressed );
		shoulderL_Up = ( prevState.Buttons.LeftShoulder == ButtonState.Pressed && state.Buttons.LeftShoulder == ButtonState.Released );
		shoulderL_Down = ( prevState.Buttons.LeftShoulder == ButtonState.Released && state.Buttons.LeftShoulder == ButtonState.Pressed );
		shoulderR = ( state.Buttons.RightShoulder == ButtonState.Pressed );
		shoulderR_Up = ( prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released );
		shoulderR_Down = ( prevState.Buttons.RightShoulder == ButtonState.Released && state.Buttons.RightShoulder == ButtonState.Pressed );
				
		triggerL = state.Triggers.Left;
		triggerR = state.Triggers.Right;
		#endregion
	}

	#region Debug Functions
	void MasterDebug()
	{
	//Format-  if () {Debug.Log ("");}
	//Print all the things
		if (ThumbStickL_X < 0){Debug.Log ("Left Thumb Stick (Left):" );}
		if (ThumbStickL_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
		if (ThumbStickL_Y < 0){Debug.Log ("Left Thumb Stick (Down)");}
		if (ThumbStickL_Y > 0){Debug.Log ("Left Thumb Stick (Up)");}
		
		if (ThumbStickR_X < 0){Debug.Log ("Right Thumb Stick (Left)");}
		if (ThumbStickR_X > 0){Debug.Log ("Right Thumb Stick (Right)");}
		if (ThumbStickR_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
		if (ThumbStickR_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
		
		if (OnButton_L_Stick) {Debug.Log ("Left Stick Held");}
		if (OnButton_R_Stick) {Debug.Log ("Right Stick Held");}
		if (OnButtonUp_L_Stick) {Debug.Log ("Left Stick Released");}
		if (OnButtonUp_R_Stick) {Debug.Log ("Right Stick Released");}
		if (OnButtonDown_L_Stick) {Debug.Log ("Left Stick Pressed");}
		if (OnButtonDown_R_Stick) {Debug.Log ("Right Stick Pressed");}
		
		if (OnButtonA) {Debug.Log ("Button A Held");}
		if (OnButtonB) {Debug.Log ("Button B Held");}
		if (OnButtonX) {Debug.Log ("Button X Held");}
		if (OnButtonY) {Debug.Log ("Button Y Held");}
		if (OnButtonUpA) {Debug.Log ("Button A Released");}
		if (OnButtonUpB) {Debug.Log ("Button B Released");}
		if (OnButtonUpX) {Debug.Log ("Button X Released");}
		if (OnButtonUpY) {Debug.Log ("Button Y Released");}
		if (OnButtonDownA) {Debug.Log ("Button A Pressed");}
		if (OnButtonDownB) {Debug.Log ("Button B Pressed");}
		if (OnButtonDownX) {Debug.Log ("Button X Pressed");}
		if (OnButtonDownY) {Debug.Log ("Button Y Pressed");}
		
		if (OnButton_DpadUp) {Debug.Log ("Dpad Up Held");}
		if (OnButton_DpadDown) {Debug.Log ("Dpad Down Held");}
		if (OnButton_DpadLeft) {Debug.Log ("Dpad Left Held");}
		if (OnButton_DpadRight) {Debug.Log ("Dpad Right Held");}
		if (OnButtonUp_DpadUp) {Debug.Log ("Dpad Up Released");}
		if (OnButtonUp_DpadDown) {Debug.Log ("Dpad Down Released");}
		if (OnButtonUp_DpadLeft) {Debug.Log ("Dpad Left Released");}
		if (OnButtonUp_DpadRight) {Debug.Log ("Dpad Right Released");}
		if (OnButtonDown_DpadUp) {Debug.Log ("Dpad Up Pressed");}
		if (OnButtonDown_DpadDown) {Debug.Log ("Dpad Down Pressed");}
		if (OnButtonDown_DpadLeft) {Debug.Log ("Dpad Left Pressed");}
		if (OnButtonDown_DpadRight) {Debug.Log ("Dpad Right Pressed");}

		if (OnButtonStart) {Debug.Log ("Start Button Held");}
		if (OnButtonUpStart) {Debug.Log ("Start Button Released");}
		if (OnButtonDownStart) {Debug.Log ("Start Button Pressed");}
		if (OnButtonBack) {Debug.Log ("Back Button Held");}
		if (OnButtonUpBack) {Debug.Log ("Back Button Released");}
		if (OnButtonDownBack) {Debug.Log ("Back Button Pressed");}

		if (shoulderL) {Debug.Log ("Shoulder Left Held");}
		if (shoulderL_Up) {Debug.Log ("Shoulder Left Released");}
		if (shoulderL_Down) {Debug.Log ("Shoulder Left Pressed");}
		if (shoulderR) {Debug.Log ("Shoulder Right Held");}
		if (shoulderR_Up) {Debug.Log ("Shoulder Right Released");}
		if (shoulderR_Down) {Debug.Log ("Shoulder Right Pressed");}

		if (triggerL > 0) {Debug.Log ("Left Trigger");}
		if (triggerR > 0) {Debug.Log ("Right Trigger");}
	}

	void StickDebug()
	{
		//print only if a stick is moved
		if (ThumbStickL_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
		if (ThumbStickL_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
		if (ThumbStickL_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
		if (ThumbStickL_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
		
		if (ThumbStickR_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
		if (ThumbStickR_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
		if (ThumbStickR_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
		if (ThumbStickR_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
	}

	void ButtonDebug()
	{
		//print if a button is pressed includes stick clicks
		if (OnButton_L_Stick) {Debug.Log ("Left Stick Held");}
		if (OnButton_R_Stick) {Debug.Log ("Right Stick Held");}
		if (OnButtonUp_L_Stick) {Debug.Log ("Left Stick Released");}
		if (OnButtonUp_R_Stick) {Debug.Log ("Right Stick Released");}
		if (OnButtonDown_L_Stick) {Debug.Log ("Left Stick Pressed");}
		if (OnButtonDown_R_Stick) {Debug.Log ("Right Stick Pressed");}

		if (OnButtonA) {Debug.Log ("Button A Held");}
		if (OnButtonB) {Debug.Log ("Button B Held");}
		if (OnButtonX) {Debug.Log ("Button X Held");}
		if (OnButtonY) {Debug.Log ("Button Y Held");}
		if (OnButtonUpA) {Debug.Log ("Button A Released");}
		if (OnButtonUpB) {Debug.Log ("Button B Released");}
		if (OnButtonUpX) {Debug.Log ("Button X Released");}
		if (OnButtonUpY) {Debug.Log ("Button Y Released");}
		if (OnButtonDownA) {Debug.Log ("Button A Pressed");}
		if (OnButtonDownB) {Debug.Log ("Button B Pressed");}
		if (OnButtonDownX) {Debug.Log ("Button X Pressed");}
		if (OnButtonDownY) {Debug.Log ("Button Y Pressed");}

		if (OnButton_DpadUp) {Debug.Log ("Dpad Up Held");}
		if (OnButton_DpadDown) {Debug.Log ("Dpad Down Held");}
		if (OnButton_DpadLeft) {Debug.Log ("Dpad Left Held");}
		if (OnButton_DpadRight) {Debug.Log ("Dpad Right Held");}
		if (OnButtonUp_DpadUp) {Debug.Log ("Dpad Up Released");}
		if (OnButtonUp_DpadDown) {Debug.Log ("Dpad Down Released");}
		if (OnButtonUp_DpadLeft) {Debug.Log ("Dpad Left Released");}
		if (OnButtonUp_DpadRight) {Debug.Log ("Dpad Right Released");}
		if (OnButtonDown_DpadUp) {Debug.Log ("Dpad Up Pressed");}
		if (OnButtonDown_DpadDown) {Debug.Log ("Dpad Down Pressed");}
		if (OnButtonDown_DpadLeft) {Debug.Log ("Dpad Left Pressed");}
		if (OnButtonDown_DpadRight) {Debug.Log ("Dpad Right Pressed");}

		if (OnButtonStart) {Debug.Log ("Start Button Held");}
		if (OnButtonUpStart) {Debug.Log ("Start Button Released");}
		if (OnButtonDownStart) {Debug.Log ("Start Button Pressed");}
		if (OnButtonBack) {Debug.Log ("Back Button Held");}
		if (OnButtonUpBack) {Debug.Log ("Back Button Released");}
		if (OnButtonDownBack) {Debug.Log ("Back Button Pressed");}
		
		if (shoulderL) {Debug.Log ("Shoulder Left Held");}
		if (shoulderL_Up) {Debug.Log ("Shoulder Left Released");}
		if (shoulderL_Down) {Debug.Log ("Shoulder Left Pressed");}
		if (shoulderR) {Debug.Log ("Shoulder Right Held");}
		if (shoulderR_Up) {Debug.Log ("Shoulder Right Released");}
		if (shoulderR_Down) {Debug.Log ("Shoulder Right Pressed");}
	}

	void TriggerDebug()
	{
		//print if the triggers are being held down
		if (triggerL > 0) {Debug.Log ("Left Trigger: "+triggerL);}
		if (triggerR > 0) {Debug.Log ("Right Trigger: "+triggerR);}
	}

	void DebugOff()
	{
		
	}
	#endregion

	#region Function to make the controller vibrate
	public void  padVibration (  PlayerIndex playerIndex , float bigRumble, float smallRumble   ){
		GamePad.SetVibration ( playerIndex, bigRumble, smallRumble );
		inspectorBRumble = bigRumble;
		inspectorSRumble = smallRumble;
	}
	public void  stopPadVibration (  PlayerIndex playerIndex   ){
		GamePad.SetVibration( playerIndex, 0, 0 );
	}
	#endregion

}
