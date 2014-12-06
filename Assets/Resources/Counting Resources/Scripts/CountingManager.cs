using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CountingManager : MonoBehaviour {

	public GameObject prefab;
	public GameObject boxItemsGO;
	GridLayoutGroup gridBoxItems;

	Vector2 boxSize;

	// defaultResource
	readonly string pathResources = "Counting Resources/";

	List<GameObject> items;

	User user;
	public GameObject manager;
	UserInteraction userInteraction;
	MessageInteraction messageInteraction;
	public GameObject displayNumberGO;
	DisplayNumberUser displayNumber;

	int itemMeasured;
	string qntS;
	
	int expectedAnswer;


	void Awake(){
		gridBoxItems = boxItemsGO.GetComponent<GridLayoutGroup> ();

		user = User.getInstance;
		userInteraction = manager.GetComponent<UserInteraction> ();
		messageInteraction = manager.GetComponent<MessageInteraction> ();
		displayNumber = displayNumberGO.GetComponent<DisplayNumberUser> ();


		itemMeasured = 0;
		qntS = "";
	}

	// Use this for initialization
	void Start () {

		boxSize = (Vector2) boxItemsGO.collider2D.bounds.size;

		items = new List<GameObject> ();
		loadItems ();

	}

	List<GameObject> loadItems(){

		User user = User.getInstance;
		
		MainCategory category = (MainCategory) user.CurrentCategory;
		
		int qntThisTime = Random.Range (category.InitialValue, category.FinalValue);
		int IDItemThisTime = Random.Range (2, 36);

		Vector2 cellSize = new Vector2();

		if(qntThisTime > 0 && qntThisTime <= 10){
			cellSize = new Vector2(120, 100);
		}else if(qntThisTime >= 11 && qntThisTime <= 20){
			cellSize = new Vector2(90,80);
		}else if(qntThisTime >= 21 && qntThisTime <= 50){
			cellSize = new Vector2(50,50);
		}else if(qntThisTime >= 51 && qntThisTime <= 100){
			cellSize = new Vector2(40,25);
		}

		gridBoxItems.cellSize = cellSize;

		items = new List<GameObject> (qntThisTime);
		
		Sprite image = Resources.Load<Sprite>(pathResources + "Textures/Items/" + IDItemThisTime);
		while ((qntThisTime-=1) >= 0) {

			GameObject item = Instantiate(prefab, boxItemsGO.transform.position, Quaternion.identity) as GameObject;
			item.GetComponent<Image>().sprite = image;

			item.transform.parent = boxItemsGO.transform;

			items.Add(item);
		}

		expectedAnswer = items.Count;

		return items;
		
	}

	// used for NumberClick
	public void buttonSelected(Text textButton){
		string buttonNumber = textButton.text;
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
			messageInteraction.showMesssage("Voce precisa dizer a quantidade de itens");
			return;
		}else if(expectedAnswer == answer){
			userInteraction.hit();
			qntS = "";
		}else{
			userInteraction.fail();
			qntS = "";
		}
		clean ();
		loadItems ();
		displayNumber.stopDisplay ();
		
	}

	void clean(){
		foreach(GameObject goItem in items){
			Destroy(goItem);
		}

	}

}
