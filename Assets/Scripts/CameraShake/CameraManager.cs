using UnityEngine;
using System.Collections;

/**
 * Camera Manager
 * 
 * This script handles screen shake from object collisions.
 * 
 * Access to this class should be done using the Camera.main object, and calling the respective event using the "SendMessage" method.
 */
public class CameraManager : MonoBehaviour
{
	private float shakeTime;
	private float shakeIntensity;

	private Vector3 originalPos;


	void DoCollisionShake()
	{
		shakeIntensity = 0.15f;
		shakeTime = 0.3f;
	}

	void DoExplosionShake()
	{
		shakeIntensity = 0.1f;
		shakeTime = 0.3f;
	}

	void DoPlanetExplosionShake()
	{
		shakeIntensity = 0.2f;
		shakeTime = 0.8f;
	}

	void Update()
	{
		if (shakeTime > 0 && PauseMenu.paused == false)
		{
			originalPos = transform.position;
			transform.position = originalPos + Random.insideUnitSphere * shakeIntensity;
			shakeTime -= Time.deltaTime * 1f;
		}
		else
		{
			shakeTime = 0f;
		//	transform.localPosition = originalPos;
		}
	} 

}
