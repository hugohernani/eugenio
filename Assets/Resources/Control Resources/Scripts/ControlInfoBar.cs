using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/* @desc Class base where others gameScene can subclass
* to use some default functions and also
* the default GUI.
* @patterns: Template Method, Strategy.
* @patterns-location: Template Method -> Callback MonoBehavior Functions
* @patterns-location: Strategy -> Subclass make use of dynamic polymorfism.
*/
public abstract class ControlInfoBar : MonoBehaviour{

	// current scene and User
	User currentUser;
	SceneDatabase scenesDatabase;

	// default Resource
	readonly string DEFAULT_PATH = "Control Resources/";

	// cotas
	readonly float COTA_SIDE_AND_BOTTOM_SCREEN = 9.3f; // 20px in 1280x720
	readonly float COTA_TOP_SCREEN = 10.8f; // 23px in 1280x720
	readonly float COTA_BETWEEN_BARS = 8.4f; // 18px in 1280x720
	readonly float COTA_GAME_BAR = 6.1f; // 13px in 1280x720


	// scale factors used to adapter the size of items according of the screen and rectStageBarSize
	readonly float LARGE_SPACE_BETWEEN_ITEMS = 0.07f;
	readonly float SMALL_SPACE_BETWEEN_ITEMS = 0.035f;
	readonly float[] SF_RECT_TITLE = {0.80f,0.14f};
	readonly float[] SF_RECT_SCORE = {0.59f,0.16f};
	readonly float[] SF_RECT_STAGE = {0.2f, 0.5f};
	readonly float[] SF_RECT_EUGENIO = {0.25f, 1.4f};
	readonly float[] SF_RECT_HITS = {0.20f, 0.65f};
	readonly float[] SF_RECT_FAILS = {0.20f, 0.65f};

	// dimensioningVariables
	float sizeSmallSpaceBetweenItems, sizeLargeSpaceBetweenItems;
	float[] limitReferenceScore, sizeRectScore, posRectScore, sizeRectTitle, posRectTitle, sizeRectStage, posRectStage, sizeRectEugenio, posRectEugenio, sizeRectHits, posRectHits,
			sizeRectFails, posRectFails, sizeRectMessage, posRectMessage, sizeRectControl, posRectControl, sizeRectHelpMessage,
			posRectHelpMessage, sizeRectSound, posRectSound;

	//variables
	int currentStage, totalStage, qtyHit, qtyFail, fase;

	// timeMessage control
	float timeMessage;

	// textures
	Texture2D eugenioTexture, barScoreTexture, messageTexture, helpDialogTexture, background, noneTexture,
				TITLE_TEXTURE;

	// eugenioTextures
	Texture2D previousEugenio, eugenioFeliz, eugenioTriste, eugenioEsperando, eugenioNormal;


	//container of clips and host of the sound
	AudioSource audioSource;
	GameObject mainCamGameObject;


	// clips
	protected AudioClip clipBackground, clipAcerto, clipErro, clipOpenWindow, clipRestartScore, clipCloseScene, clipInteraction;

	// messages
	string message;
	string helpMessage = "";
	string endGameMessage = "";

	// rects
	Rect rectBarScore;
	Rect rectControl;
	
	Rect rectScore, rectTitle, rectStage, rectEugenio, rectHits, rectFails, rectMessage, rectbuttonSound;

	// skins
//	[SerializeField]
	GUISkin mainSkin;


	//flags
	bool flagMessage;
	bool flagHelpMessage;
	bool flagEndGame;
	bool flagControlMessageEndGame;
	bool flagRectBarDimensionsHasChanged;
	bool flagSound;

	// Callback
	void Awake(){
		mainCamGameObject = GameObject.Find ("Main Camera");
		audioSource = (AudioSource)mainCamGameObject.AddComponent("AudioSource");
		audioSource.loop = true;
		audioSource.volume = 0.6f;

		Aberto (); // abstract method. Subclass implement
	}

