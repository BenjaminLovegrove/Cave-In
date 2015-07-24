using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using XInputDotNetPure; // Required in C#

using UnityEngine;
using System.Collections;

public class xInput_Setup : MonoBehaviour {
	
	bool  playerIndexSet = false;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	
	// Static variables for
	public 	static float h1 = 0.0f;
	public static float v1 = 0.0f;
	
	public static float h2 = 0.0f;
	public static float v2 = 0.0f;
	
	public static bool  buttonA = false;
	public static bool  buttonB = false;
	public static bool  buttonX = false;
	public static bool  buttonY = false;
	
	public static bool  dpadUp = false;
	public static bool  dpadDown = false;
	public static bool  dpadLeft = false;
	public static bool  dpadRight = false;
	
	public static bool  buttonStart = false;
	public static bool  buttonBack = false;
	
	public static bool  shoulderL = false;
	public static bool  shoulderR = false;
	
	public static bool  stickL = false;
	public static bool  stickR = false;
	
	public static float triggerL = 0.0f;
	public static float triggerR = 0.0f;
	
	void  Update (){

		prevState = state;
		state = GamePad.GetState(playerIndex);

		// Find a PlayerIndex, for a single player game
		if ( !playerIndexSet || !prevState.IsConnected ) {
			;
			for ( int i = 0; i < 4; ++i ) {

				PlayerIndex testPlayerIndex = (PlayerIndex)i;
				Debug.Log ( "GamePad found {0}" + testPlayerIndex) ;
				switch ( i ) {
				case 0:
					testPlayerIndex = PlayerIndex.One;
					break;
				case 1:
					testPlayerIndex = PlayerIndex.Two;
					break;
				case 2:
					testPlayerIndex = PlayerIndex.Three;
					break;
				case 3:
					testPlayerIndex = PlayerIndex.Four;
					break;
				}
				
				GamePadState testState = GamePad.GetState ( testPlayerIndex );
				if ( testState.IsConnected ) {
					Debug.Log ( "GamePad found {0}" + testPlayerIndex) ;
					playerIndex = playerIndex;
					playerIndexSet = true;
				}
			}
		}
		
		state = GamePad.GetState ( playerIndex );
		
		h1 = state.ThumbSticks.Left.X;
		v1 = state.ThumbSticks.Left.Y;
		
		h2 = state.ThumbSticks.Right.X;
		v2 = state.ThumbSticks.Right.Y;
		
		buttonA = ( state.Buttons.A == ButtonState.Pressed );
		buttonB = ( state.Buttons.B == ButtonState.Pressed );
		buttonX = ( state.Buttons.X == ButtonState.Pressed );
		buttonY = ( state.Buttons.Y == ButtonState.Pressed );
		
		dpadUp = ( state.DPad.Up == ButtonState.Pressed );
		dpadDown = ( state.DPad.Down == ButtonState.Pressed );
		dpadLeft = ( state.DPad.Left == ButtonState.Pressed );
		dpadRight = ( state.DPad.Right == ButtonState.Pressed );
		
		buttonStart = ( state.Buttons.Start == ButtonState.Pressed );
		buttonBack = ( state.Buttons.Back == ButtonState.Pressed );
		
		shoulderL = ( state.Buttons.LeftShoulder == ButtonState.Pressed );
		shoulderR = ( state.Buttons.RightShoulder == ButtonState.Pressed );
		
		stickL = ( state.Buttons.LeftStick == ButtonState.Pressed );
		stickR = ( state.Buttons.RightStick == ButtonState.Pressed );
		
		triggerL = state.Triggers.Left;
		triggerR = state.Triggers.Right;
	}
	
	static void  padVibration (  PlayerIndex playerIndex ,   float big ,   float small   ){
		GamePad.SetVibration ( playerIndex, big, small );
	}
	
	static void  stopPadVibration (  PlayerIndex playerIndex   ){
		GamePad.SetVibration( playerIndex, 0, 0 );
	}
	
}