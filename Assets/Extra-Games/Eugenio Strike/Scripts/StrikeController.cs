using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StrikeController : MonoBehaviour {

	//http://goo.gl/4702fV Part 1

	public LineRenderer catapultLineFront, catapultLineBack;	//The two rubber bands
	public GameObject startButton, splash, groundLimit;
	public Text attemptsText;

	private float maxStretch = 3.0f;
	private float maxStretchSqr, circleRadius, eugenioWidth;
	private GameScore gameScore;
	private SpringJoint2D spring;
	private Transform catapultBack;
	private Ray rayToMouse, frontCatapultToProjectile;
	private bool clickedOn, playing;
	private Vector2 prevVelocity;

	void Awake() {
		spring = GetComponent<SpringJoint2D>();
		catapultBack = spring.connectedBody.transform;
		gameScore = GameScore.getInstance;
	}

	void Start () {
		attemptsText.text = gameScore.UpdateAttemptsText();

		startButton.SetActive(gameScore.getStatusButton());

		splash.SetActive(gameScore.getStatusButton());

		playing = !gameScore.getStatusButton();

		LineRendererSetup();

		/* rayToMouse starts at catapult position and has no direction
		because we are gonna set this when we need it */
		rayToMouse = new Ray(catapultBack.position, Vector3.zero);	//Ray coming from catapultBack
		frontCatapultToProjectile = new Ray(catapultLineFront.transform.position, Vector3.zero);	//Ray coming from catapultFront

		maxStretchSqr = maxStretch * maxStretch;
		eugenioWidth = renderer.bounds.extents.x;
	}
	
	void Update () {
		if(clickedOn) Dragging();

		if(spring != null) { //If has the rubber band...
			//When we let it go
			if(!rigidbody2D.isKinematic && prevVelocity.sqrMagnitude > rigidbody2D.velocity.sqrMagnitude) {
				Destroy(spring);
				rigidbody2D.velocity = prevVelocity;	//Launch the object
			}
			if(!clickedOn)
				prevVelocity = rigidbody2D.velocity;
				
			LineRendererUpdate();
		} else {
			catapultLineFront.enabled = false;
			catapultLineBack.enabled = false;
		}
	}

	void OnMouseDown() {
		if(playing) {
			spring.enabled = false;
			clickedOn = true;
		}
	}

	void OnMouseUp() {
		if(playing) {
			spring.enabled = true;
			rigidbody2D.isKinematic = false; //Eugenio
			clickedOn = false;
		}
	}

	void Dragging() {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if(mouseWorldPoint.y < groundLimit.transform.position.y)
			mouseWorldPoint.y = groundLimit.transform.position.y+renderer.bounds.extents.x;

		//This is the length of the LineRenderer
		Vector2 catapultToMouse = mouseWorldPoint - catapultBack.position;

		/*When we r comparing magnitude or values is much better to compare
		 * the square magnitude round the magnitude itself */
		if(catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint(maxStretch); //Find a point along that ray = maxStretch
		}
		mouseWorldPoint.z = 0.0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererSetup() {
		catapultLineFront.SetPosition(0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition(0, catapultLineBack.transform.position);
		
		catapultLineFront.sortingLayerName = "Foreground";
		catapultLineBack.sortingLayerName = "Foreground";
		
		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;
	}

	void LineRendererUpdate() {
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		frontCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = frontCatapultToProjectile.GetPoint(catapultToProjectile.magnitude + eugenioWidth);

		catapultLineFront.SetPosition(1, holdPoint);
		catapultLineBack.SetPosition(1, holdPoint);
	}

	public void StartGame() {
		gameScore.increaseAttempts();	//Attempts: 1/3

		attemptsText.text = gameScore.UpdateAttemptsText(); //Update Attempts text

		startButton.SetActive(!gameScore.getStatusButton()); //Hide Start button

		splash.SetActive(!gameScore.getStatusButton());	//Hide Splash

		playing = gameScore.getStatusButton();	//Now it's playing

		gameScore.changeStatusButton();
	}
 }