	/*
	 * @desc Subclass implement and add components instantiation to the Awake callback function
	 */
	protected abstract void Aberto ();

	/*
	 * @desc Variable initializations.
	 * CurrentStage caught.
	 * @see UserInfo
	 */
	void variablesInitializations(){
		currentStage = 1;
		totalStage = 10;
		qtyHit = 0;
		qtyFail = 0;
		MainCategory category = (MainCategory) currentUser.getCategory (currentUser.CurrentCategory.Id);
		fase = category.Stage;

		timeMessage = 4;
		
		message = "Interaction";
		if (helpMessage == "") {
			helpMessage = "Help message";
		}else if(endGameMessage == ""){
			endGameMessage = "Fim de jogo!";
		}
		
		previousEugenio = eugenioTexture;
		
		// flags
		flagHelpMessage = false;
		flagMessage = false;
		flagEndGame = false;
		flagControlMessageEndGame = false;
		flagSound = true;
		flagRectBarDimensionsHasChanged = true;
		
	}

	/*
	 * @desc Initialization of Resources
	 */
	void defaultInitializations(){

		mainSkin = Resources.Load<GUISkin> (DEFAULT_PATH + "Skins/skin");
		noneTexture = mainSkin.label.normal.background;

		TITLE_TEXTURE = noneTexture;
		
		//textures
		eugenioTexture = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Characters/eug_normal");
		messageTexture = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Main Textures/balao");
		
		Texture2D barBackground = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Main Textures/barBackground");
		barScoreTexture = barBackground;

		//		helpDialogTexture = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Main Textures/ajuda_quadro");
		helpDialogTexture = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Main Textures/ajuda_quadro full_screen");
		background = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Main Textures/background");

		
		eugenioFeliz = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Characters/eug_feliz");
		eugenioTriste = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Characters/eug_triste");
		eugenioEsperando = Resources.Load<Texture2D> (DEFAULT_PATH + "Textures/Characters/eug_olhar_cima");
		eugenioNormal = eugenioTexture;
		
		//audios and clipAudios
		clipBackground = Resources.Load<AudioClip> (DEFAULT_PATH + "Sounds/happy_rexalxing_moment");
		clipAcerto = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/Acertou");
		clipErro = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/Errou");
		clipOpenWindow = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/AbrirDialog");
		clipRestartScore = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/Restart");
		clipCloseScene = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/End_Fx"); // TODO this one HAS TO be changed
		clipInteraction = Resources.Load<AudioClip> (DEFAULT_PATH + "SFX/Tick");


		/*
		 * States verifications needed. 
		 */

		if (clipBackground != null)
			audioSource.clip = clipBackground;
		
	}

	// Use this for initialization. Callback
	/*
	 *@desc Callback function for initialization.
	 *UserInfo and SceneDatabase instances caught.
	 *@see UserInfo;
	 *@see SceneDatabase;
	 */
	void Start ()
	{
		currentUser = User.getInstance;
		scenesDatabase = SceneDatabase.getInstance;


		variablesInitializations ();
		defaultInitializations ();

		playBackground ();

		initializingDimensioningVariables ();

		adjustRectsSizes ();
		settingRectDimensions ();

		Inicializar (); // abstract method

	}

	/*
	 * @desc Subclass implement and add initializations to the Start callback function
	 */
	protected abstract void Inicializar();

	/*
	 * @desc dimensioningVariables initilizer
	 */
	void initializingDimensioningVariables(){

		limitReferenceScore = new float[2];

		sizeRectTitle = new float[2];
		posRectTitle = new float[2];

		sizeRectScore = new float[2];
		posRectScore = new float[2];

		sizeRectStage = new float[2];
		posRectStage = new float[2];

		sizeRectEugenio = new float[2];
		posRectEugenio = new float[2];

		sizeRectHits = new float[2];
		posRectHits = new float[2];

		sizeRectFails = new float[2];
		posRectFails = new float[2];

		sizeRectMessage = new float[2];
		posRectMessage = new float[2];
		sizeRectControl = new float[2];
		posRectControl = new float[2];
		sizeRectHelpMessage = new float[2];
		posRectHelpMessage = new float[2];
		
		sizeRectSound = new float[2];
		posRectSound = new float[2];

	}

