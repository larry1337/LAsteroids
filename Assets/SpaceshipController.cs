using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class SpaceshipController : MonoBehaviour {

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



//	if (KEY_STATUS.up) {
//		var rad = ((this.rot-90) * Math.PI)/180;
//		this.acc.x = 0.5 * Math.cos(rad);
//		this.acc.y = 0.5 * Math.sin(rad);
//		this.children.exhaust.visible = Math.random() > 0.1;
//	} else {
//		this.acc.x = 0;
//		this.acc.y = 0;
//		this.children.exhaust.visible = false;
//	}
//
//	this.vel.x += this.acc.x * delta;
//	this.vel.y += this.acc.y * delta;
//	this.x += this.vel.x * delta;
//	this.y += this.vel.y * delta;
//	this.rot += this.vel.rot * delta;


	void Start(){
		this.degree = 0;
		initialRot = this.transform.localRotation;

		GameObject gameControllerObject = GameObject.FindWithTag ("GameManager");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
		invulnEnd = DateTime.Now.AddSeconds (3);

		capsuleCollider = this.gameObject.GetComponent<CapsuleCollider> ();
		capsuleCollider.enabled = false;
	}


	void Update()
	{
//		if (!isLocalPlayer)
//		{
//			return;
//		}

		if (invulnEnd < DateTime.Now)
			capsuleCollider.enabled = true;

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
			Fire();
		}

	} 

//	public override void OnStartLocalPlayer()
//	{
//		//GetComponent<MeshRenderer>().transform.rotation = Quaternion.Euler(new Vector3(-90,0,0));
//		//GetComponent<MeshRenderer>().material.color = Color.blue;
//	}

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
		

	void Fire()
	{
		var bullet = (GameObject)Instantiate (
			bulletPrefab,
			bulletSpawn.position,
			bulletSpawn.rotation);

		// Add velocity to the bullet
		bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * 6;

		// Destroy the bullet after 10 seconds
		Destroy(bullet, 2.0f);
	}

	void OnTriggerEnter(Collider other) {

		if (other.tag == "AsteroidXL" || other.tag == "AsteroidM" || other.tag == "AsteroidS")
		{


			Destroy(this.gameObject);
			gameController.SpawnPlayer = true;

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


}
