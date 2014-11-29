using UnityEngine;
using System.Collections;

public class ControlClass {

	private static volatile ControlClass instance;
	private static Object syncRoot = new Object();

	bool activeGame, updatedScore;
	int qtd_Blocks, score;
	float maxSpeed;
//	bool[] hasCollider;

	ControlClass() {
		qtd_Blocks = 24;
		score = 0;
		maxSpeed = 9.0f;
		activeGame = false;
		updatedScore = false;
	}

	public static ControlClass getInstance {
		get {
			if(instance == null) {
				lock(syncRoot) {
					if(instance == null) {
						instance = new ControlClass();
					}
				}
			}
			return instance;
		}
	}

	public bool GetActiveGame() {
		return activeGame;
	}

	public void ChangeActiveGame() {
		activeGame = !activeGame;
	}

	public int GetQtdBlocks() {
		return qtd_Blocks;
	}

	public void Dec_bricks() {
		qtd_Blocks--;
	}

	public int GetScore() {
		return score;
	}

	public void AddScore(int score) {
		this.score += score;
	}

	public float GetSpeed() {
		return maxSpeed;
	}

	public void SetSpeed(float maxSpeed) {
		this.maxSpeed = maxSpeed;
	}

	public void ResetSpeed() {
		maxSpeed = 9.0f;
	}

	public bool UpdatedScoreGotChanged() {
		return updatedScore;
	}

	public void ChangeUpdatedScore() {
		updatedScore = !updatedScore;
	}




	//Inicie todos os BonusItem com collider
//	public void BonusControl(int qtd) {
//		hasCollider = new bool[qtd];
//
//		for (int i = 0; i < hasCollider.Length; i++)
//			hasCollider[i] = true;
//	}
//
//	//Ativar ou desativar o collider de algum BonusItem
//	public void SetColliderON_OFF(int thisBonus, bool state) {
//		hasCollider[thisBonus] = state;
//	}
//
//	//Verifique se determinado BonusItem possue collider
//	public bool ReturnBonusCollider(int thatBonus) {
//		return hasCollider[thatBonus];
//	}
//
//	//Procurar por BonusItem sem collider
//	public bool HasBonusColliderOFF() {
//		for (int i = 0; i < hasCollider.Length; i++)
//			if(hasCollider[i] == false)
//				return true;
//	}




	public void ResetAll() {
		qtd_Blocks = 24;
		score = 0;
		maxSpeed = 9.0f;
		activeGame = false;
		updatedScore = false;
	}

}
