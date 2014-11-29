using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Resetter : MonoBehaviour {

	//http://goo.gl/oejlok part 2

	public Rigidbody2D projectile;	//EugenioBall
	public Text attemptsText;
	public GameObject restartButton, finalScore, startButton, exitButton;

	private float resetSpeed = 0.015f;
	private GameScore gameScore;
	private float resetSpeedSqr;
	private SpringJoint2D spring;
	private bool isPlaying;

	void Awake() {
		gameScore = GameScore.getInstance;
	}

	void Start () {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D>();
		isPlaying = true;
	}

	void Update () {
		if(isPlaying) {
			if(Input.GetKeyDown(KeyCode.R)) {
				Reset();
			}
			if(spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr) {
				Reset();
			}
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.rigidbody2D == projectile) Reset();
	}

	void Reset() {
		if(gameScore.getAttempts() < 3) {
			gameScore.increaseAttempts();
			attemptsText.text = gameScore.UpdateAttemptsText();
			Application.LoadLevel(Application.loadedLevel);
		} else {
			isPlaying = false;
			finalScore.GetComponent<Text>().text = gameScore.getScore().ToString()+" pontos";
			finalScore.SetActive(true);
			restartButton.SetActive(true);
			exitButton.SetActive(true);
		}
	}

}