	/*
	 * @desc Main control of flags and preSet for OnGUI
	 * It is a callback function called once per frame.
	 */
	void Update ()
	{
		//changingEugenio according to the hit/fail values
		if(qtyHit > qtyFail){
			previousEugenio = eugenioFeliz;
		}else if(qtyHit == qtyFail && !flagHelpMessage){
			previousEugenio = eugenioNormal; // can be another one or maybe eugenioTexture (initial default)
		}else if(qtyHit < qtyFail){
			previousEugenio = eugenioTriste;
		}

		if(flagRectBarDimensionsHasChanged){
			adjustRectsSizes ();
			settingRectDimensions ();
		}
		
		Atualizar (); // abstract method

	}

	/*
	 * @desc Subclass implement and add Update checks to the Update callback function
	 */
	protected abstract void Atualizar ();

	/*
	 * @desc Main method for adjust the Rect with the Screen.
	 * It is not necessary call this all the time in the Update in the final product.
	 * Remember to create flags to deal with change of screen as happens and "Screen Orientation" on mobile devices.
	 */
	void adjustRectsSizes (){

		sizeRectTitle [0] = Screen.width * SF_RECT_TITLE [0];
		sizeRectTitle [1] = Screen.height * SF_RECT_TITLE [1];

		posRectTitle [0] = COTA_SIDE_AND_BOTTOM_SCREEN;
		posRectTitle [1] = COTA_TOP_SCREEN;

		sizeRectScore [0] = Screen.width * SF_RECT_SCORE[0] + COTA_SIDE_AND_BOTTOM_SCREEN;
		sizeRectScore [1] = Screen.height * SF_RECT_SCORE [1];
		
		posRectScore [0] = COTA_SIDE_AND_BOTTOM_SCREEN;
		posRectScore [1] = posRectTitle[1] + sizeRectTitle[1] + COTA_TOP_SCREEN; // TODO adjust with the title

		// getting the limits of the rectBarScore
		limitReferenceScore [0] = sizeRectScore[0];
		limitReferenceScore [1] = sizeRectScore[1];

		// calculating the small space between items and to be used as a leftMargin
		sizeSmallSpaceBetweenItems = limitReferenceScore [0] * SMALL_SPACE_BETWEEN_ITEMS;
		// calculating the large (double of small) space between items that is going to be increased in the leftMargin
		sizeLargeSpaceBetweenItems = limitReferenceScore [0] * LARGE_SPACE_BETWEEN_ITEMS;

		// Updating the leftMargin
		float leftSpace = sizeLargeSpaceBetweenItems + sizeSmallSpaceBetweenItems/2;

		adjustRect (leftSpace, sizeRectStage, posRectStage, limitReferenceScore, SF_RECT_STAGE);

		leftSpace += sizeRectStage [0] + sizeLargeSpaceBetweenItems/2;

		adjustRect (leftSpace, sizeRectEugenio, posRectEugenio, limitReferenceScore, SF_RECT_EUGENIO);
		
		posRectEugenio [0] += posRectScore[0] + sizeSmallSpaceBetweenItems + sizeSmallSpaceBetweenItems/4;
		posRectEugenio [1] += posRectScore[1];

		sizeRectMessage [0] = 2 * sizeRectEugenio [0];
		sizeRectMessage [1] = sizeRectEugenio [1];

		posRectMessage [0] = posRectEugenio [0]/2 + sizeLargeSpaceBetweenItems + sizeSmallSpaceBetweenItems/2;
		posRectMessage [1] = posRectScore[1] + sizeRectScore[1] - COTA_TOP_SCREEN;


		leftSpace += sizeRectEugenio [0] + sizeSmallSpaceBetweenItems;

		adjustRect (leftSpace, sizeRectHits, posRectHits, limitReferenceScore, SF_RECT_HITS);

		leftSpace += sizeRectHits[0] - sizeSmallSpaceBetweenItems/2;

		adjustRect (leftSpace, sizeRectFails, posRectFails, limitReferenceScore, SF_RECT_FAILS);		


		float spaceBars = limitReferenceScore [0] + COTA_BETWEEN_BARS;

		posRectControl [0] = posRectScore[0] + spaceBars;
		posRectControl [1] = posRectScore[1];

		sizeRectControl [0] = Screen.width - posRectControl[0] - COTA_SIDE_AND_BOTTOM_SCREEN;
		sizeRectControl [1] = limitReferenceScore [1];

		sizeRectHelpMessage [0] = sizeRectScore[0];
		sizeRectHelpMessage [1] = sizeRectScore[1] * 4;

		posRectHelpMessage [0] = posRectScore[0];
		posRectHelpMessage [1] = posRectScore[1] + sizeRectScore[1] - COTA_GAME_BAR;

		sizeRectSound[0] = 3 * COTA_SIDE_AND_BOTTOM_SCREEN;
		sizeRectSound[1] = 3 * COTA_TOP_SCREEN;

		posRectSound[0] = Screen.width - 4*COTA_SIDE_AND_BOTTOM_SCREEN;
		posRectSound[1] = COTA_TOP_SCREEN;

	}

