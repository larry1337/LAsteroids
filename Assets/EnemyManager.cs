using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemyManager : MonoBehaviour {

	public Transform[] spawnPoints = {sP1, sP2, sP3, sP4, sP5, sP6, sP7, sP8};
	public static Transform sP1;
	public static Transform sP2;
	public static Transform sP3;
	public static Transform sP4;
	public static Transform sP5;
	public static Transform sP6;
	public static Transform sP7;
	public static Transform sP8;
	public string[] GameObjectTypes = {"AsteroidXL", "AsteroidM", "AsteroidS"};
	public int spawnInterval = 4000;    
	public float launchedTime = 4.0f; // How long between each spawn.
	private DateTime nextSpawn;  

	void Start () {
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.
		//InvokeRepeating ("Spawn", 2.0f, launchedTime);
		this.nextSpawn = DateTime.Now;

	}

	void Update(){
		if(nextSpawn < DateTime.Now){
			Spawn ();
			if (spawnInterval > 500) {
				spawnInterval -= 50;
			}
			nextSpawn = DateTime.Now.AddMilliseconds (spawnInterval);
		}
	}

	void Spawn() {

		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = UnityEngine.Random.Range (0, spawnPoints.Length);
		int objectTypeIndex = UnityEngine.Random.Range (0, GameObjectTypes.Length);

		// Create an instance of the enemy prefab at the randomly selected spawn point's position and rotation.


//		var enemy = (GameObject)Instantiate(Resources.Load(GameObjectTypes[objectTypeIndex]), spawnPoints[spawnPointIndex].transform);
//		NetworkServer.Spawn(enemy);
		Instantiate(Resources.Load(GameObjectTypes[objectTypeIndex]), spawnPoints[spawnPointIndex].transform);
	}
}
