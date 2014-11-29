using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions; // needed for Regex 

/**
* This class manages the display of information
* 
* @author David Cesar
*/

public class EstudarScript : MonoBehaviour {
	/* Declaring variables */	
	/* Size Screen */	
	private float originalWidth = 1024.0f;
	private float originalHeight = 768.0f;
	private float calcPixelScreenWidht = 1024.0f;
	private float calcPixelScreenHeight = 768.0f;

	/* Multiplication Factors Relative to the screen */
	private const float FM_RESULT_TEXT = 0.80f; 
	private Vector2 FM_MAIN_TITLE = new Vector2 (0.5898f, 0.1484f);
	private Vector2 FM_BG_PERFORMANCE = new Vector2 (0.5898f, 0.1563f); 
	private Vector2 FM_BUTTON_HELP = new Vector2 (0.1172f, 0.1563f);
	private Vector2 FM_BUTTON_RESTART = new Vector2 (0.1172f, 0.1563f);
	private Vector2 FM_BUTTON_EXIT = new Vector2 (0.1172f, 0.1563f);
	private Vector2 FM_CHECK_BUTTON = new Vector2 (0.2295f, 0.0755f); 
	private Vector2 FM_EUGENIO = new Vector2 (0.1162f, 0.1758f);
	private Vector2 FM_BALLOON = new Vector2 (0.2764f, 0.1784f);
    private Vector2 FM_HELP = new Vector2 (0.9434f, 0.9245f);
	private Vector2 FM_BUTTON_BOX_HELP = new Vector2 (0.2354f, 0.1563f);
	private Vector2 FM_BUTTON_SOUND = new Vector2 (0.0361f, 0.0482f);

	/* Sizes */
	private Vector2 sizeBackground;
	private Vector2 sizeBackgroundPer;
	private Vector2 sizeMainTitle;
	private Vector2 sizeBlackBoard;
	private Vector2 sizeResultText;
	private Vector2 sizeButtonHelp;
	private Vector2 sizeButtonRestart;
	private Vector2 sizeButtonExit;
	private Vector2 sizeCheckButton;
	private Vector2 sizeEugenio;
	private Vector2 sizeTextLevel;
	private Vector2 sizeTextChlMet;
	private Vector2 sizeTextHits;
	private Vector2 sizeTextWrong;
	private Vector2 sizeQtyHits;
	private Vector2 sizeQtyWrong;
    private Vector2 sizeBalloon;
    private Vector2[] sizeTextBalloon; 
    private Vector2 sizeHelpBox;
	private Vector2 sizeButtonBoxHelp;
	private Vector2 sizeButtonSound;

	/* Positions */
	private Vector2 posBackground;
	private Vector2 posBackgroundPer;
	private Vector2 posMainTitle;
	private Vector2 posBlackBoard;
	private Vector2 posResultText;
	private Vector2 posOperandsText;
	private Vector2 posButtonHelp;
	private Vector2 posButtonRestart;
	private Vector2 posButtonExit;
	private Vector2 posCheckButton;
	private Vector2 posEugenio;
	private Vector2 posTextLevel;
	private Vector2 posTextChlMet;
	private Vector2 posTextHits;
	private Vector2 posTextWrong;
	private Vector2 posQtyHits;
	private Vector2 posQtyWrong;
    private Vector2 posBalloon;
    private Vector2[] posTextBalloon;
	private Vector2 posHelpBox;
	private Vector2 posButtonBoxHelp;
	private Vector2 posButtonSound;	
	
