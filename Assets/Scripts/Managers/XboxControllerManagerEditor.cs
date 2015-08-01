﻿using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(XboxControlManager))]
public class XboxControllerManagerEditor : Editor {

	//XboxControlManager xBoxCont  = (XboxControlManager) target;

	
	public override void OnInspectorGUI () {

		XboxControlManager selected =  (XboxControlManager) target;
		selected.currDebug = (XboxControlManager.Debug_Types)EditorGUILayout.EnumPopup(new GUIContent("Debug Type", "Changes what will print in the console"),selected.currDebug);

		EditorGUILayout.HelpBox("Cave_In xInput Manager by Jarred", MessageType.Warning);

	}

}
