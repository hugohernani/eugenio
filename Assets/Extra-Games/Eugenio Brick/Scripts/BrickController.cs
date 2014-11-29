using UnityEngine;
using System.Collections;

public class BrickController : MonoBehaviour {

	bool colliderON;
	float timeLeft;

	ControlClass controlClass;

	void Start() {
		controlClass = ControlClass.getInstance;

		colliderON = true;
		timeLeft = 1.0f;
	}

	void FixedUpdate() {
		if(!colliderON) {
			timeLeft -= Time.deltaTime;
			if(timeLeft < 0) {
				timeLeft = 0;
				gameObject.collider2D.enabled = true;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if(collision.gameObject.tag == "Star") {
			gameObject.collider2D.enabled = false;
			colliderON = false;
		} else if(collision.gameObject.tag == "QMark") {
			gameObject.collider2D.enabled = false;
			colliderON = false;
		} else if(collision.gameObject.tag == "Up") {
			gameObject.collider2D.enabled = false;
			colliderON = false;
		} else if(collision.gameObject.tag == "Down") {
			gameObject.collider2D.enabled = false;
			colliderON = false;
		} else if(collision.gameObject.tag == "Aim") {
			gameObject.collider2D.enabled = false;
			colliderON = false;
		} else if(collision.gameObject.tag == "Bullet") {
			Destroy(gameObject);
			Destroy(collision.gameObject);
			controlClass.AddScore(1);
		}
	}

}