	/* Private properties */
	private System.Random rndNumb;
	private int op1;
	private int op2;
	private int sizeFontExp;
	private int sizeFontLevel;
	private int sizeFontScoreText;
	private int sizeFontScoreValue;
    private int sizeFontTextBalloon;
	private int sizeFontButtonBoxHelp;
	private int qtyHits;
	private int currentStateTotal;
	private int currentStateInst;
	private int qtyWrong;
	private int qtyPlayedChallenge;
	private int qtyLimitChallenge;
	private int level;
	private string operatorType;
	private string resultText;
	private string operationText;
	private string levelText;
	private string chlMetText;
	private string hitsText = "Acertos";
	private string wrongText = "Erros";
    private string[] balloonText;
    private string nameFontExp = "Font/comicsansms";
	private string nameFontInf = "Font/grobold";
	private bool showBalloonHit;
    private bool showBalloonWrong;
    private bool showBoxHelp;
	private bool soundIsOn;
	private GUIStyle myStyle;
	private GUIText operation;
	private GameObject go; 	
	//proportion balloon in relation to speach part
    private float FM_BALLOON_PROP = 0.4433f;
	private ControllerGeneral controller;

	/* Publics properties */	
	/* Texture properties */
	public Texture backgroundTexture;
	public Texture mainTitleTexture;
	public Texture performanceTexture;
	public Texture blackboardTexture;
	public Texture[] eugenioTexture;
    public Texture balloonTexture;
    public Texture helpTexture;
	
	/* GUIStyle properties */
	public GUIStyle buttonHelpStyle;
	public GUIStyle buttonRestartStyle;
	public GUIStyle buttonExitStyle;
	public GUIStyle buttonCheckStyle;
	public GUIStyle buttonBoxHelpStyle;
	public GUIStyle buttonSoundOn; 
	public GUIStyle buttonSoundOff;
	
	/* AudioClip properties */
	public AudioClip wrongAnswerSound;
	public AudioClip correctAnswerSound;
	public AudioClip typeWriterSound;
	public AudioClip buttonClickSound;

