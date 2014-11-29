using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GroundController : MonoBehaviour {

	ControlClass controlClass;
	MyEugenioController eugenioController;

	public GameObject bonusTimeText;

	public GameObject powerGunPrefab;
	bool powerGunActivated, shooted;

	float maxWidth;
	float timeStar, timeQM, timeUp, timeDown, bulletStar;

	float[] scale, originalScaleBackup;

	bool starBonus, qmBonus, upBonus, downBonus, bulletBonus;

	void Start() {
		controlClass = ControlClass.getInstance;

		SetMaxWidth();

		timeStar = timeQM = timeUp = timeDown = 5.0f;

		starBonus = qmBonus = upBonus = downBonus = false;
	
		GetOriginalScale();

	}

	void Update () {
		if(powerGunActivated) SetFire();
	}

	void FixedUpdate() {
		if(controlClass.GetActiveGame()) FollowMouse();

		BonusTimer();
	}

	void BonusTimer() {
		if(starBonus) CountDown(2);					//2
		if(qmBonus) CountDown(3);					//3
		if(upBonus) CountDown(4);					//4
		if(downBonus) CountDown(5);					//5
		if(bulletBonus) CounterBonusTimeShooting();	//6
	}

	void CountDown(int bonusItem) {

		switch (bonusItem) {
		
		case 2:
			timeStar -= Time.deltaTime;
			Debug.Log("Tempo para o "+bonusItem);
			if(timeStar < 0) {
				timeStar = 0;
				controlClass.AddScore(20);
				starBonus = false;
			}
			break;
		case 3:
			timeQM -= Time.deltaTime;
			if(timeQM < 0) {
				timeQM = 0;
				RestoreInitialValue(3);
				qmBonus = false;
			}
			break;
		case 4:
			timeUp -= Time.deltaTime;
			if(timeUp < 0) {
				timeUp = 0;
				RestoreInitialValue(4);
				upBonus = false;
			}
			break;
		case 5:
			timeDown -= Time.deltaTime;
			if(timeDown < 0) {
				timeDown = 0;
				RestoreInitialValue(5);
				downBonus = false;
			}
			break;
		default:
				break;
		}
	}
	
	void CounterBonusTimeShooting() {

		bonusTimeText.SetActive(true);
		bulletStar -= Time.deltaTime;
		UpdateBonusTimer();

		if(bulletStar < 0) {
			bulletStar = 0;
			UpdateBonusTimer();
			starBonus = false;
			bonusTimeText.SetActive(false);
		}
	}

	void RestoreInitialValue(int bonusItem) {
		switch (bonusItem) {
		case 3:
			scale[0] = originalScaleBackup[0];
			scale[1] = originalScaleBackup[1];
			scale[2] = originalScaleBackup[2];

			transform.localScale = new Vector3(scale[0], scale[1], scale[2]);
			qmBonus = false;
			break;
		case 4:
			controlClass.ResetSpeed();
			upBonus = false;
			break;
		case 5:
			controlClass.ResetSpeed();
			downBonus = false;
			break;
		default:
			Debug.Log("BonusItem: " + bonusItem);
				break;
		}
	}
	
	void GetOriginalScale() {
		scale = new float[3];
		originalScaleBackup = new float[3];
	
		scale[0] = transform.localScale.x;
		scale[1] = transform.localScale.y;
		scale[2] = transform.localScale.z;

		originalScaleBackup[0] = transform.localScale.x;
		originalScaleBackup[1] = transform.localScale.y;
		originalScaleBackup[2] = transform.localScale.z;
	}
	
	void FollowMouse() {
		Vector3 myMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector3 newGroundPositon = new Vector3(myMousePosition.x, transform.position.y, 0.0f);
		float w_Limit = Mathf.Clamp(newGroundPositon.x, -maxWidth, maxWidth);
		
		newGroundPositon = new Vector3(w_Limit, newGroundPositon.y, newGroundPositon.z);
		rigidbody2D.MovePosition(newGroundPositon);
	}

	void OnCollisionEnter2D(Collision2D collision) {

		if(collision.gameObject.tag == "Eugenio") {

			foreach(ContactPoint2D contact in collision.contacts) {
				float centerDistances = contact.point.x - transform.position.x;
				collision.gameObject.rigidbody2D.AddForce(new Vector2(100f * centerDistances, 0));
			}

		} else if(collision.gameObject.tag == "Star") {
			Destroy(collision.gameObject);

			controlClass.AddScore(20);
			controlClass.ChangeUpdatedScore();	//Set it as true

			Debug.Log("STAR");

			starBonus = true;
		}
		else if(collision.gameObject.tag == "QMark") {
			Destroy(collision.gameObject);

			// 0 - Big size
			// 1 - Small size
			int size = Random.Range(0, 2);


			if(size == 0)
				scale[0] *= 2;
			else
				scale[0] /= 2;
			transform.localScale = new Vector3(scale[0], scale[1], scale[2]);

			Debug.Log("QMARK");

			qmBonus = true;
		}
		else if(collision.gameObject.tag == "Up") {
			Destroy(collision.gameObject);
			
			controlClass.SetSpeed(15.0f);

			Debug.Log("UP");

			upBonus = true;
		}
		else if(collision.gameObject.tag == "Down") {
			Destroy(collision.gameObject);
			
			controlClass.SetSpeed(5.0f);

			Debug.Log("DOWN");

			downBonus = true;
		}
		else if(collision.gameObject.tag == "Aim") {
			Destroy(collision.gameObject);

			Vector3 gunPos = transform.position;
			gunPos.y -= powerGunPrefab.gameObject.renderer.bounds.extents.y;
			powerGunPrefab = Instantiate(powerGunPrefab, gunPos, transform.rotation) as GameObject;

			powerGunActivated = true;
		}

	}

	void SetFire() {
		if(shooted) {


		} else {
			powerGunPrefab.transform.position = new Vector3(transform.position.x,
			                                          powerGunPrefab.transform.position.y,
			                                          0.0f);
			Debug.Log("Posicao da bola X "+powerGunPrefab.transform.position.x );

			if(Input.GetKey(KeyCode.Space)) {
				powerGunPrefab.rigidbody2D.isKinematic = false;
				powerGunPrefab.rigidbody2D.AddForce(new Vector2(0.0f, 1000.0f));
				shooted = true;
				Debug.Log("Atirou");
			}
		}
		
	}

	void SetMaxWidth() {
		Vector3 screenDimensions = new Vector3(Screen.width, Screen.height, 0.0f);
		float groundWidth = renderer.bounds.extents.x;

		maxWidth = Camera.main.ScreenToWorldPoint(screenDimensions).x - groundWidth; 
	}

	void UpdateBonusTimer() {
		bonusTimeText.GetComponent<Text>().text = "Bonus\n" + Mathf.RoundToInt(timeStar);
	}

}