	/*
	 * @desc adjust the size and position of a single Rect according of the limitRect and proportionalValues passed
	 * Also receive a leftMargin and put in the x position of the Rect. It is called in the adjustRectsSizes
	 * @param leftMargin -> left Offset of the Rect
	 * @param sizeRect -> width and height of the Rect
	 * @param posRect -> x and y of the Rect
	 * @param limitRect -> screen limits reference
	 * @param proportialValues -> scale factors to adjust the size of the Rect with the Screen.
	 * @see adjustRectSizes
	 * @see scale factors constants
	 * @see Rect
	*/
	void adjustRect(float leftMargin, float[] sizeRect, float[] posRect, float[] limitRect, float[] proportialValues){
		for (int i = 0; i < sizeRect.Length; i++) {
			sizeRect[i] = limitRect[i] * proportialValues[i]; 
		}
		posRect [0] = leftMargin;
		posRect [1] = limitRect [1] / 2 - sizeRect [1] / 2;
	}

	/*
	 * @desc Setting up dimensions to the Rects.
	 * It should not be called before adjustRectsSizes method
	 * @see adjustRectSizes
	 */
	void settingRectDimensions(){
		
		rectTitle = new Rect (posRectTitle [0], posRectTitle [1], sizeRectTitle [0], sizeRectTitle [1]);
		rectBarScore = new Rect (posRectScore[0],posRectScore[1],sizeRectScore[0],sizeRectScore[1]);
		rectStage = new Rect (posRectStage[0], posRectStage[1], sizeRectStage[0], sizeRectStage[1]);
		rectEugenio = new Rect (posRectEugenio[0], posRectEugenio[1], sizeRectEugenio[0], sizeRectEugenio[1]);
		rectMessage = new Rect (posRectMessage[0], posRectMessage[1], sizeRectMessage[0], sizeRectMessage[1]);
		rectHits = new Rect (posRectHits[0], posRectHits[1], sizeRectHits[0], sizeRectHits[1]);
		rectFails = new Rect (posRectFails[0], posRectFails[1], sizeRectFails[0], sizeRectFails[1]);
		rectControl = new Rect (posRectControl[0], posRectControl[1], sizeRectControl[0], sizeRectControl[1]);
		rectbuttonSound = new Rect (posRectSound[0], posRectSound[1], sizeRectSound[0], sizeRectSound[1]);
	}

