using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour {

	public Transform target; // target prefab
	public float spawnTime; // time between target spawns
	private float timer;

	void Start () 
	{
		timer = 0.0f;
	}
	void FixedUpdate()
	{
		timer += Time.deltaTime;

		if (timer > spawnTime)
		{
			SpawnTarget();
			timer = 0.0f;
		}
	}

	void SpawnTarget()
	{
		// Below: x is 10 instead of 0 because spawning on 0 will cause a collision with player, the positions are then
		// set in the Target class
		Instantiate(target.gameObject, new Vector3(10,0,0), Quaternion.identity); 
	}
}
