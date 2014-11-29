using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Here we are going to treat things like spawn point,
 * time and more.
 */

public class GameController : MonoBehaviour {

	public Camera cam;
	public GameObject[] balls;
	public GameObject gameOverText, restartButton;
	public GameObject splashScreen, startButton;
	public float timeLeft;
	public Text timeText;
	public HatController hatController;
	
	private float a, b, c, d, e, fx;
	private float maxWidth;
	private bool playing;
	
	void Start () {
		if(cam == null) cam = Camera.main;

		playing = false;

		setMaxWidth();

		setCounter();

		updateTime();
	}

	//A way to decrement timeLeft
	/* FixedUpdate is not gonna happen every frame but 
	 it comes in a regular period of time */
	void FixedUpdate() {
		if(playing) {
			timeLeft -= Time.deltaTime;
			if(timeLeft < 0) timeLeft = 0; 
			updateTime();
		}
	}

	public void StartGame() {
		splashScreen.SetActive(false);
		startButton.SetActive(false);
		hatController.ToggleControl(true);
		StartCoroutine(Spawn());
	}

	//LOOP - Coroutine
	IEnumerator Spawn() {
		//Wait 2s before starting to spawn the balls
		yield return new WaitForSeconds(2.0f); 
		playing = true;

		while(timeLeft > 0) {	
			Vector3 spawnPosition = new Vector3(
				Random.Range(-maxWidth, maxWidth), 	//x
				transform.position.y,				//y
				0.0f);								//z
			Quaternion spawnRotation = Quaternion.identity;	//Rotation = 0

			Instantiate(balls[Random.Range(0, balls.Length)], spawnPosition, spawnRotation);

			//Wait between 1 and 2s before spawning another ball
			if(timeLeft <= a && timeLeft > b)
				yield return new WaitForSeconds(Random.Range(1.0f, 2.0f));
			else if(timeLeft <= b && timeLeft > c)
				yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));
			else if(timeLeft <= c && timeLeft > d)
				yield return new WaitForSeconds(Random.Range(0.75f, 1.0f));
			else if(timeLeft <= d && timeLeft > e)
				yield return new WaitForSeconds(Random.Range(0.5f, 0.75f));
			else if(timeLeft <= e)
				yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
		}
		//When the time is over
		yield return new WaitForSeconds(2.0f);
		gameOverText.SetActive(true);
		yield return new WaitForSeconds(2.0f);
		restartButton.SetActive(true);
		hatController.ToggleControl(false);
	}

	void setCounter() {
		fx = timeLeft/5;
		a = timeLeft;
		b = a - fx;
		c = b - fx;
		d = c - fx;
		e = d - fx;
	}
	
	void setMaxWidth() {
		Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
		float ballWidth = balls[0].renderer.bounds.extents.x;
		maxWidth = cam.ScreenToWorldPoint(upperCorner).x - ballWidth;
	}

	void updateTime() {
		timeText.text = "Time left:\n" + Mathf.RoundToInt(timeLeft);
	}
}
