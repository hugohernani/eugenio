using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
//using System.Data;

/**
* This class manages the display of information
* 
* @author David Cesar and Hugo
*/

public class EstudarScript : MonoBehaviour {
	/* Declaring variables */	
	
	/* Private properties */

	UserInteraction userInteraction;
	MessageInteraction msgInteraction;
	AudioManager audioManager;
	User user;
	MainCategory mainCategory;
	MathSubCategory mathCategory;

	readonly string[] operators = {"+","-"};
	string operationType;
	List<int> numbersUsed;
	int attempt = 0;
	int firstNumber = 0;
	int secondNumber = 0;

	/* Publics properties */
	public Text firstArgument;
	public Text mathOperator;
	public Text secondArgument;
	public InputField InputResult;

	/* AudioClip properties */
//	public AudioClip wrongAnswerSound;
//	public AudioClip correctAnswerSound;
	public AudioClip typeWriterSound;
//	public AudioClip buttonClickSound;
	public Animator animator;

	void Awake(){
		GameObject manager = GameObject.Find ("Manager");
		userInteraction = manager.GetComponent<UserInteraction> ();
		msgInteraction = manager.GetComponent<MessageInteraction> ();
		audioManager = manager.GetComponent<AudioManager> ();

		this.audioManager = manager.GetComponent<AudioManager> ();
	}

    /** 
	* Use this for initialization 
	*/
	void Start () {
		user = User.getInstance;
		mainCategory = (MainCategory)user.CurrentCategory;
		mathCategory = (MathSubCategory)user.CurrentSubCategory;
		numbersUsed = new List<int> ();

		// Initializing variables

		setOperationOptions ();
		createOperation ();
		animator.SetBool("Waiting", true);
	}

	void setOperationOptions(){
		int value = Mathf.Abs(mathCategory.Value);
		if(value != 0){
			firstNumber = value;
		}else{
			firstNumber = Random.Range(mainCategory.InitialValue, mainCategory.FinalValue+1);
		}

		mathOperator.text = mathCategory.Value < 0 ? "-" : "+";

	}



	void FixedUpdate(){
		if(Input.GetKeyDown(KeyCode.Return)){
			callSound(typeWriterSound);
			evaluateOperation();
		}else if(Input.GetKeyUp(KeyCode.Return)){
			EventSystem.current.SetSelectedGameObject(InputResult.gameObject, null);
			InputResult.OnPointerClick(new PointerEventData(EventSystem.current));
			animator.SetBool("Waiting", true);
		}
	}


	/**
	* @desc Creates members of the operation that will be
  	*		shown on the blackboard.
	*		One member is created randomly, the other follows
	*		the level 	
	*/
	void createOperation () {
		animator.SetBool ("Waiting", false);
		attempt++;
		InputResult.text = "";

		int secondNumber = 0;
		do{
			secondNumber = Random.Range(mainCategory.InitialValue,mainCategory.FinalValue+1);
		}while(numbersUsed.Contains(secondNumber) && attempt != 10);
		numbersUsed.Add (secondNumber);

		if(firstNumber < secondNumber){
			int temp = secondNumber;
			firstNumber = secondNumber;
			secondNumber = temp;
		}

		firstArgument.text = firstNumber.ToString ();
		secondArgument.text = secondNumber.ToString ();
		if(mathCategory.Integer){
			mathOperator.text = operators[Random.Range(0,2)];
		}
	}

	public void evaluateOperation() {
		int resultUser = -1;
		bool succeed = int.TryParse(InputResult.text, out resultUser);

		if(succeed && InputResult.text != ""){
			// Check operation
			if (resultUser == Evaluate()){
				userInteraction.hit ();
			}else {
				userInteraction.fail ();
			}
			// Create new operation
			createOperation ();
		}else{
			msgInteraction.showMesssage("Desculpe. Tente novamente.");
		}
	}

	void callSound (AudioClip sound) {
		audioManager.playOneClip (sound);
	}

	int Evaluate() {
		int firstNumber = int.Parse (firstArgument.text);
		int secondNumber = int.Parse (secondArgument.text);
		int result = -1;
		switch (mathOperator.text) {
		case "+":
			result = firstNumber + secondNumber;
			break;
		case "-":
			result = firstNumber - secondNumber;
			break;
		}
//		string expression = firstArgument.text + operationType + secondArgument.text;
//		int result = (int) new DataTable().Compute(expression, null);
		return result;
	}

}