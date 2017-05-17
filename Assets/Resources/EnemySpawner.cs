using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EnemySpawner : NetworkBehaviour {

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
	public List<GameObject> GameObjects;

	public GameObject enemyPrefabS;
	public GameObject enemyPrefabM;
	public GameObject enemyPrefabXL;
	public int numberOfEnemies;
	private DateTime nextSpawn;

	[SyncVar]
	public int spawnInterval = 4000;    
	public float launchedTime = 4.0f;

	void Awake () {
		GameObjects.Add (enemyPrefabS);
		GameObjects.Add (enemyPrefabM);
		GameObjects.Add (enemyPrefabXL);
		this.nextSpawn = DateTime.Now;
	}

	void Update(){
		SpawnIntervall ();
	}


	public override void OnStartServer() {
		

		for (int i=0; i < numberOfEnemies; i++)
		{
			CmdSpawn ();
		}
	}

	void CmdSpawn() {
		if (!isServer)
			return;
		
		if (isServer) {

		var spawnRotation = Quaternion.Euler (0, 0, 0);

		// Find a random index between zero and one less than the number of spawn points.
		int spawnPointIndex = UnityEngine.Random.Range (0, spawnPoints.Length);
		int objectTypeIndex = UnityEngine.Random.Range (0, GameObjects.Count);


		var enemy = (GameObject)Instantiate(GameObjects[objectTypeIndex], spawnPoints[spawnPointIndex].transform.localPosition, spawnRotation);
		NetworkServer.Spawn(enemy);
		}
	}


		
	void SpawnIntervall(){
		if(nextSpawn < DateTime.Now){
			CmdSpawn ();
			if (spawnInterval > 500) {
				spawnInterval -= 50;
			}
			nextSpawn = DateTime.Now.AddMilliseconds (spawnInterval);
		}
	}

}