    /** 
	* Use this for initialization 
	*/
	void Start () {
		controller = new ControllerGeneral();
		// Initializing variables
		rndNumb = new System.Random ();
		op1 = 0;
		op2 = 0;
		operatorType = "+";
		resultText = "";
		operationText = "";
		balloonText = new string [2] {"Acertou!", "Errou!"};
        showBalloonHit = false;
        showBalloonWrong = false;
		showBoxHelp = false;
		soundIsOn = true;
		currentStateTotal = 0;
		currentStateInst = 0;
		qtyHits = 0;
		qtyWrong = 0;
		level = controller.getLevelQuantoEh();
		qtyLimitChallenge = controller.getQtyChlQuantoEh();
		qtyPlayedChallenge = 0;

		// Instance variables
		sizeBackground = new Vector2 ();
		sizeMainTitle = new Vector2 ();
		sizeBackgroundPer = new Vector2 ();
		sizeBlackBoard = new Vector2 ();
		sizeResultText = new Vector2 ();
		sizeButtonHelp = new Vector2 ();
		sizeButtonRestart = new Vector2 ();
		sizeButtonExit = new Vector2 ();
		sizeEugenio = new Vector2 ();
		sizeTextLevel = new Vector2 ();
		sizeTextChlMet = new Vector2 ();
		sizeTextHits = new Vector2 ();
		sizeTextWrong = new Vector2 ();
		sizeQtyHits = new Vector2 ();
		sizeQtyWrong = new Vector2 ();
        sizeBalloon = new Vector2 ();
        sizeTextBalloon = new Vector2 [2];
        sizeHelpBox = new Vector2 ();
		sizeButtonBoxHelp = new Vector2 ();
		sizeButtonSound = new Vector2 ();

		posBackground = new Vector2 ();
		posMainTitle = new Vector2 ();
		posBackgroundPer = new Vector2 ();
		posBlackBoard = new Vector2 ();
		posOperandsText = new Vector2 ();
		posButtonHelp = new Vector2 ();
		posButtonRestart = new Vector2 ();
		posButtonExit = new Vector2 ();
		posEugenio = new Vector2 ();
		posTextLevel = new Vector2 ();
		posTextChlMet = new Vector2 ();
		posTextHits = new Vector2 ();
		posTextWrong = new Vector2 ();
		posQtyHits = new Vector2 ();
		posQtyWrong = new Vector2 ();
        posBalloon = new Vector2 ();
        posTextBalloon = new Vector2 [2];
        posHelpBox = new Vector2 ();
		posButtonBoxHelp = new Vector2 ();
		posButtonSound = new Vector2 ();

        /* Background */
		// Set size
		sizeBackground.x = Screen.width;
		sizeBackground.y = Screen.height;
		posBackground.x = 0;
		posBackground.y = 0;

		/* Main Title */
		// Set size
		sizeMainTitle.x = Screen.width * FM_MAIN_TITLE.x;
		sizeMainTitle.y = Screen.height * FM_MAIN_TITLE.y;
		// Set positions
		posMainTitle.x = (Screen.width - sizeMainTitle.x) / 2.0f;
		posMainTitle.y = 10 * Screen.height / calcPixelScreenHeight;

		/* BlackBoard */
		// Set size
		sizeBlackBoard.x = (calcPixelScreenWidht - 40) 
                            * Screen.width / calcPixelScreenWidht;
		sizeBlackBoard.y = sizeBlackBoard.x * 0.4864f;
		// Set positions
		posBlackBoard.x = (Screen.width - sizeBlackBoard.x) / 2.0f;
		posBlackBoard.y = Screen.height - sizeBlackBoard.y - 
						  ((20) * Screen.height / calcPixelScreenHeight);

		/* Background Per */
		// Set size
		sizeBackgroundPer.x = Screen.width * FM_BG_PERFORMANCE.x;
		sizeBackgroundPer.y = Screen.height * FM_BG_PERFORMANCE.y;
		// Set positions
		posBackgroundPer.x = 20 * Screen.width / calcPixelScreenWidht;
		posBackgroundPer.y = posBlackBoard.y - 
                            (13 * Screen.height / calcPixelScreenHeight) 
							- sizeBackgroundPer.y;;

		/* Font */
		sizeFontExp = (int) sizeBlackBoard.y / 4;

		/* Result Text */
		// Set size
		sizeResultText.x = sizeBlackBoard.x * FM_RESULT_TEXT;
		sizeResultText.y = sizeFontExp + 50;
		// Set positions
		posResultText.x = posBlackBoard.x + 
                           ((sizeBlackBoard.x - sizeResultText.x) / 2.0f);
		posResultText.y = posBlackBoard.y + 
                           ((sizeBlackBoard.y - sizeResultText.y) / 3.0f);
	
		/* Operands Text */
		createOperation ();
	
		// Set text
		updateExpressionTexts ();
		Vector2 sizeOperationText = calcSizeText (operationText, 
                                                  nameFontExp, sizeFontExp);
		// Set positions
		posOperandsText.x = posBlackBoard.x + 
                            ((sizeBlackBoard.x - sizeOperationText.x)/2);
		posOperandsText.y = posBlackBoard.y + 2 * (sizeBlackBoard.y / 5);

		/* Button Help */
		// Set size 
		sizeButtonHelp.x = Screen.width * FM_BUTTON_HELP.x;
		sizeButtonHelp.y = Screen.height * FM_BUTTON_HELP.y;
		// Set positions
		posButtonHelp.x = 38 * Screen.width / calcPixelScreenWidht 
                            + sizeBackgroundPer.x;
		posButtonHelp.y = (20 * Screen.height / calcPixelScreenHeight) 
                            + sizeMainTitle.y;

		/* Calculating space between the buttons */
		float spaceBetweenButtons = Screen.width - 
                        (58 * Screen.width / calcPixelScreenWidht) -
						sizeBackgroundPer.x - 3 * sizeButtonHelp.x;
	
		/* Button Restart */
		// Set size
		sizeButtonRestart.x = Screen.width * FM_BUTTON_RESTART.x;
		sizeButtonRestart.y = Screen.height * FM_BUTTON_RESTART.y;
		// Set positions
		posButtonRestart.x = 38 * Screen.width / calcPixelScreenWidht 
                        + sizeBackgroundPer.x + sizeButtonHelp.x 
                        + spaceBetweenButtons;
		posButtonRestart.y = (20 * Screen.height / calcPixelScreenHeight) 
                        + sizeMainTitle.y;

		/* Button Exit */
		// Set size
		sizeButtonExit.x = Screen.width * FM_BUTTON_EXIT.x;
		sizeButtonExit.y = Screen.height * FM_BUTTON_EXIT.y;;
		// Set positions
		posButtonExit.x = 38 * Screen.width / calcPixelScreenWidht 
                        + sizeBackgroundPer.x + sizeButtonHelp.x 
                        + sizeButtonRestart.x + 2*spaceBetweenButtons;
		posButtonExit.y = (20 * Screen.height / calcPixelScreenHeight) 
                        + sizeMainTitle.y;

		/* Check Button */
		// Set size
		sizeCheckButton.x = Screen.width * FM_CHECK_BUTTON.x;
		sizeCheckButton.y = Screen.height * FM_CHECK_BUTTON.y;
		// Set positions
		posCheckButton.x = posBlackBoard.x 
                        + ((sizeBlackBoard.x - sizeCheckButton.x)/2);
		posCheckButton.y = posBlackBoard.y + 3 * (sizeBlackBoard.y / 4);

		/* Eugenio */
		// Set size
		sizeEugenio.x = Screen.width * FM_EUGENIO.x;
		sizeEugenio.y = Screen.height * FM_EUGENIO.y;
		// Set positions
		posEugenio.x = posBackgroundPer.x 
                        + (sizeBackgroundPer.x - sizeEugenio.x)/2;
		posEugenio.y = posBackgroundPer.y 
                        + (sizeBackgroundPer.y - sizeEugenio.y)/2;
        
        /* Balloon */
        // Set size
        sizeBalloon.x = Screen.width * FM_BALLOON.x;
        sizeBalloon.y = Screen.height * FM_BALLOON.y;
        // Set positions
        posBalloon.x = posEugenio.x + (sizeEugenio.x / 2) 
                        - (sizeBalloon.x * FM_BALLOON_PROP);
        posBalloon.y = posEugenio.y + (sizeEugenio.y);

		/* Text Level */
		levelText = "Fase " + controller.getLevelQuantoEh().ToString();
		sizeFontLevel = (int) sizeBackgroundPer.y / 3;
		sizeTextLevel = calcSizeText (levelText, nameFontInf, sizeFontLevel);

		// Set positions
		posTextLevel.x = posBackgroundPer.x + 
			((posEugenio.x - posBackgroundPer.x) - sizeTextLevel.x)/2;
		posTextLevel.y = posBackgroundPer.y + (sizeBackgroundPer.y 
                        - 2*sizeTextLevel.y) / 3;
       
		/* Text Chl Met */
		chlMetText = qtyPlayedChallenge + " / " + qtyLimitChallenge;
		sizeTextChlMet = calcSizeText (chlMetText, nameFontInf, sizeFontLevel);

		// Set positions
		posTextChlMet.x = posBackgroundPer.x + 
			((posEugenio.x - posBackgroundPer.x) - sizeTextChlMet.x)/2;
		posTextChlMet.y = posBackgroundPer.y + 2*(sizeBackgroundPer.y
						- 2*sizeTextLevel.y) / 3
						+ sizeTextLevel.y;	

        /* Text Balloon */
        sizeFontTextBalloon = (int)sizeBalloon.y / 3;

        sizeTextBalloon[0] = calcSizeText (balloonText[0], nameFontInf, 
                                        sizeFontTextBalloon);
        sizeTextBalloon[1] = calcSizeText (balloonText[1], nameFontInf,
                                        sizeFontTextBalloon);

        // Set positions
        posTextBalloon[0].x = posBalloon.x 
                              + (sizeBalloon.x - sizeTextBalloon[0].x)/2; 
        posTextBalloon[0].y = posBalloon.y + sizeBalloon.y/3f;


        posTextBalloon[1].x = posBalloon.x 
                              + (sizeBalloon.x - sizeTextBalloon[1].x)/2; 

        posTextBalloon[1].y = posBalloon.y + sizeBalloon.y/3f;
		
        /* Text Hits wrong values */
		sizeFontScoreText = (int)sizeBackgroundPer.y / 7;
		sizeFontScoreValue = (int)sizeBackgroundPer.y / 3;

		sizeTextHits = calcSizeText (hitsText, nameFontInf, sizeFontScoreText);
		sizeTextWrong = calcSizeText (wrongText, nameFontInf, 
                                      sizeFontScoreText);
		sizeQtyHits = calcSizeText (qtyHits.ToString(), nameFontInf, 
                                    sizeFontScoreValue);
		sizeQtyWrong = calcSizeText (qtyWrong.ToString(), nameFontInf, 
                                    sizeFontScoreValue);

		// Calculating space between the score texts
		Vector2 spaceBetweenTexts = new Vector2 ();
		spaceBetweenTexts.x = ((posBackgroundPer.x + sizeBackgroundPer.x) 
                                - (posEugenio.x + sizeEugenio.x) -
						        sizeTextHits.x - sizeTextWrong.x) / 3;
		spaceBetweenTexts.y = (sizeBackgroundPer.y - sizeTextHits.y 
                                - sizeQtyHits.y)/3;

		// Set positions
		posTextHits.x = posEugenio.x + sizeEugenio.x + spaceBetweenTexts.x;
		posTextHits.y = posBackgroundPer.y + spaceBetweenTexts.y;

		posTextWrong.x = posTextHits.x + sizeTextHits.x + spaceBetweenTexts.x;
		posTextWrong.y = posBackgroundPer.y + spaceBetweenTexts.y;

		posQtyHits.x = posTextHits.x + (sizeTextHits.x - sizeQtyHits.x) / 2;
		posQtyHits.y = posBackgroundPer.y + 2*spaceBetweenTexts.y 
                        + sizeTextHits.y;

		posQtyWrong.x = posTextWrong.x + (sizeTextWrong.x - sizeQtyWrong.x) / 2;
		posQtyWrong.y = posBackgroundPer.y + 2 * spaceBetweenTexts.y 
                        + sizeTextWrong.y;

        /* Help Box */
        // Set Size
        sizeHelpBox.x = Screen.width * FM_HELP.x;
        sizeHelpBox.y = Screen.height * FM_HELP.y;

        // Set Positions
        posHelpBox.x = ( Screen.width - sizeHelpBox.x ) / 2;
        posHelpBox.y = ( Screen.height - sizeHelpBox.y ) / 2;
         
		/* Button Box Help */
		// Set size
		sizeButtonBoxHelp.x = Screen.width * FM_BUTTON_BOX_HELP.x;
		sizeButtonBoxHelp.y = Screen.height * FM_BUTTON_BOX_HELP.y;
		
		// Font Size
		sizeFontButtonBoxHelp = (int) (sizeButtonBoxHelp.y / 1.5);

		// Set positions
		posButtonBoxHelp.x = posHelpBox.x 
							+ (sizeHelpBox.x - sizeButtonBoxHelp.x)/2;
		posButtonBoxHelp.y = posHelpBox.y + sizeHelpBox.y
							- sizeButtonBoxHelp.y
							- (30 * Screen.height / calcPixelScreenHeight);   
		/* Button Sound */
		// Set size
		sizeButtonSound.x = Screen.width * FM_BUTTON_SOUND.x;
		sizeButtonSound.y = Screen.height * FM_BUTTON_SOUND.y;

		// Set Positions
		posButtonSound.x = Screen.width - sizeButtonSound.x
							- (20 * Screen.width / calcPixelScreenWidht);    
		posButtonSound.y = (23 * Screen.height / calcPixelScreenHeight);    


	}

