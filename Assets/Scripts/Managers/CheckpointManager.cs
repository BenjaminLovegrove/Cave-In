using UnityEngine;
using System.Collections;

public class CheckpointManager : MonoBehaviour {

	private static CheckpointManager _instance;

	public static CheckpointManager instance
	{
		get
		{
			if(_instance == null)
			{
				_instance = GameObject.FindObjectOfType<CheckpointManager>();
				
				//Tell unity not to destroy this object when loading a new scene!
				DontDestroyOnLoad(_instance.gameObject);
			}
			
			return _instance;
		}
	}

	public static bool checkpointSpawn = false;
	public static Vector3 p1checkpoint;
	public static Vector3 p2checkpoint;

	void Awake(){
		if(_instance == null)
		{
			//If I am the first instance, make me the Singleton
			_instance = this;
			DontDestroyOnLoad(this);
		}
		else
		{
			//If a Singleton already exists and you find
			//another reference in scene, destroy it!
			if(this != _instance)
				Destroy(this.gameObject);
		}
	}
}
