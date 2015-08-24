using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Credits : MonoBehaviour {

	public float scrollSpeed = 0.6f;
	RectTransform trans;

	void Start(){
		trans = GetComponent<RectTransform> ();
	}

	void FixedUpdate()
	{
		trans.Translate (Vector3.up * scrollSpeed);
	}
}