	/**
	*  @desc OnGUI is called for rendering and handling GUI events
	*  		 This means that OnGUI implementation might be called
	*  		 several times per frame (one call per event).
	*  		 Furthemore the method set and reset the matrix that
	*  		 scale GUIs according to resolution.
    */	
	void OnGUI () {
		setMatrix ();
		
		createEnvironment();
		
		resetMatrix ();
	}

	 /**
 	 * @desc Scale GUIs According to Resolution
	 *       All gui elements drawn after this
	 *       will be scaled 
 	 */
	private void setMatrix(){
		Vector2 ratio = new Vector2(Screen.width/originalWidth, 
                                    Screen.height/originalHeight );
		Matrix4x4 guiMatrix = Matrix4x4.identity;
		guiMatrix.SetTRS(new Vector3(1, 1, 1), Quaternion.identity, 
                         new Vector3(ratio.x, ratio.y, 1));
		GUI.matrix = guiMatrix;
	}

	 /**
	 * @desc Restores the default gui scale
	 *		 All gui elements drawn after this
	 *		 will not be scaled
	 */
	private void resetMatrix(){
	
		GUI.matrix = Matrix4x4.identity;
	}

	/**
	* @desc Creates members of the operation that will be
  	*		shown on the blackboard.
	*		One member is created randomly, the other follows
	*		the level 	
	*/
	private void createOperation () {
			
		op1 = rndNumb.Next (0, 10);
		op2 = level;

		updateExpressionTexts ();
	}

