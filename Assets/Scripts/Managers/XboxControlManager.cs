using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

//Custom xInput plugin by Lulz
public class XboxControlManager : MonoBehaviour {

	public static XboxControlManager xInput;

	public enum Debug_Types 
	{
		MSTRDBUG,
		STICK,
		BUTTON,
		TRIGGERS,
		NONE
	};

	public bool masterDebugActive = false; //show debug log
	public bool stickDebugActive = false; //show debug log
	public bool buttonDebugActive = false; //show debug log
	public bool triggerDebugActive = false; //show debug log

	public Debug_Types currDebug;

	//Detect Controllers connected and state
	public  PlayerIndex playerIndex;
	public  GamePadState state;
	public  GamePadState prevState;
	
	public  PlayerIndex pone = PlayerIndex.One;
	public  PlayerIndex ptwo = PlayerIndex.Two;

	//xbox controller input
	private  float ThumbStickL_X = 0.0f;
	private  float ThumbStickL_Y = 0.0f;
	private  float ThumbStickR_X = 0.0f;
	private  float ThumbStickR_Y = 0.0f;

	private  bool  OnButton_L_Stick = false;
	private  bool  OnButton_R_Stick = false;
	private  bool  OnButtonUp_L_Stick = false;
	private  bool  OnButtonUp_R_Stick = false;
	private  bool  OnButtonDown_L_Stick = false;
	private  bool  OnButtonDown_R_Stick = false;

	private  bool  OnButtonA = false;
	private  bool  OnButtonB = false;
	private  bool  OnButtonX = false;
	private  bool  OnButtonY = false;
	private  bool  OnButtonUpA = false;
	private  bool  OnButtonUpB = false;
	private  bool  OnButtonUpX = false;
	private  bool  OnButtonUpY = false;
	private  bool  OnButtonDownA = false;
	private  bool  OnButtonDownB = false;
	private  bool  OnButtonDownX = false;
	private  bool  OnButtonDownY = false;

	private  bool  OnButton_DpadUp = false;
	private  bool  OnButton_DpadDown = false;
	private  bool  OnButton_DpadLeft = false;
	private  bool  OnButton_DpadRight = false;

	private  bool  OnButtonStart = false;
	private  bool  OnButtonBack = false;
	private  bool  shoulderL = false;
	private  bool  shoulderR = false;

	private  float triggerL = 0.0f;
	private  float triggerR = 0.0f;

		//---------------------------------------------------------------------------------//

	void Awake (){
		xInput = this; 
	}

	// Use this for initialization
	void Start () 
	{
		currDebug = Debug_Types.NONE;
	}


	// Update is called once per frame
	void Update () {
		#region Debug Switch
		if (masterDebugActive){currDebug = Debug_Types.MSTRDBUG;}
		else if (!masterDebugActive) {currDebug = Debug_Types.NONE;}
		if (buttonDebugActive){currDebug = Debug_Types.BUTTON;}
		else if (!buttonDebugActive) {currDebug = Debug_Types.NONE;}
		
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
		print (currDebug);
		#endregion


		#region Function Calls

		#endregion


		#region Detect controllers connected

		#endregion


		//Get last updates input states, then refresh to new
		prevState = state;
		state = GamePad.GetState(playerIndex);
		state = GamePad.GetState ( playerIndex );

		#region Xbox controller inputs
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
		
		OnButtonStart = ( state.Buttons.Start == ButtonState.Pressed );
		OnButtonBack = ( state.Buttons.Back == ButtonState.Pressed );
	
		shoulderL = ( state.Buttons.LeftShoulder == ButtonState.Pressed );
		shoulderR = ( state.Buttons.RightShoulder == ButtonState.Pressed );
		

		
		triggerL = state.Triggers.Left;
		triggerR = state.Triggers.Right;

//		if (masterDebugActive)
//		{
//			//if () {Debug.Log ();}
//
//			if (ThumbStickL_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
//			if (ThumbStickL_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
//			if (ThumbStickL_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
//			if (ThumbStickL_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
//
//			if (ThumbStickR_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
//			if (ThumbStickR_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
//			if (ThumbStickR_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
//			if (ThumbStickR_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
//
//			if (OnButton_L_Stick) {Debug.Log ("Left Stick Held: " + OnButton_L_Stick);}
//			if (OnButton_R_Stick) {Debug.Log ("Right Stick Held: " + OnButton_R_Stick);}
//			//if () {Debug.Log ();}
//
//		}
		#endregion

	}

	void MasterDebug()
	{
		if (ThumbStickL_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
		if (ThumbStickL_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
		if (ThumbStickL_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
		if (ThumbStickL_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
		
		if (ThumbStickR_X < 0){Debug.Log ("Left Thumb Stick (Left)");}
		if (ThumbStickR_X > 0){Debug.Log ("Left Thumb Stick (Right)");}
		if (ThumbStickR_Y < 0){Debug.Log ("Right Thumb Stick (Down)");}
		if (ThumbStickR_Y > 0){Debug.Log ("Right Thumb Stick (Up)");}
		
		if (OnButton_L_Stick) {Debug.Log ("Left Stick Held: " + OnButton_L_Stick);}
		if (OnButton_R_Stick) {Debug.Log ("Right Stick Held: " + OnButton_R_Stick);}
	}

	void StickDebug()
	{
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
		if (OnButton_L_Stick) {Debug.Log ("Left Stick Held: " + OnButton_L_Stick);}
		if (OnButton_R_Stick) {Debug.Log ("Right Stick Held: " + OnButton_R_Stick);}

	}

	void TriggerDebug()
	{
		currDebug = Debug_Types.TRIGGERS;
	}

	void DebugOff()
	{
		
	}
//	static void  padVibration (  PlayerIndex playerIndex ,   float big ,   float small   ){
//		GamePad.SetVibration ( playerIndex, big, small );
//	}
//	
//	static void  stopPadVibration (  PlayerIndex playerIndex   ){
//		GamePad.SetVibration( playerIndex, 0, 0 );
//	}
}
