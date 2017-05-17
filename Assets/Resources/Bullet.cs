using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {

	public short spawnOriginID;

	void Update()
	{

		Util.ScreenWrap (this.transform);
	}


	 void OnCollisionEnter(Collision collision)
	{
		Destroy(this.gameObject);

		if (collision.gameObject.tag == "AsteroidXL" || collision.gameObject.tag == "AsteroidM" || collision.gameObject.tag == "AsteroidS"  )
		{ 
//			if (collision.gameObject.tag== "AsteroidXL") {
//				GameObject asteroidSOne = Instantiate(Resources.Load("AsteroidS")) as GameObject;
//				GameObject asteroidSTwo = Instantiate(Resources.Load("AsteroidS")) as GameObject;
//				asteroidSOne.transform.position = new Vector3 (-3f, 0f, 0f);
//				asteroidSTwo.transform.position = new Vector3 (-2f, 0f, 0f);
//			}
			NetworkServer.Destroy(collision.gameObject);

		}



	}
}