	/**
	* @desc 
	*/
	private void updateExpressionTexts () {
		operationText = op1 + " " + operatorType + " " + op2 + " = ";
		resultText = op1 + " " + operatorType + " " + op2 + " = ";
	}		
	
	private void evaluateOperation() {
		string resultUser = resultText.Replace (operationText, "");
		
		if(!resultUser.Equals("")){
			qtyPlayedChallenge++;
			updateTextChlMet();

			// Check operation
			double resultOperation = Evaluate (operationText.Replace("=", ""));

			if (resultOperation == double.Parse (resultUser)){
				qtyHits++;
				callSound (correctAnswerSound);
				currentStateInst = 1;
				StartCoroutine(GuiDisplayTimer(1.0f, "showBalloonHit"));
			}else {
				qtyWrong++;
				callSound (wrongAnswerSound);
				currentStateInst = 2;
				StartCoroutine(GuiDisplayTimer(1.0f, "showBalloonWrong"));
			}
			
			// Check current Eugenio State
			if (qtyHits == qtyWrong)
				currentStateTotal = 0;
			else if (qtyHits > qtyWrong)
				currentStateTotal = 1;
			else
				currentStateTotal = 2;

			// Check end of task
			if (qtyPlayedChallenge == qtyLimitChallenge)
				endGame();

			// Create new operation
			createOperation ();
		}
	}
	