	/*
	 * Callback to draw on the Screen.
	 */
	void OnGUI(){

		// background paint.
		drawBackground ();

		// bars
		drawBarScoreAndControl();

		Draw(); // abstract method.

		// sound Button paint.
		drawSoundButton ();

		// flag to control when message is shown.
		if (flagMessage) {
			showMessage (); // deal with message
		}

		// flag to deal with helpMessage
		if(flagHelpMessage){
			showBigHelpMessage();
		}

	}

	/*
	 * @desc Subclass implement and add "Paints" to the OnGUI callback function
	 */
	protected abstract void Draw();

	/*
	 * @desc draw the background on the whole Screen.
	 * @param backgroundParameter Retrieve a background gave from the user-programmer. If not, deal with defaultBackground.
	 * @see Texture2D
	 */
	protected void drawBackground(Texture2D backgroundParameter = null){
		if (background == null) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), backgroundParameter);
		}else{
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), background);
		}

	}

	/*
	 * @desc draw the title of the GameScene on the Screen.
	 * @return Rect -> Can be used to adjust others rect taken it as a reference.
	 * @param textureArg Retrieve a Texture2D gave from the user-programmer.
	 * @see Texture2D
	 * @see Rect
	 */
	protected Rect drawTitle(Texture2D textureArg){
		if (textureArg != null)
			TITLE_TEXTURE = textureArg;
		GUI.DrawTexture (rectTitle, TITLE_TEXTURE);

		return rectTitle;

	}

	/*
	 * @desc Draw the barScore and delegate to drawBarControl method which will deal with the other bar.
	 * @see GUI
	 * @see GUILayout
	 */
	void drawBarScoreAndControl(){

		GUI.DrawTexture (rectBarScore, barScoreTexture);
		GUILayout.BeginArea (rectBarScore);
		drawStage();
		drawHits ();
		drawFails();
		GUILayout.EndArea ();

		// above others drawings
		drawEugenioG(); // has to be out of the BeginArea above

		drawBarControl ();

	}

	/*
	 * @desc Draw the current Stage of the User and also the step of the game
	 * @see GUILayout
	 */
	void drawStage(){

		GUILayout.BeginArea (rectStage);

		GUILayout.FlexibleSpace ();

		string tempS = "Fase " + fase + "\n" + currentStage + "/" + totalStage;
		GUILayout.Label (tempS,mainSkin.GetStyle("stageStyle"));

		GUILayout.FlexibleSpace ();

		GUILayout.EndArea ();


	}

	/*
	 * @desc draw Eugenio OUT of the barRect. To do that it was needed to use GUI instead of GUILayout
	 * @see GUI
	 * @see GUILayout
	 */
	void drawEugenioG(){
		
		GUI.DrawTexture (rectEugenio, eugenioTexture,ScaleMode.ScaleToFit);

	}

	/*
	 * @desc draw Hits Text and Value according with rectHits and using GUiLayout.
	 * @see GUILayout
	 */
	void drawHits(){

		GUILayout.BeginArea (rectHits);

		GUILayout.BeginVertical ();

		GUILayout.FlexibleSpace ();

		GUILayout.Label ("Acertos",mainSkin.GetStyle("hitStyleText"));

		GUILayout.Space (8);

		GUILayout.Label ("" + qtyHit, mainSkin.GetStyle("hitStyleValue"));

		GUILayout.FlexibleSpace ();

		GUILayout.EndVertical ();
		GUILayout.EndArea ();

	}

	/*
	 * @desc draw Fails Text and Value according with rectHits and using GUiLayout.
	 * @see GUILayout
	 */
	void drawFails(){

		GUILayout.BeginArea (rectFails);
		
		GUILayout.BeginVertical ();

		GUILayout.FlexibleSpace ();

		GUILayout.Label ("Erros", mainSkin.GetStyle("failStyleText"));
		
		GUILayout.Space (8);
		
		GUILayout.Label ("" + qtyFail, mainSkin.GetStyle("failStyleValue"));

		GUILayout.FlexibleSpace ();
		
		GUILayout.EndVertical ();
		GUILayout.EndArea ();

	}

	/*
	 * @desc delegate Control Buttons painters to deal with Help, Restart and Close buttons.
	 * @see GUILayout
	 */
	void drawBarControl(){

		GUILayout.BeginArea (rectControl);
		GUILayout.BeginHorizontal ();

		drawHelpButton ();
		drawRestartButton ();
		drawCloseButton ();

		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();

	}

	/*
	 * Draw HelpButton using Skin and Style and deal with the click of this button.
	 * @desc Also make use of flagControlMessageEndGame and flagHelpMessage to make sure the correct time
	 * of showing the helpMessage to the user. And also deal with sound of open a dialog.
	 * @see GUILayout
	 * @see Skin (Unity documentation)
	 * @see Style (Unity documentation)
	 */
	void drawHelpButton(){

		if (GUILayout.Button (noneTexture, mainSkin.GetStyle ("helpButtonStyle"))) {
			if (!flagControlMessageEndGame){
				bool flag = (flagHelpMessage = !flagHelpMessage); // I made like this just for good programming aspects
				changePreviousEugenio(flag);
				audioSource.PlayOneShot(clipOpenWindow);
				showHelpMessage();
			}
		}

	}

	/*
	 * Delegate the obligation of dealing with the click in Restart Button
	 * @see GUILayout
	 */
	void drawRestartButton(){

		if (GUILayout.Button (noneTexture, mainSkin.GetStyle ("restartButtonStyle"))) {
			restart ();
		}

	}

	/*
	 * Deal with the sound of closing the task.
	 * Delegate the obligation of dealing with the click in Close Button
	 * @see GUILayout
	 */
	void drawCloseButton(){

		if (GUILayout.Button (noneTexture, mainSkin.GetStyle ("closeButtonStyle"))) {
			audioSource.PlayOneShot(clipCloseScene);
			closeApplication();
		}

	}

	/*
	 * Deal soundButton drawing.
	 * @desc Make use of flagSound to make sure the correct time when a the wanted Style will be used.
	 * Make use of GUI as a almost constant place on the screen.
	 * @see GUI
	 * @see Style (Unity documentation)
	 */
	void drawSoundButton(){

		if (flagSound) {
			if(GUI.Button(rectbuttonSound, noneTexture, mainSkin.GetStyle("soundButtonStyle"))){
				flagSound = false;
				audioSource.mute = flagSound;
			}
		}else{
			if(GUI.Button(rectbuttonSound, noneTexture, mainSkin.GetStyle("notSoundButtonStyle"))){
				flagSound = true;
				audioSource.mute = flagSound;

			}
		}

		

	}


	protected int hit(){
		increaseStage ();
		audioSource.PlayOneShot (clipAcerto);
		eugenioHappy ();
//		anim.SetTrigger ("Yes");
		qtyHit++;
		currentUser.TaskPoints = qtyHit;
		currentUser.SaveTaskHits();
		return qtyHit;
	}

	protected int fail(){
		increaseStage ();
		audioSource.PlayOneShot (clipErro);
		eugenioSad ();
//		anim.SetTrigger("Nao");
		return ++qtyFail;
	}


	protected int increaseStage(){
		if(currentStage < totalStage){
			currentStage++;
		}else{
			audioSource.PlayOneShot(clipInteraction);
			scenesDatabase.pushScene(Application.loadedLevel);
			
			Application.LoadLevel("EndTask");
			Destroy(gameObject);

		}
		return currentStage;
	}

	protected void restart(){
		audioSource.PlayOneShot(clipOpenWindow);
		Application.LoadLevel(Application.loadedLevel);
	}

	protected void closeApplication(){
		Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));
		Application.LoadLevel ("MainMenu");
	}

	protected void setEndGameMessage(string msg){
		this.endGameMessage = msg;
	}
	
	protected void showMessage(string msg){
		this.flagMessage = true; // this is necessary when this method is called from outside
		this.message = msg;
	}

	protected void setHelpMessage(string msg){
		this.helpMessage = msg;

	}

	void showHelpMessage(){
//		changeBackgroundHelpButton ();

//		GUILayout.BeginArea (rectMessageHelp, helpDialogTexture);
		
//		GUILayout.Label (helpMessage, mainSkin.GetStyle ("messageHelpStyle"));
		
//		GUILayout.EndArea ();

	}

	void showBigHelpMessage(){		

		float width = Screen.width - COTA_SIDE_AND_BOTTOM_SCREEN;
		float height = Screen.height - COTA_SIDE_AND_BOTTOM_SCREEN;

		float posX = Screen.width - width;
		float posY = Screen.height - height;

		Rect helpRect = new Rect (posX, posY,width - COTA_SIDE_AND_BOTTOM_SCREEN,height - COTA_SIDE_AND_BOTTOM_SCREEN);

		GUILayout.BeginArea (helpRect);

		GUI.DrawTexture (helpRect, helpDialogTexture);
		
		GUILayout.BeginVertical ();

		GUILayout.Label(helpMessage,mainSkin.GetStyle("messageHelpStyle"));

		GUILayout.FlexibleSpace ();

		GUILayout.BeginHorizontal ();

		GUILayout.FlexibleSpace ();

//		GUILayout.Space (55);

		if (GUILayout.Button ("Jogar", mainSkin.GetStyle ("closeButtonHelpStyle"))) {
			bool flag = (flagHelpMessage = !flagHelpMessage);
			changePreviousEugenio(flag);
		}

//		GUILayout.Space(55);

		GUILayout.FlexibleSpace ();

		GUILayout.EndHorizontal ();

		GUILayout.EndVertical ();
		
		GUILayout.EndArea ();

	}


	void showMessage(){
		GUILayout.BeginArea (rectMessage,messageTexture);
		
		GUILayout.Label (message, mainSkin.GetStyle ("messageStyleDefault"));
		
		GUILayout.EndArea ();
		
		if ((timeMessage -= Time.deltaTime) <= 0.0) {
			timeMessage = 4;
			flagMessage = false;
			changePreviousEugenio(flagMessage);
		}
	}

	protected void playBackground(){
		audioSource.Play ();

	}
	
	protected void stopBackground(){
		audioSource.Stop ();
	}

	protected void playSoundInteraction(){
		audioSource.PlayOneShot (clipInteraction);
	}

	protected void changePreviousEugenio(bool flag){
		if (flag) {
			eugenioTexture = eugenioEsperando;
		}else{
			eugenioTexture = previousEugenio;
		}
	}

	protected void eugenioHappy(){
		eugenioTexture = eugenioFeliz;
	}

	protected void eugenioSad(){
		eugenioTexture = eugenioTriste;
	}

	 void eugenioWaiting(){
		eugenioTexture = eugenioEsperando;
	}

	protected Rect getScoreBar(){
		return rectBarScore;
	}

	protected Rect getControlBar(){
		return rectControl;
	}

	protected void setScoreBarValues(float left, float top, float width, float height){
		this.rectBarScore = new Rect (left, top, width, height);
		flagRectBarDimensionsHasChanged = true;
	}

	protected bool isEndOfTheGame(){
		return flagEndGame;
	}

	protected float getScreenWidth(){
		// TODO
		return Screen.width;
	}

	protected float getScreenHeight(){
		// TODO
		return Screen.height;
	}

	protected float getCotaSideOrBottom(){
		return COTA_SIDE_AND_BOTTOM_SCREEN;
	}

	protected float getCotaTop(){
		return COTA_TOP_SCREEN;
	}

	protected float getCotaGameScene(){
		return COTA_GAME_BAR;
	}

}

