using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EugenioController : MonoBehaviour {

	public Transform ground;
	public Transform[] bonus;	//Start[0] QuestMark[1] Up[2] Down[3]
	public GameObject finalScoreText, restartButton, exitButton;

	public Text score;

	//Bricks
	public Transform[] bricks;

	//ShowingUp Gifts
	bool activated, starIsGiven, questMarkIsGiven, upIsGiven, downIsGiven, aimIsGiven;
	int[] giftsPosition, arrayBonus;	//Mirror for gifts
	float lastBrickPosition_x, lastBrickPosition_y;

	//Velocity
	Vector3 lastPosition;
	float speed;

	ControlClass controlClass;
	float pos_y;
	bool shooted; 

	void Start () {
		controlClass = ControlClass.getInstance;

		pos_y = ground.position.y + renderer.bounds.extents.y;

		GenerateRandomBonusPosition();

		SetBoolFalse();

		UpdateScoreText();

	}
	
	void Update () {

		GameStarted();
	
		LookingForScoreUpdate();

		if(activated) GetNullBrick();

	}

	void FixedUpdate() {

		VelocityManagement();

	}

	void VelocityManagement() {
		if(controlClass.GetActiveGame()) {
			KeepingVelocity();
		}
	}

	void SetBoolFalse() {
		shooted = false;
		activated = false;
		starIsGiven = false;
		questMarkIsGiven = false;
		upIsGiven = false;
		downIsGiven = false;
		aimIsGiven = false;
	}

	void KeepingVelocity() {
		float deltaS = (transform.position - lastPosition).magnitude;
		float vel_m = deltaS / Time.deltaTime;
//		Debug.Log("Velocidade: "+ vel_m);
		lastPosition = transform.position;

		if(vel_m != controlClass.GetSpeed()) {
			rigidbody2D.velocity = rigidbody2D.velocity.normalized * controlClass.GetSpeed();
		}
	}

	public void LookingForScoreUpdate() {
		if(controlClass.UpdatedScoreGotChanged()) {
			UpdateScoreText();
			controlClass.ChangeUpdatedScore(); //Set it as false
		}
	}

	void GameStarted() {
		if(controlClass.GetActiveGame() && !shooted) {
			transform.position = new Vector3(ground.position.x, pos_y, 0.0f);
			if(Input.GetKey(KeyCode.Space)) {
				rigidbody2D.AddForce(new Vector2(300.0f, 300.0f));
				shooted = true;
				return;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Brick") {
			LastBrickPosition(collision);
			if(controlClass.GetQtdBlocks() > 1) {	
				Destroy(collision.gameObject);

				//Score control
				controlClass.Dec_bricks();
				controlClass.AddScore(1);
				controlClass.ChangeUpdatedScore();	//Set it as true
				//UpdateScoreText();
				Update();

				//Empty block
				activated = true;
			} else {
				Destroy(collision.gameObject);
				Destroy(gameObject);
					
				finalScoreText.SetActive(true);
				UpdateScreenScore();

				controlClass.ChangeActiveGame(); //Not activated
				Update();

				restartButton.SetActive(true);
				exitButton.SetActive(true);
			}
		}
	}

	void LastBrickPosition(Collision2D collision) {
		lastBrickPosition_x = collision.gameObject.transform.position.x;
		lastBrickPosition_y = collision.gameObject.transform.position.y;
	}

	void GetNullBrick() {
		for(int a = 0; a < bricks.Length; a++) {
			if(bricks[a] == null) {
				if(arrayBonus[a] == 0) {
//					Debug.Log("Removido: "+ a);
					arrayBonus[a] = 1;
//					PrintArray();
					activated = false;
				} else if(arrayBonus[a] == 2 && !starIsGiven) {
					InstantiateBonus(a, 0);
					starIsGiven = true;
				} else if(arrayBonus[a] == 3 && !questMarkIsGiven) {
					InstantiateBonus(a, 1);
					questMarkIsGiven = true;
				} else if(arrayBonus[a] == 4 && !upIsGiven) {
					InstantiateBonus(a, 2);
					upIsGiven = true;
				} else if(arrayBonus[a] == 5 && !downIsGiven) {
					InstantiateBonus(a, 3);
					downIsGiven = true;
				} else if(arrayBonus[a] == 6 && !aimIsGiven) {
					InstantiateBonus(a, 4);
					aimIsGiven = true;
				}
			}
		}
	}

	void InstantiateBonus(int a, int secretCode) {
		Vector3 pos = new Vector3(lastBrickPosition_x, lastBrickPosition_y, 0.0f);
		Instantiate(bonus[secretCode], pos, Quaternion.identity);
		arrayBonus[a] = 1;
		activated = false;
	}

	void GenerateRandomBonusPosition() {

		arrayBonus = new int[bricks.Length];
		giftsPosition = new int[bonus.Length];

		for(int b = 0; b < bonus.Length; b++)
			giftsPosition[b] = GetRandomNumber(bricks.Length);
	
		CalculateProbability();

		Debug.Log(giftsPosition[0] + " " + giftsPosition[1] + " " 
		          + giftsPosition[2] + " " + giftsPosition[3] + " "
		          + giftsPosition[4]);
	}

	void CalculateProbability() {
		//		If there is two or more gifts chosen randomly
		//		then its visibility will be 1 instead of 2, 3, 4, 5 or 6.
		int aux = 0;

		for(int c = 0; c < giftsPosition.Length; c++) {
			if(giftsPosition[c] >= 0) {
				for(int d = c+1; d < giftsPosition.Length; d++) {
				   if(giftsPosition[c] == giftsPosition[d]) {
						giftsPosition[d] = -1;
						arrayBonus[giftsPosition[c]] = 0;
						aux++;
					}
				}
				if(aux >= 1)
					giftsPosition[c] = -1;
				else
					arrayBonus[giftsPosition[c]] = c + 2;
				
				aux = 0;
			}
		}
	}

	int GetRandomNumber(int max) {
		return Random.Range(0, max);
	}

//	void PrintArray() {
//		Debug.Log(ordering[0] + " " + ordering[1] + " " + ordering[2] + " " +
//		          ordering[3] + " " + ordering[4] + " " + ordering[5] + " " +
//		          ordering[6] + " " + ordering[7] + "\n" + ordering[8] + " " +
//		          ordering[9] + " " + ordering[10] + " " + ordering[11] + " " +
//		          ordering[12] + " " + ordering[13] + " " + ordering[14] + " " +
//		          ordering[15]);
//		Debug.Log(ordering[16] + " " + ordering[17] + " " +
//		          ordering[18] + " " + ordering[19] + " " + ordering[20] + " " +
//		          ordering[21] + " " + ordering[22] + " " + ordering[23]);
//
//	}

	public void UpdateScoreText() {
		score.text = "Pontos:\n" + controlClass.GetScore();
	}

	void UpdateScreenScore() {
		finalScoreText.GetComponent<Text>().text = controlClass.GetScore().ToString() + " pontos";
	}
}