	private void updateTextChlMet(){
		chlMetText = qtyPlayedChallenge + " / " + qtyLimitChallenge; 
	}
	
	private void endGame (){
		Application.LoadLevel("EndOfTask");
	}

	private void createTextField () {
		GUI.SetNextControlName("ResultTextField");
		
		if (Event.current.type == EventType.KeyDown) {
			callSound (typeWriterSound);
		}

		if (Event.current.type == EventType.KeyDown && 
            Event.current.keyCode == KeyCode.Return) {		 
			
            evaluateOperation();
		}else if (!(Event.current.type == EventType.KeyDown && 
                    Event.current.keyCode == KeyCode.Backspace && 
                    resultText.Equals(operationText))){

			drawTextField();
		}

		GUI.FocusControl("ResultTextField");
	}

	private void drawTextField() {
		myStyle = new GUIStyle (GUI.skin.textField);
		myStyle.fontSize = sizeFontExp;
		myStyle.font = (Font) Resources.Load(nameFontExp);
		myStyle.normal.textColor = Color.white;
		myStyle.alignment = TextAnchor.MiddleCenter;
		myStyle.normal.background = null;
		myStyle.hover.background = null;
		myStyle.focused.background = null;
		
		resultText = GUI.TextField (new Rect(posResultText.x, posResultText.y, 
                                    sizeResultText.x, sizeResultText.y), 
                                    resultText, 13, myStyle);
		
		// Regex that accepts only numbers and operators
		resultText = Regex.Replace(resultText, 
                                    @"[^ 0-9 | \+ | \- | \* | \/ | \= ]", "");
		
		// Moving the keyboard cursor position to the end of a TextField
		TextEditor editor = (TextEditor)GUIUtility.GetStateObject(
                                                    typeof(TextEditor), 
                                                    GUIUtility.keyboardControl);
		editor.selectPos = resultText.Length + 1;
		editor.pos = resultText.Length + 1;
	}

