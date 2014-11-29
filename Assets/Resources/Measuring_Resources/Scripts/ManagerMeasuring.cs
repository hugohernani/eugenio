using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ManagerMeasuring : MonoBehaviour {

	User user;
	UserInteraction userInteraction;
	MessageInteraction messageInteraction;
	DisplayNumberUser displayNumber;

	RectTransform transformItemHolder;
	RectTransform ItemPrefab;

	RectTransform itemToMeasure;
	Image itemImageComponent;
	BoxCollider2D boxCollider2DComponent;

	readonly string resourceRules = "Measuring_Resources/Textures/Ruler/";
	readonly string resourceItemPrefab = "Measuring_Resources/preFabs/Item";

	int itemMeasured;
	string qntS;

	int expectedAnswer;

	int initialX;
	int mediumX;
	int finalX;
	int multiplier;

	void Awake(){
		user = User.getInstance;
		userInteraction = GameObject.Find ("Manager").GetComponent<UserInteraction> ();
		messageInteraction = GameObject.Find ("Manager").GetComponent<MessageInteraction> ();
		displayNumber = GameObject.Find ("DisplayUserNumber").GetComponent<DisplayNumberUser> ();
		transformItemHolder = GameObject.Find ("ItemsContainer").transform as RectTransform;
		ItemPrefab = Resources.Load<RectTransform> (resourceItemPrefab);
	}
	
	// Use this for initialization
	void Start () {
		itemMeasured = 0;
		qntS = "";
		int stage = user.CurrentStage;
		applyRuler (stage);
		applyRulerReference (stage);
		generateItem ();
	}

	void applyRuler(int stage){
		Image RulerPicture = GameObject.Find ("Ruler").GetComponent<Image> ();
		RulerPicture.sprite = Resources.Load<Sprite> (resourceRules + stage);
	}

	void applyRulerReference(int stage){
		switch (stage) {
			case 3:
				multiplier = 3; // (limitRuler - initial) / 10 -> (50-20)/10
				break;
			case 4:
				multiplier = 10; // (limitRuler - initial) / 10 -> (100-0)/10
			break;
			default:
				multiplier = 1; // fase 1 and fase 2
				break;
		}
	}

	void generateItem(){

		itemToMeasure = Instantiate (ItemPrefab, transformItemHolder.position, Quaternion.identity) as RectTransform;

		itemToMeasure.transform.parent = transformItemHolder;

		itemImageComponent = itemToMeasure.GetComponent<Image> ();
		boxCollider2DComponent = itemToMeasure.GetComponent<BoxCollider2D> ();

		applyNewProperties ();
	}

	void applyNewProperties(){
		// loading a sprite
//		int randNumber = Random.Range (1, 4);
//		itemImageComponent.sprite = Resources.Load<Sprite> (resourceObjects + randNumber);

		// loading a color
		itemImageComponent.color = new Color (
			Random.Range(0f,1f),
			Random.Range(0f,1f),
			Random.Range(0f,1f)
			);
		
		int minX = Random.Range (1, 5);
		float minY = Random.Range(0.2f,0.5f);

		int maxX = Random.Range (5, 10);
		float maxY = Random.Range(minY+0.2f, 0.9f);

		itemMeasured = (maxX - minX);
		expectedAnswer = itemMeasured * multiplier;

		itemToMeasure.anchorMin = new Vector2 (minX * 0.1f, minY);
		itemToMeasure.anchorMax = new Vector2 (maxX * 0.1f, maxY);

		itemToMeasure.offsetMax = Vector2.zero;
		itemToMeasure.offsetMin = Vector2.zero;
		
		// collider adjust
		Rect rectItSeft = itemToMeasure.rect;
		boxCollider2DComponent.size = new Vector2 (rectItSeft.size.x, rectItSeft.size.y);

		// restart qntS
		qntS = "";
		
	}

	// used for NumberClick
	public void buttonSelected(string buttonNumber){
		if(qntS.Length < 2){
			qntS += buttonNumber;
		}else{
			messageInteraction.showMesssage("Apenas 2 numeros");
			qntS = "";
		}
		displayNumber.display (qntS);
	}

	// used for VerifyClick
	public void verifyAnswer(){
		int answer;
		int.TryParse (qntS, out answer);
		if(qntS == ""){
			messageInteraction.showMesssage("Voce precisa dizer o tamanho do item");
			return;
		}else if(expectedAnswer == answer){
			userInteraction.hit();
		}else{
			userInteraction.fail();
		}
		applyNewProperties();
		displayNumber.stopDisplay ();

	}

}
