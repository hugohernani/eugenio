using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Counting : ControlInfoBar {

	// cota
	readonly float COTA_GAME_BAR = 6.1f;
	
	// Values of control
	readonly float[] numberSize = {110f,110f};
//	readonly float[] boxNumbersSize = {215,310};

	// defaultResource
	readonly string pathResources = "Counting Resources/";
	
	// Textures
	Texture2D titleTexture;
	
	// skins
	GUISkin skinScene;

//	Stack<Texture2D> items;
	List<Texture2D> items;
	
	//flags
	bool evaluateNumber = false;
	bool loadNewItems = true;

	// defaultHelpMessage
	readonly string helpMessage = "Sua mensagem de ajuda";


	readonly string[] messagesAnimo = {
		"Continue assim",
		"Muito bom!",
		"Excelente!"
	};
	readonly string[] messagesDesanimo = {
		"Voce errou. :/",
		"Esta errado. :/"
	};

	enum TypeOfLoad{
		ITEMS = 0,
		BUTTONS = 1
	}

	float[] sizeButtonsContainer;
	float[] posButtonsContainer;
	float[] sizeItemsContainer;
	float[] posItemsContainer;

	Rect rectButtonsContainer;
	Rect rectItemsContainer;


	string quantityS = "";

//	float timeShowItem = 0.4f;

	// FROM CONTROLINFOBAR
	protected override void Aberto(){

	}

	// FROM CONTROLINFOBAR
	protected override void Inicializar(){

		skinScene = Resources.Load<GUISkin> (pathResources + "Skins/mySkin");

		setHelpMessage(helpMessage);
		
		items = new List<Texture2D> ();
		
		titleTexture = Resources.Load<Texture2D> (pathResources + "Textures/Others/vamos_contar");

//		adjustRectDimensions ();

	}

	void adjustRectDimensions(){
		//itemsContainer
		sizeItemsContainer = new float[2];
		posItemsContainer = new float[2];

		Rect rectScoreBar = getScoreBar ();
		Rect rectControlBar = getControlBar();

		posItemsContainer[0] = rectScoreBar.x;
		posItemsContainer[1] = rectScoreBar.y + rectScoreBar.height + getCotaGameScene();

		sizeItemsContainer[0] = rectScoreBar.width;
		sizeItemsContainer[1] = Screen.height - posItemsContainer[1] - getCotaSideOrBottom();

		rectItemsContainer = new Rect (posItemsContainer[0], posItemsContainer[1], sizeItemsContainer[0], sizeItemsContainer[1]);

		//buttonsContainer
		sizeButtonsContainer = new float[2];
		posButtonsContainer = new float[2];

		posButtonsContainer[0] = rectControlBar.x;
		posButtonsContainer[1] = rectControlBar.y + rectControlBar.height + COTA_GAME_BAR;

		sizeButtonsContainer[0] = rectControlBar.width;
		sizeButtonsContainer[1] = sizeItemsContainer[1];

		rectButtonsContainer = new Rect (posButtonsContainer[0], posButtonsContainer[1], sizeButtonsContainer[0],sizeButtonsContainer[1]);


	}

	// FROM CONTROLINFOBAR
	protected override void Atualizar(){
		adjustRectDimensions ();

		if (evaluateNumber) {
			int quantity = -1;
			if (int.TryParse (quantityS, out quantity)){
				if(quantity == items.Count){
					string msg = messagesAnimo[Random.Range(0,messagesAnimo.Length)];
					showMessage(msg); // from ControlInfoBar
					hit();
				}else{
					string msg = messagesDesanimo[Random.Range(0,messagesDesanimo.Length)];
					showMessage(msg); // from ControlInfoBar
					fail();
					
				}

				loadNewItems = !isEndOfTheGame();

			}else{
				showMessage("Tente novamente");
			}
			quantityS = "";
			evaluateNumber = false;
		}
	}

	// FROM CONTROLINFOBAR
	protected override void Draw(){
		
		drawTitle (titleTexture);

		DrawBoxNumbers ();

		if (loadNewItems) {
//			itemsAnimate = true;
			items = loadItems ();
		}

		if (items.Count != 0) {
			showItems (items);
		}

		int quant = quantityS.Length;
		if(quant >= 0){
			if(quant >= 3){
				showMessage ("Maximo 3 digitos");
				quantityS = "";
			}
			drawDisplayQuantity();
		}

	}

	void drawDisplayQuantity(){

		float xPos = rectButtonsContainer.x - 10 * getCotaSideOrBottom();
		float yPos = getScreenHeight () - 5 * getCotaSideOrBottom();

		Rect rectDisplayQuantity = new Rect (
			xPos, yPos, 100, 30);


		GUILayout.BeginArea (rectDisplayQuantity);
		GUILayout.Label(quantityS, skinScene.GetStyle("displayQuantityStyle"));
		GUILayout.FlexibleSpace ();
		GUILayout.EndArea ();

	}
	
	void DrawBoxNumbers(){

		GUILayout.BeginArea (rectButtonsContainer);

		GUILayout.BeginVertical ();

		GUILayout.FlexibleSpace ();

		int buttonCount = 1;

		for(int row = 0; row < 3; row++){

			GUILayout.BeginHorizontal();

			for (int column = 0; column < 3; column++) {
				GUIContent contentNumber = new GUIContent ("" + buttonCount, quantityS);

				if(GUILayout.Button(contentNumber,skinScene.GetStyle("buttonStyle"),
				                    GUILayout.MaxWidth(numberSize[0]),GUILayout.MaxHeight(numberSize[1]))){
					if(!isEndOfTheGame()){
						playSoundInteraction();
						quantityS += contentNumber.text;
					}
				}
				buttonCount++;

//				GUILayout.FlexibleSpace();

			}

			GUILayout.EndHorizontal();

		}

		GUILayout.BeginHorizontal ();

		GUILayout.FlexibleSpace ();

		GUIContent contentButton0 = new GUIContent ("0", quantityS);

		if (GUILayout.Button ("0", skinScene.GetStyle ("buttonStyle"),
		                    GUILayout.MaxWidth (numberSize [0]), GUILayout.MaxHeight (numberSize [1]))) {

			if(!isEndOfTheGame()){
				playSoundInteraction();
				quantityS += contentButton0.text;
			}
		}

		if (GUILayout.Button ("=", skinScene.GetStyle ("buttonStyle"),
		                      GUILayout.MaxWidth (2*numberSize [0]), GUILayout.MaxHeight (numberSize [1]))) {

			if(!isEndOfTheGame()){
				playSoundInteraction();
				evaluateNumber = true;
			}
		}
		
		GUILayout.FlexibleSpace();

		
		GUILayout.EndHorizontal ();


		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical ();


		GUILayout.EndArea ();
		
	}
	
	List<Texture2D> loadItems(){

		User user = User.getInstance;

		MainCategory category = (MainCategory) user.CurrentCategory;

		int qntThisTime = Random.Range (category.InitialValue, category.FinalValue);
		int IDItemThisTime = Random.Range (2, 36);

		items = new List<Texture2D> (qntThisTime);
		
		Texture2D item = Resources.Load<Texture2D>(pathResources + "Textures/Items/" + IDItemThisTime);
		while ((qntThisTime-=1) >= 0) {
			items.Add(item);
		}
		
		return items;
		
	}
	
	// TODO make something animated
	void showItems(List<Texture2D> items){
		Stack<Texture2D> itemsLocal = new Stack<Texture2D> (items.Count);

		for (int i = 0; i < items.Count; i++) {
			itemsLocal.Push(items[i]);
		}

		GUILayout.BeginArea (rectItemsContainer, skinScene.GetStyle("itemsBoxStyle"));

		GUILayout.BeginVertical ();
		GUILayout.FlexibleSpace ();

		int qntRow = ((items.Count * 50)/100) + 1; // 50% and never 0

		float sizeItemW = rectItemsContainer.width/(qntRow * 1.05f);
		float sizeItemH = rectItemsContainer.height/(qntRow * 1.05f);

		for(int row = 0; row < qntRow; row++){
			if(itemsLocal.Count == 0) break; // guarantee do not try to draw more than items loaded

			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();

//			GUILayout.Space(25); //adjust
			int qntColumn = (qntRow/2) + 1;

			while (qntColumn > 0 && itemsLocal.Count != 0) {
				Texture2D item = itemsLocal.Pop();
				drawItem(item, sizeItemW, sizeItemH);
				qntColumn--;

			}
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}
		GUILayout.FlexibleSpace ();
		GUILayout.EndVertical();
		GUILayout.EndArea ();

		loadNewItems = false;
	}

	void drawItem(Texture2D item, float widthItem, float heightItem){
		GUILayout.Box(item, skinScene.GetStyle("itemStyle"), GUILayout.MaxWidth(widthItem), GUILayout.MaxHeight(heightItem));
		GUILayout.FlexibleSpace();
	}



}