	private Vector2 calcSizeText (string text, string nameFont, int sizeFont) {
		GUIStyle style = new GUIStyle();
		style.font = (Font) Resources.Load(nameFont);
		style.fontSize = sizeFont;
		return style.CalcSize(new GUIContent(text));
	}

	private void callSound (AudioClip sound) {
		if(soundIsOn)
			audio.PlayOneShot(sound);
	}

	private void createButton (GUIStyle style, Vector2 pos, 
								Vector2 size, string name){
		if (GUI.Button (new Rect (pos.x, pos.y, size.x, size.y), "", style)) {
			callSound (buttonClickSound);
			decidesCallFunction (name);
		}
	}
	
	private void createButton (GUIStyle style, Vector2 pos, Vector2 size, 
								int fontSize, string name, string message){

		style.fontSize = fontSize;
		style.font = (Font) Resources.Load(nameFontInf);
		style.normal.textColor = Color.white;
		style.hover.textColor = Color.white;
		style.alignment = TextAnchor.MiddleCenter;	

		if (GUI.Button (new Rect (pos.x, pos.y, size.x, size.y),message,style)) {
			callSound (buttonClickSound);
			hideHelpMsg();
		}	
	}

	private void decidesCallFunction (string nameButton) {
		switch (nameButton) {	
			case "help":
				showHelpMsg();
				break;
			case "check":
				evaluateOperation ();
				break;	
			case "restart":
				restartGame ();
				break;
			case "exit":
				Application.LoadLevel("MainMenu");
				break;
			case "buttonBoxHelp":
				hideHelpMsg();
				break;
			case "sound":
				buttonSoundClicked ();
				break;
			default:
				break;
		}
	}

	private void buttonSoundClicked () {
		if (soundIsOn){
			soundIsOn = false;
			audio.Stop();
		}else{
			soundIsOn = true;	
			audio.Play();
		}
	}
	
	IEnumerator GuiDisplayTimer(float seconds, string showBalloon) {
        if(showBalloon.Equals("showBalloonHit"))
            showBalloonHit = true;
        else
            showBalloonWrong = true;

        // Waits an amount of time
        yield return new WaitForSeconds(seconds);
        
        if(showBalloon.Equals("showBalloonHit"))
            showBalloonHit = false;
        else
            showBalloonWrong = false;
    }
    
	private void showHelpMsg () {
		showBoxHelp = true;	
	}
	
	private void hideHelpMsg () {
		showBoxHelp = false;
	}

	private void restartGame () {
		qtyHits = 0;
		qtyWrong = 0;
	}

	private double Evaluate(string expression) {  
		return (double)new System.Xml.XPath.XPathDocument  
			(new System.IO.StringReader("<r/>")).CreateNavigator().Evaluate  
				(string.Format("number({0})", new  
				               System.Text.RegularExpressions.Regex(
                               @"([\+\-\*])").Replace(expression, " ${1} ")
                               .Replace("/", " div ").Replace("%", " mod ")));  
	}

	private void createTexture (Texture texture, Vector2 pos, Vector2 size) {
		GUI.DrawTexture (new Rect(pos.x, pos.y, size.x, size.y), texture);
	}

