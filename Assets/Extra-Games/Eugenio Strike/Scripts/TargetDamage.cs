using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TargetDamage : MonoBehaviour {
	
	public int hitPoints = 2;
 	public float damageImpactSpeed;
	//public Sprite damagedSprite;
	public GameObject explosion;

	private Text scoreText;
	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;
	private GameScore gameScore;
	
	void Awake(){
		gameScore = GameScore.getInstance;
		scoreText = GameObject.Find ("Score").GetComponent<Text>();
		scoreText.text = gameScore.UpdateScoreText();
	}
	
	void Start () {
		spriteRenderer = GetComponent <SpriteRenderer>();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	}

	void OnCollisionEnter2D(Collision2D collision) {

		if(collision.collider.tag != "Damager") return;
		if(collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) return;

		//spriteRenderer.sprite = damagedSprite;
		currentHitPoints--;

		if(currentHitPoints <= 0) Kill();
	}
	
	void Kill() {
		Instantiate(explosion, transform.position, transform.rotation);
		spriteRenderer.enabled = false;
		collider2D.enabled = false;
		Destroy(gameObject);
		rigidbody2D.isKinematic = true;

		gameScore.increaseScore();
		UpdateScoreText();
	}

	void UpdateScoreText() {
		scoreText.text = gameScore.UpdateScoreText();
	}
}