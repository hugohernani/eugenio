using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameScore {

	private static volatile GameScore instance;
	private static object syncRoot = new Object();
	
	private int score, attempts;
	private bool startButton, isPlaying;
	private string attemptsText, scoreText;

	GameScore() {
		score = 0;
		attempts = 0;
		startButton = true;
		isPlaying = false;
	}

	public static GameScore getInstance {
		get {
			if(instance == null) {
				lock(syncRoot) {
					if(instance == null) {
						instance = new GameScore();
					}
				}
			}
			return instance;
		}
	}

	public int getAttempts() {
		return attempts;
	}
	
	public void increaseAttempts() {
		attempts++;
	}

	public string UpdateAttemptsText() {
		return "Tentativas:\n"+ attempts.ToString()+"/3";
	}

	public string UpdateScoreText() {
		return "Pontos:\n"+ score.ToString();
	}

	public int getScore(){
		return score;
	}

	public void increaseScore() {
		score += 2;
	}

	public bool getStatusButton() {
		return startButton;
	}

	public void changeStatusButton() {
		startButton = !startButton;
	}

	public bool getIsPlaying() {
		return isPlaying;
	}

	public void setIsNotPlaying() {
		isPlaying = !isPlaying;
	}

	public void resetAll() {
		score = 0;
		attempts = 0;
		startButton = true;
		isPlaying = false;
	}
}
