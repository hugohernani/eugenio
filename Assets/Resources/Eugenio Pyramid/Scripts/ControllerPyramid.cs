using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllerPyramid : ControlInfoBar {

	private readonly float[] SF_PYRAMID_GAME_SCENE = {0.34f,0.45f};

	private Texture2D txtBackGameUpper, txtBackGameLower, txtBlock1, txtBlock2, titleTexture;
	private Rect rectGameUpper, rectGameLower, rectBlock, rectPyramid, rectChecker;
	private GUISkin mySkin;

	private List<int> randNumbers, orderedList, disorderedList;
	private List<Rect> listOfBlocks;

	private bool flagLoadBlocks = true, flagNewNumbers = true;

	private bool isDropped = false;
	private bool isDragging;
	private Rect draggingBlock;
	private int currentNumberDragging = -1, prevIndex = -1, prevDragIndex = -1;
	
	private float sizeSquareBlock;
	private float[] sizeGameUpper, posGameUpper, sizeGameLower, posGameLower, sizePyramid, posPyramid, sizeCheckButton, posCheckButton;

	//If you drag any of the blocks its skin will be changed
	private bool[] isOutOfPlace;

	//Variables to be used when it is being dragged
	private Vector2 posB, posA; //defaultPosition and currentPosition
	private Rect currentBlock;
	private bool[] isWalking;

	private User user;
	private int initial, final;

	//Used as an initializer of attached things such as styles and sfxs
	// AWAKE();
	protected override void Aberto() {
		user = User.getInstance;
	}

	// START();
	protected override void Inicializar() {
		
		//Load the textures2D that will be used in the background objects
		txtBackGameUpper = Resources.Load<Texture2D>("Eugenio Pyramid/Textures/ordenando_quadro1");
		txtBackGameLower = Resources.Load<Texture2D>("Eugenio Pyramid/Textures/ordenando_quadro2");
		
		mySkin = Resources.Load<GUISkin> ("Eugenio Pyramid/Skins/mySkin1");
		titleTexture = Resources.Load<Texture2D> ("Eugenio Pyramid/Textures/ordenando_titulo");
		
		sizeGameUpper = new float[2];
		posGameUpper = new float[2];
		
		sizeGameLower = new float[2];
		posGameLower = new float[2];
		
		sizePyramid = new float[2];
		posPyramid = new float[2];

		sizeCheckButton = new float[2];
		posCheckButton = new float[2];
		
		sizeSquareBlock = 0.0f;

		listOfBlocks = new List<Rect> (10);
		randNumbers = new List<int> (10);
		orderedList = new List<int>(10);
		disorderedList = new List<int>(10);

		isOutOfPlace = new bool[10];
		for (int i = 0; i < isOutOfPlace.Length; i++) {
			isOutOfPlace[i] = false;	
		}
		isWalking = new bool[10];
		for (int i = 0; i < isWalking.Length; i++) {
			isWalking[i] = false;	
		}

		final = getFinalValue();
		initial = getInitialValue();

	}

	// UPDATE();
	protected override void Atualizar() { 
		adjustSizes ();
		
		listOfBlocks = getBlocks(rectPyramid);

		if(flagNewNumbers){
			getRandNumbers(initial,final);
			flagNewNumbers = false; 
		}
	}

	void adjustSizes (){

		//Get ScoreBar parametres
		Rect rectScoreBar = getScoreBar ();
		Rect rectControlBar = getControlBar ();

		sizeGameLower [0] = rectScoreBar.width + rectControlBar.width + getCotaSideOrBottom ();
		sizeGameLower [1] = rectScoreBar.height - getCotaSideOrBottom();

		posGameUpper [0] = rectScoreBar.x;
		posGameUpper [1] = rectScoreBar.y + rectScoreBar.height + getCotaGameScene ();

		sizeGameUpper [0] = sizeGameLower [0];
		sizeGameUpper [1] = Screen.height - (posGameUpper [1] + sizeGameLower [1]);

		posGameLower [0] = posGameUpper [0];
		posGameLower [1] = posGameUpper[1] + sizeGameUpper[1] - getCotaSideOrBottom();

		rectGameUpper = new Rect (posGameUpper[0], posGameUpper[1], sizeGameUpper[0], sizeGameUpper[1]);
		rectGameLower = new Rect (posGameLower[0], posGameLower[1], sizeGameLower[0], sizeGameLower[1]);

		sizePyramid [0] = Screen.width * SF_PYRAMID_GAME_SCENE[0]; //0.4f
		sizePyramid [1] = Screen.height * SF_PYRAMID_GAME_SCENE [1]; //0.45f

		posPyramid [0] = (sizeGameUpper [0] - sizePyramid [0]) / 2;
		posPyramid [1] = posGameUpper [1] + (sizeGameUpper[1] - sizePyramid[1]);

		rectPyramid = new Rect(posPyramid[0], posPyramid[1], sizePyramid[0], sizePyramid[1]);

		sizeSquareBlock = sizePyramid [1] / 4;

		sizeCheckButton [0] = sizePyramid[0] / 2;
		sizeCheckButton [1] = sizeGameLower [1] - getCotaSideOrBottom () * 2.40f;

		posCheckButton [0] = (sizeGameLower[0] - sizeCheckButton [0]) / 2;
		posCheckButton [1] = posGameLower [1] + (getCotaSideOrBottom () * 1.60f);

		rectChecker = new Rect (posCheckButton [0], posCheckButton [1], sizeCheckButton [0], sizeCheckButton [1]);

	}

	// My_OnGUI();
	protected override void Draw() {

		drawTitle (titleTexture);

		drawGameBackground ();

		Event e = Event.current;

		drawCheckButton (e);

		drawPyramid (listOfBlocks, "brownPyramid", e);

		if(isDragging){

			GUI.Box(draggingBlock, "" + randNumbers[prevDragIndex],mySkin.GetStyle("brownPyramid"));

			posA = new Vector2(Event.current.mousePosition.x - 30, Event.current.mousePosition.y - 30);

			draggingBlock = new Rect(posA.x, posA.y, sizeSquareBlock, sizeSquareBlock); 
			GUI.Box(draggingBlock,"" + currentNumberDragging, mySkin.GetStyle("brownPyramid"));
		}
		if(prevDragIndex != -1) {
			if(!isDragging && isWalking[prevDragIndex]) {
				positionChanging();
			}
		}
	}

	void drawGameBackground() {

		GUI.DrawTexture (rectGameLower, txtBackGameLower);
		GUI.DrawTexture (rectGameUpper, txtBackGameUpper);
	}

	void drawCheckButton(Event e) {

		if(GUI.Button (rectChecker, "Verificar", mySkin.GetStyle("checkButton"))) {

			if (compareList (randNumbers, orderedList, disorderedList)) {
				showMessage ("Parabens!!!");
				hit();
				flagNewNumbers = true;
			} else { 
				showMessage("Ops, algo esta errado!");
				fail();
				flagNewNumbers = true;
			}
		}
	}

	void drawPyramid(List<Rect> list, string skin, Event e){

		for (int i = 0; i < list.Count; i++) {

			Rect currentRect = list[i];

			if (isDropped){

				if(prevIndex != prevDragIndex) {
					int number = randNumbers[prevIndex];
					randNumbers[prevIndex] = currentNumberDragging;
					randNumbers[prevDragIndex] = number;
				}
				isDropped = false;
			}
	 		
			if(!isOutOfPlace[i])
				GUI.Box(currentRect, "" + randNumbers[i],mySkin.GetStyle(skin));
			else
				GUI.Box(currentRect, "" + randNumbers[i],mySkin.GetStyle("yellowPyramid"));

			//If the mouse is into all blocks
			if(currentRect.Contains(e.mousePosition)){
				if(e.button == 0 && e.type == EventType.MouseDrag && !isDragging){
					if(prevDragIndex != -1) 
						isOutOfPlace[prevDragIndex] = false;
					isDragging = true;
					prevDragIndex = i;
					currentNumberDragging = randNumbers[i];

					currentBlock = currentRect;

					isOutOfPlace [prevDragIndex] = true;
				}
				if(isDragging && e.type == EventType.mouseUp){
					isDragging = false;
					isDropped = true;
					isWalking[prevDragIndex] = false;

					prevIndex = i;

					isOutOfPlace[prevDragIndex] = false;
				}

			}
			//If the mouse is out of all blocks
			if(!isMouseOverPyramid(list, e) && isDragging && e.type == EventType.mouseUp) {
				isWalking[prevDragIndex] = true;
				isDragging = false;
			}
		}
	}

	void positionChanging() {
//		Exemple:
//		Vector2 posA
//		Vector2 posB
//		GUI.Box (new Rect(posA.x,posA.y,50,50),"","box");
//		posA = Vector2.Lerp (posA, posB, 0.02f);
		 
		GUI.Box(new Rect(posA.x, posA.y, sizeSquareBlock, sizeSquareBlock),"" + currentNumberDragging, mySkin.GetStyle("brownPyramid"));

		posB = new Vector2(currentBlock.x, currentBlock.y);

		posA = Vector2.Lerp(posA, posB, 0.2f);

	}

	bool isMouseOverPyramid(List<Rect> list, Event e) {
		int cont = 0;
		for (int i = 0; i < list.Count; i++)
			if(list[i].Contains(e.mousePosition))
				cont++;
		if(cont >= 1) return true;
		else return false;

	}
	
	List<Rect> getBlocks(Rect pyramid){
		
		List<Rect> blocks = new List<Rect>(10);

		// x for pyramid.x, s for sizeSquareBlock, y for pyramid.y 
		// y --> I. y+3s
		// x --> I. x, II. x+s, III. x+2s, IV. x+3s
		blocks.Add(new Rect(pyramid.x, pyramid.y+3*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+sizeSquareBlock, pyramid.y+3*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+2*sizeSquareBlock, pyramid.y+3*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+3*sizeSquareBlock, pyramid.y+3*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));

		// y --> II. y+2s
		// x --> I. x+s/2, II. x+3s/2, III. x+5s/2
		blocks.Add(new Rect(pyramid.x+(sizeSquareBlock)/2, pyramid.y+2*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+(3*sizeSquareBlock)/2, pyramid.y+2*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+(5*sizeSquareBlock)/2, pyramid.y+2*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		//		blocks.Push(new Rect(pyramid.x+3*(sizeSquareBlock/2), pyramid.height-2*sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));

		// y --> III. y+s
		// x --> I. x+s, II. x+2s,
		blocks.Add(new Rect(pyramid.x+sizeSquareBlock, pyramid.y+sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));
		blocks.Add(new Rect(pyramid.x+2*sizeSquareBlock, pyramid.y+sizeSquareBlock, sizeSquareBlock,sizeSquareBlock));

		// y --> IV. y
		// x --> I. x + 3s/2
		blocks.Add(new Rect(pyramid.x+3*(sizeSquareBlock/2), pyramid.y, sizeSquareBlock,sizeSquareBlock));

		return blocks;
	}

	bool compareList(List<int> rand, List<int> ordered, List<int> disordered) {
		int cont = 0;
		for (int i = 0; i < rand.Count; i++) {
			if(rand[i] == ordered[i] || rand[i] == disordered[i]) cont++;
		}
		if (cont == rand.Count) return true;
		else return false;
	}
		
	void getRandNumbers (int min, int max){
		List<int> context = new List<int> ();
		orderedList.Clear ();
		disorderedList.Clear();
		randNumbers.Clear ();

		//TODO criar o disorderedList


		// the 4 lines beneath this comment is necessary to have a orderedList so that can be possible to
		// compare with the randNumbers in the end of the order process.
		for (int n = min; n <= max; n++) {
			orderedList.Add(n);
			if(min <= n && n <= min+3)			//(1 <= n <= 4)
				disorderedList.Add(n+6);	
			else if(min+4 <= n && n <= min+6)	//(5 <= n <= 7)
				disorderedList.Add(n-1);	
			else if(min+7 <= n && n <= min+8)	//(8 <= n <= 9)
				disorderedList.Add(n-6);	
			else if(n == max)					//(n = 10)
				disorderedList.Add(n-9);		
			context.Add(n);
		}

		while (context.Count > 0) {
			int numb = Random.Range(min,max+1);
			if(context.Contains(numb)){
				randNumbers.Add(numb);
				context.Remove(numb);
			}
		}
	}
}
