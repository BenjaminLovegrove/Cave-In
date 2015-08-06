using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Controller))]
public class ControllerEditor : Editor {

	public override void OnInspectorGUI () {

		Controller selected =  (Controller) target;

		#region Rumble Inspector Indication
		float bigRumble = selected.inspectorBRumble;
		float smallRumble = selected.inspectorSRumble;
		BigRumbleReader (bigRumble*1, "Big Rumble "+bigRumble*100+ "/100" ); 
		SmallRumbleReader (smallRumble*1, "Small Rumble "+smallRumble*100+ "/100" );
		#endregion

		#region Debug Types for Inspector
		EditorGUILayout.Space ();
		selected.currDebug = (Controller.Debug_Types)EditorGUILayout.EnumPopup(new GUIContent("Debug Type", "Changes what will print in the console"),selected.currDebug);

		EditorGUILayout.Space ();
		#endregion

		#region Check Controller Connections
		selected.checkControllers = EditorGUILayout.Toggle (new GUIContent("Controller Connection Check", "Enable Runtime Controller Connection Check"), selected.checkControllers);

		if (selected.checkControllers == true)
		{
			selected.devControllers = EditorGUILayout.IntField(new GUIContent ("Dev Controllers","Trick the check into thinking you have more controllers connected"),selected.devControllers);
			selected.maxControllers = EditorGUILayout.IntField(new GUIContent ("Maximum Players","Input the max number of players for your game"),selected.maxControllers);
			selected.checkRateInSeconds = EditorGUILayout.FloatField(new GUIContent ("Check Controller Rate(sec)","How many times per second will the game check for a controller"),selected.checkRateInSeconds);

		
		
		}
		#endregion

		EditorGUILayout.EnumPopup(new GUIContent("PlayerIndex", ""),selected.playerIndex);

		EditorGUILayout.Space ();
		EditorGUILayout.HelpBox("Cave_In xInput Manager by Jarred", MessageType.Warning);



	}

	#region Rumble Inspector Indication Functions
	void BigRumbleReader (float value, string label) {
		EditorGUILayout.Space ();
		Rect vRect = GUILayoutUtility.GetRect (18, 18, "TextField");      
		EditorGUI.ProgressBar (vRect, value, label);
	}
	void SmallRumbleReader (float value, string label){
		Rect vRect = GUILayoutUtility.GetRect (18, 18, "TextField");      
		EditorGUI.ProgressBar (vRect, value, label);
		EditorGUILayout.Space ();
	}
	#endregion
}

/* Copy of microphone editor for references
if (microphoneDeviceFound) {
	float micInputValue = micCon.howLoud;    
	VolumeReader (micInputValue * 25, "Loudness " + micInputValue *100);  
	
	//micCon.virual3D = EditorGUILayout.Toggle (new GUIContent("Enable Virtual 3D", "Uses a falloff system to simulate virtual 3D"), micCon.virual3D);
	//micCon.volumeFallOff = EditorGUILayout.FloatField (new GUIContent("Volume Falloff", "Set the rate at wich audio volume gets lowered. A lower value will have a slower falloff and thus hearable from a greater distance, while a higher value will make the audio degrate faster and dissapear from a shorter distance"), micCon.volumeFallOff);
	//micCon.GuiSelectDevice = EditorGUILayout.Toggle (new GUIContent("Gui Selection", "Select the microphone ingame using a GUI menu"), micCon.GuiSelectDevice);
	//EditorGUI.BeginChangeCheck();
	//micCon.ableToHearMic = EditorGUILayout.Toggle (new GUIContent("Audio Mute", "Select whether you can hear yourself talking or not"), micCon.ableToHearMic);
	//if (EditorGUI.EndChangeCheck()) {
	//	micCon.audio.mute = micCon.ableToHearMic;
	//}
	
	micCon.debug = EditorGUILayout.Toggle(new GUIContent("Debug",""),micCon.debug);
	micCon.Initialised = EditorGUILayout.Toggle(new GUIContent("Initialised Mic",""),micCon.Initialised);
	micCon.Mute = EditorGUILayout.Toggle(new GUIContent("Mute","Off to hear the input from your mic"),micCon.Mute);
	micCon.ShowDeviceName = EditorGUILayout.Toggle(new GUIContent("Show Device Name",""),micCon.ShowDeviceName);
	
	micCon.sensitivity = EditorGUILayout.FloatField(new GUIContent("Mic Sensitivity", "The sensitivity that the audio is recieved from the microphone"), micCon.sensitivity);
	//micCon.ramFlushSpeed = EditorGUILayout.FloatField(new GUIContent("Ram Flush Speed", "The interval time between when the microphone audioclip is reset"), micCon.ramFlushSpeed);
	micCon.sourceVolume = EditorGUILayout.FloatField(new GUIContent("Volume", "Volume of the audio that comes out of audiosouce, basically the volume of the microphone"), micCon.sourceVolume);
	micCon.micControl = (Lulz_MicInputEdit.micActivation)EditorGUILayout.EnumPopup(new GUIContent("Mic Control Type", "Changes the type of method for using the microphone"), micCon.micControl);
	micCon.SelectIngame = EditorGUILayout.Toggle(new GUIContent("Select Ingame",""),micCon.SelectIngame);
	//micCon.InputDevice = EditorGUILayout.EnumPopup (new GUIContent ("Input Device",""),micCon.InputDevice);
	micCon.amountSamples = EditorGUILayout.IntField(new GUIContent ("Sample Rate",""),micCon.amountSamples);
	micCon.sourceVolume = EditorGUILayout.FloatField(new GUIContent ("Source Volume",""),micCon.sourceVolume);
	micCon.maxFreq = EditorGUILayout.IntField(new GUIContent("Max Frequency",""),micCon.maxFreq);
	
	micCon.ThreeD = EditorGUILayout.Toggle(new GUIContent("3D Voice",""),micCon.ThreeD);
	micCon.VolumeFallOff = EditorGUILayout.FloatField (new GUIContent("Volume Falloff", ""),micCon.VolumeFallOff);
	micCon.PanThreshold = EditorGUILayout.FloatField (new GUIContent("Pan Threshold", ""),micCon.PanThreshold);
	micCon.ListenerDistance = EditorGUILayout.FloatField (new GUIContent("Listener Distance", ""),micCon.ListenerDistance);
	
	
	EditorUtility.SetDirty(target);

*/