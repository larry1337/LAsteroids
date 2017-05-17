using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Asteroid : NetworkBehaviour {

	public Vector3 direction;
	public List<int> VALUES_FOR_DIRECTION = new List<int>(){-1,0,1};
	float thrust = 1.0f;
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

		var randomValueX = 0;
		var randomValueY = 0;
	    
		while(randomValueX == 0 && randomValueY == 0){
			randomValueX = VALUES_FOR_DIRECTION[Random.Range(0, VALUES_FOR_DIRECTION.Count)];
			randomValueY = VALUES_FOR_DIRECTION[Random.Range(0, VALUES_FOR_DIRECTION.Count)];
		}

		this.direction = new Vector3 (randomValueX, randomValueY, 0);
		
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
					var asteroidSOneScript = asteroidSOne.GetComponent<Asteroid> ();
		asteroidSOne.transform.position = this.transform.position;
					asteroidSOneScript.direction = Quaternion.Euler (90, 0, 0) * direction;
					

					GameObject asteroidSTwo = Instantiate(Resources.Load("AsteroidS")) as GameObject;
					var asteroidSTwoScript = asteroidSTwo.GetComponent<Asteroid> ();
		asteroidSTwo.transform.position = this.transform.position;
					asteroidSTwoScript.direction = Quaternion.Euler (-90, 0, 0) * direction;
					
					Destroy(collision.gameObject);
		gameController.AddScore(20);
		}

	if (collision.gameObject.tag == "Bullet" && (this.gameObject.tag == "AsteroidS" || this.gameObject.tag == "AsteroidM")) {
					Destroy(collision.gameObject); 
		gameController.AddScore(10);
		}
			

	}
		
}
