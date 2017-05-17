using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	float degree;
	float acclX;
	float acclY;
	float SPEED_MAX = 10f;
	float SPEED_INCR = 4.5f;
	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	Quaternion initialRot;
	private GameController gameController;
	private DateTime invulnEnd;
	private CapsuleCollider capsuleCollider;
	private NetworkStartPosition[] spawnPoints;
	private Vector3 mySpawnPoint = Vector3.zero;


	void Start(){

		if (isLocalPlayer)
		{
			spawnPoints = FindObjectsOfType<NetworkStartPosition>();
		}

		this.transform.rotation = Quaternion.Euler(new Vector3(-90,0,0));
		this.degree = 0;
		initialRot = this.transform.localRotation;

		invulnEnd = DateTime.Now.AddSeconds (3);

		capsuleCollider = this.gameObject.GetComponent<CapsuleCollider> ();
		capsuleCollider.enabled = false;


		GameObject gameControllerObject = GameObject.FindWithTag ("GameManager");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}


	}

	void Update() {
		
		if (!isLocalPlayer)
			return;

		if (invulnEnd < DateTime.Now)
			capsuleCollider.enabled = true;

		// wrap the screen
		Util.ScreenWrap(this.transform);


		if (Input.GetAxis ("Vertical") > 0) {
			var rad =  Mathf.PI * (this.degree -90)/180;

			this.acclX += Mathf.Cos (-rad) * SPEED_INCR * Time.deltaTime;
			this.acclY += Mathf.Sin (-rad) * SPEED_INCR * Time.deltaTime;

			if (acclX > SPEED_MAX) {
				acclX = SPEED_MAX;
			}

			if (acclY > SPEED_MAX) {
				acclY = SPEED_MAX;
			}

		} else {
			acclX -= acclX * 0.2f * Time.deltaTime; 
			acclY -= acclY * 0.2f * Time.deltaTime; 
		}

		SetDegree ();
		this.transform.position += new Vector3(acclX, acclY ,0) * Time.deltaTime * SPEED_INCR;

		if (Input.GetKeyDown(KeyCode.Space))
		{
			CmdFire();
		}

	}
		

	void SetDegree(){
		var horizotal = Input.GetAxis("Horizontal");
		degree += horizotal * Time.deltaTime * 200.0f;
		degree = degree % 360;
		while (degree < 0) {
			degree = degree + 360;
		}
		SpaceshipRotation(degree);
	}


	void SpaceshipRotation(float deg) {
		this.transform.localRotation = initialRot;
		transform.Rotate(0, deg, 0);
	}

	[Command]
	void CmdFire()
	{
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Spawn the bullet on the Clients
		NetworkServer.Spawn(bullet);

		// Destroy the bullet after 10 seconds
		Destroy(bullet, 2.0f);
	}

	public override void OnStartLocalPlayer()
	{
		GetComponent<MeshRenderer>().material.color = Color.yellow;
		this.mySpawnPoint = this.transform.position;
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "AsteroidXL" || other.tag == "AsteroidM" || other.tag == "AsteroidS") {

			RpcRespawn();

			Destroy (gameController.LiveThree);
			if (gameController.LiveThree == null) {
				Destroy (gameController.LiveTwo);
			}
			if (gameController.LiveTwo == null) {
				Destroy (gameController.LiveOne);
				gameController.gameOver = true;
			}

		}
	}


	void RpcRespawn()
	{
		if (isLocalPlayer)
		{
			this.degree = 0;
			this.acclX = 0.0f;
			this.acclY = 0.0f;


				// If there is a spawn point array and the array is not empty, pick a spawn point at random
			if (mySpawnPoint == Vector3.zero && spawnPoints != null && spawnPoints.Length > 0) {
					this.mySpawnPoint = spawnPoints [UnityEngine.Random.Range (0, spawnPoints.Length)].transform.position;
				}

				// Set the player’s position to the chosen spawn point
				transform.position = this.mySpawnPoint;
		}
	}
		

}