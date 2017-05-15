using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class GameController : MonoBehaviour {

	public Text scoreText;
	public Text gameOverText;
	public Text restartText;
	public GameObject LiveOne;
	public GameObject LiveTwo;
	public GameObject LiveThree;
	public bool SpawnPlayer;
	private int score;
	public bool gameOver;
	private bool restart;



	void Start () {

		gameOver = false;
		restart = false;
		restartText.text = "";
		gameOverText.text = "";


		score = 0;
		UpdateScore ();
		AddPlayer ();


	}
	
	// Update is called once per frame
	void Update () {

		if (gameOver)
		{
			GameOver();
			if (restart)
			{
				if (Input.GetKeyDown (KeyCode.R))
				{
					SceneManager.LoadScene (0);
				}
			}
			return;
		}
			

		if(SpawnPlayer) 
		{
			AddPlayer ();
			SpawnPlayer = false;
		}


	}

	void UpdateScore ()
	{
		scoreText.text = "SCORE: " + score;
	}

	public void AddScore (int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}
		

	public void AddPlayer(){

		if (GameObject.FindGameObjectsWithTag("Spaceship").Length == 0) {
			var obj = Instantiate (Resources.Load ("Spaceship")) as GameObject;
			//var script = obj.GetComponent<SpaceshipController> ();
			obj.transform.position = new Vector3 (0, 0, 0);
		}
	}

	public void GameOver ()
	{
		gameOver = true;
		gameOverText.text = "Game Over!";
		restart = true;
		restartText.text = "press 'R' for restart";
	}


}
