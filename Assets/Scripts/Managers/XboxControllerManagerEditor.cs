using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(XboxControlManager))]
public class XboxControllerManagerEditor : Editor {

	//XboxControlManager xBoxCont  = (XboxControlManager) target;

	
	public override void OnInspectorGUI () {

		XboxControlManager selected;

		EditorGUILayout.HelpBox("test", MessageType.Warning);

		//xBoxCont.d
		//micCon.micControl = (Lulz_MicInputEdit.micActivation)EditorGUILayout.EnumPopup(new GUIContent("Mic Control Type", "Changes the type of method for using the microphone"), micCon.micControl);
		selected.currDebug = (XboxControlManager.Debug_Types)EditorGUILayout.EnumPopup(new GUIContent("Debug Type", "Changes what will print in the console"),selected.currDebug);

	}

}
