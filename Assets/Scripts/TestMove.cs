using UnityEngine;
using System.Collections;

public class TestMove : MonoBehaviour {
	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;

	void FixedUpdate() {
		float UpDown = Input.GetAxis("Vertical") * speed;
		float LeftRight = Input.GetAxis("Horizontal") * speed;
		UpDown *= Time.deltaTime;
		LeftRight *= Time.deltaTime;
		transform.Translate(LeftRight, UpDown, 0);

	}
}