	private void printLabel (Vector2 pos, Vector2 size, string text, 
                             int fontSize, string fontName, Color color) {
		GUIStyle customFont = new GUIStyle();
		customFont.font = (Font) Resources.Load(fontName);
		customFont.fontSize = fontSize;
		customFont.normal.textColor = color;

		GUI.Label (new Rect (pos.x, pos.y, size.x, size.y), text, customFont);
	}

	private void createEnvironment() {
		createTexture (backgroundTexture, posBackground, sizeBackground);
		createTexture (mainTitleTexture, posMainTitle, sizeMainTitle);
		createTexture (performanceTexture, posBackgroundPer, sizeBackgroundPer);
		createTexture (blackboardTexture, posBlackBoard, sizeBlackBoard);
		
		if(!(showBalloonHit || showBalloonWrong))
			createTexture (eugenioTexture[currentStateTotal], 
							posEugenio, sizeEugenio);

		if(showBoxHelp == true){
			// Make a background box
			GUI.Box(new Rect(posHelpBox.x,posHelpBox.y,sizeHelpBox.x,
							 sizeHelpBox.y), helpTexture );

			createButton (buttonBoxHelpStyle, posButtonBoxHelp, 
						  sizeButtonBoxHelp, sizeFontButtonBoxHelp,
						  "buttonBoxHelp","Ok!");	
		}else{

			createButton (buttonHelpStyle, posButtonHelp, 
						  sizeButtonHelp, "help");
			createButton (buttonRestartStyle, posButtonRestart, 
					sizeButtonRestart, "restart");
			createButton (buttonExitStyle, posButtonExit, 
						  sizeButtonExit, "exit");
			createButton (buttonCheckStyle, posCheckButton, 
					sizeCheckButton, "check");
			
			if (soundIsOn)
				createButton (buttonSoundOn, posButtonSound, 
							  sizeButtonSound, "sound");	
			else 
				createButton (buttonSoundOff, posButtonSound, 
							  sizeButtonSound, "sound");
			
			// Print Texts Inf
			printLabel (posTextLevel, sizeTextLevel, levelText, 
					sizeFontLevel, nameFontInf, Color.white);
		   	printLabel (posTextChlMet, sizeTextChlMet, chlMetText,
					sizeFontLevel, nameFontInf, Color.white);	
			printLabel (posTextHits, sizeTextHits, hitsText, sizeFontScoreText, 
					nameFontInf, new Color32(0, 168, 89, 255));
			printLabel (posTextWrong, sizeTextWrong, wrongText, 
					sizeFontScoreText, nameFontInf, Color.red);
			printLabel (posQtyHits, sizeQtyHits, qtyHits.ToString(), 
					sizeFontScoreValue, nameFontInf, 
					new Color32(0, 168, 89, 255));
			printLabel (posQtyWrong, sizeQtyWrong, qtyWrong.ToString(), 
					sizeFontScoreValue, nameFontInf, Color.red);

		}

		if(showBalloonHit){
			createTexture (eugenioTexture[currentStateInst], 
						   posEugenio, sizeEugenio);
			createTexture (balloonTexture, posBalloon, sizeBalloon);
			printLabel (posTextBalloon[0], sizeTextBalloon[0], balloonText[0], 
					sizeFontTextBalloon, nameFontExp, 
					new Color32(0, 168, 89, 255));
		}

		if(showBalloonWrong){
			createTexture (eugenioTexture[currentStateInst], 
						   posEugenio, sizeEugenio);
			createTexture (balloonTexture, posBalloon, sizeBalloon);
			printLabel (posTextBalloon[1], sizeTextBalloon[1], balloonText[1], 
					sizeFontTextBalloon, nameFontExp, 
					new Color32(0, 168, 89, 255));
		}        

		if (!showBoxHelp && !showBalloonHit && !showBalloonWrong){
			createTextField();
		}
		
	}
}
