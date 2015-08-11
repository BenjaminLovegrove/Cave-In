using UnityEngine;
using System.Collections;

public class VisibilityCheck : MonoBehaviour {

	// Update is called once per frame
	void LateUpdate () {

		#region check if a player is out of camera view
		if (Intro.introTimer <= 0 && renderer.isVisible == false)
			{
				if (renderer.gameObject.name == "Hickory_sprite")
				{
					print (renderer.gameObject.name + "Out of View 01");
				}
				if (renderer.gameObject.name == "Sprite")
				{
					print (renderer.gameObject.name + "Out of View 02");
				}
			}
		#endregion
	}
}
