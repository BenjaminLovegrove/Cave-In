using UnityEngine;
using System.Collections;

public class EnableOnStart : MonoBehaviour {

	public GameObject[] toEnable;

	void Start () {
	foreach (GameObject obj in toEnable) {
			obj.SetActive(true);
	}
	}
}
