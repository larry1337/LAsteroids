using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AsteroidController : NetworkBehaviour {

	[SyncVar]
	public Vector3 direction;

	[SyncVar]
	float thrust = 1.0f;

	public List<int> VALUES_FOR_DIRECTION = new List<int>(){-1,0,1};
	private GameController gameController;



	void Start () {

		GameObject gameControllerObject = GameObject.FindWithTag ("GameManager");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
			
		this.gameObject.GetComponent<NetworkTransform> ().sendInterval = 0.01f; 

		if (direction == Vector3.zero) {

			var randomValueX = 0;
			var randomValueY = 0;

			while (randomValueX == 0 && randomValueY == 0) {
				randomValueX = VALUES_FOR_DIRECTION [Random.Range (0, VALUES_FOR_DIRECTION.Count)];
				randomValueY = VALUES_FOR_DIRECTION [Random.Range (0, VALUES_FOR_DIRECTION.Count)];
			}

			this.direction = new Vector3 (randomValueX, randomValueY, 0);
		}
	}
	
	void Update () {
		Util.ScreenWrap(this.transform);
		this.transform.position += direction * Time.deltaTime * thrust;
		this.transform.Rotate (1f, 1.8f, 1f);

	}


	void OnCollisionEnter(Collision collision)
	{
		
		if (collision.gameObject.tag == "Bullet" && this.gameObject.tag == "AsteroidXL" ) {

			GameObject asteroidSOne = Instantiate(Resources.Load("AsteroidS")) as GameObject;

			if (NetworkServer.active == true) {
				NetworkServer.Spawn (asteroidSOne);
			}


			var asteroidSOneScript = asteroidSOne.GetComponent<AsteroidController> ();
			asteroidSOne.transform.position = this.transform.position;
			asteroidSOneScript.direction = Quaternion.Euler (0, 0, 90) * direction;


			GameObject asteroidSTwo = Instantiate(Resources.Load("AsteroidS")) as GameObject;

			if (NetworkServer.active == true) {
				NetworkServer.Spawn(asteroidSTwo);
			}


			var asteroidSTwoScript = asteroidSTwo.GetComponent<AsteroidController> ();
			asteroidSTwo.transform.position = this.transform.position;
			asteroidSTwoScript.direction = Quaternion.Euler (0, 0, -90) * direction;

			var bullet = collision.gameObject.GetComponent<Bullet>();
			Debug.Log(bullet.spawnOriginID);
			Debug.Log(PlayerController.player.GetComponent<NetworkIdentity>().playerControllerId);
			if (bullet.spawnOriginID == PlayerController.player.GetComponent<NetworkIdentity>().playerControllerId) {
  				gameController.TargetAddScore (connectionToClient, 20);
			}
		


  				

		}

		if (collision.gameObject.tag == "Bullet" && (this.gameObject.tag == "AsteroidS" || this.gameObject.tag == "AsteroidM")) {
			
			var bullet = collision.gameObject.GetComponent<Bullet>();
			if (bullet.spawnOriginID == PlayerController.player.GetComponent<NetworkIdentity>().playerControllerId) {
				gameController.TargetAddScore (connectionToClient, 10);
			}

		}


	}
}
