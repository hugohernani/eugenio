using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
	
	/* Variables */	
	private readonly float TIMEDECAY = 60;

	private readonly float MR = 0.9f;
	private readonly float R = 0.75f;
	private readonly float M = 0.5f;
	private readonly float L = 0.25f;
	private readonly float ML = 0.09f;

	private float qtyFood;
	private float qtyHealth;
	private float qtyEnt;

	private float decayRate = 0;

	private float beforeTime = 0;
	private float currentTime = 0;
	int i = 0;

	/* Mi's  */	
	private float[] uFood;
	private float[] uHealth;
	private float[] uEnt;
	
	/* R's */
	private float[] r;
	
	/* Y's */
	private float[] y;

	/* Decay List */
	private List<float> decayList;

	Pet petStatus;
	User user;

	EugenioSceneControl eugenioSceneControl;

	public static MainMenu instance;

	void Awake () {
		if(instance == null){
			instance = this;

			DontDestroyOnLoad(gameObject);
		}else{
			Destroy(gameObject);
		}

		GameObject eugenioSceneGO = GameObject.FindGameObjectWithTag("EugenioSceneControl");
		if(eugenioSceneGO != null){
			instance.eugenioSceneControl = eugenioSceneGO.GetComponent<EugenioSceneControl>();
		}

		user = User.getInstance;
		petStatus = user.CurrentPetStatus;

		uFood = new float [3];
		uHealth = new float [3];
		uEnt = new float [3];

		r = new float [27];
		y = new float [27];
		
		decayList = new List<float>();
	}

	void Start(){

		qtyEnt = petStatus.Entertainment;
		qtyHealth = petStatus.Health;
		qtyFood = petStatus.Feed;

		instance.updateSliderValues();

		InvokeRepeating ("calcDecayRate", 30f, 30f);
		InvokeRepeating ("applyDecay", 40f, 40f);
		InvokeRepeating ("updateSliderValues", 60f, 60f);

	}

	void FixedUpdate () {

//
//		if (qtyFood < 0.50 || qtyHealth < 0.5){
//			ThoughtController.playThinking ();
//		}else {
//			ThoughtController.playNoThinking ();
//		}
//
//		if (qtyFood < 0.50) {
//			ThinkingFoodController.playThinkingFood ();	
//		}else {
//			ThinkingFoodController.playNoThinkingFood ();
//		}
//
//		if (qtyHealth < 0.50) {
//			ThinkingPlayController.playThinkingPlay ();	
//		}else {
//			ThinkingPlayController.playNoThinkingPlay ();
//		}
	}
	
	void updateSliderValues () {
		if(eugenioSceneControl != null){
			eugenioSceneControl.updateStatusSliders(qtyHealth, qtyEnt, qtyFood);
			eugenioSceneControl.updateExperienceSlider(user.CalculateExperience());
		}
	}
	
	void calcFuzzyFicacao () {
		
		// uFood
		if (0 <= qtyFood && qtyFood < 0.25) {
			uFood[0] = 1;
			uFood[1] = 0;
			uFood[2] = 0;	
		}else if (0.25 <= qtyFood && qtyFood < 0.4) {
			uFood[0] = 0.5f;
			uFood[1] = 0.5f;
			uFood[2] = 0;
		}else if (0.4 <= qtyFood && qtyFood < 0.6) {
			uFood[0] = 0;
			uFood[1] = 1;
			uFood[2] = 0;
		}else if (0.6 <= qtyFood && qtyFood < 0.75) {
			uFood[0] = 0;
			uFood[1] = 0.5f;
			uFood[2] = 0.5f;
		}else if (0.75 <= qtyFood && qtyFood < 1) {
			uFood[0] = 0;
			uFood[1] = 0;
			uFood[2] = 1;
		}
		
		// uHealth
		if (0 <= qtyHealth && qtyHealth < 0.25) {
			uHealth[0] = 1;
			uHealth[1] = 0;
			uHealth[2] = 0;	
		}else if (0.25 <= qtyHealth && qtyHealth < 0.4) {
			uHealth[0] = 0.5f;
			uHealth[1] = 0.5f;
			uHealth[2] = 0;
		}else if (0.4 <= qtyHealth && qtyHealth < 0.6) {
			uHealth[0] = 0;
			uHealth[1] = 1;
			uHealth[2] = 0;
		}else if (0.6 <= qtyHealth && qtyHealth < 0.75) {
			uHealth[0] = 0;
			uHealth[1] = 0.5f;
			uHealth[2] = 0.5f;
		}else if (0.75 <= qtyHealth && qtyHealth < 1) {
			uHealth[0] = 0;
			uHealth[1] = 0;
			uHealth[2] = 1;
		}
		
		// uEnt
		if (0 <= qtyEnt && qtyEnt < 0.25) {
			uEnt[0] = 1;
			uEnt[1] = 0;
			uEnt[2] = 0;	
		}else if (0.25 <= qtyEnt && qtyEnt < 0.4) {
			uEnt[0] = 0.5f;
			uEnt[1] = 0.5f;
			uEnt[2] = 0;
		}else if (0.4 <= qtyEnt && qtyEnt < 0.6) {
			uEnt[0] = 0;
			uEnt[1] = 1;
			uEnt[2] = 0;
		}else if (0.6 <= qtyEnt && qtyEnt < 0.75) {
			uEnt[0] = 0;
			uEnt[1] = 0.5f;
			uEnt[2] = 0.5f;
		}else if (0.75 <= qtyEnt && qtyEnt < 1) {
			uEnt[0] = 0;
			uEnt[1] = 0;
			uEnt[2] = 1;
		}
	}

	void calcInference () {

		for ( int i = 0; i < 3; i++ ){
			for ( int j = 0; j < 3; j++ ){
				for ( int k = 0; k < 3; k++ ){
					r[i*9 + j*3 + k] = uFood[i] * uEnt[j] * uHealth[k];  
				}
			}
		}

		for ( int i = 0; i < 27; i++ ){
			y[i] = r[i] * decayList[i];
		}
	}

	void calcDefuzzyFicacao () {
		float sumY = 0;
		float sumR = 0;

		for ( int i = 0; i < 27; i++ ) {
			sumY = sumY + y[i];
			sumR = sumR + r[i];
		}		
		
		this.decayRate = sumY / sumR;
	}

	void calcDecayRate () {
		createDecayList();
		
		calcFuzzyFicacao();
		calcInference();
		calcDefuzzyFicacao ();
	}

	void createDecayList () {
		decayList.Add(MR);
		decayList.Add(MR);	
		decayList.Add(R);	
		decayList.Add(MR);
		decayList.Add(R);	
		decayList.Add(M);
		decayList.Add(R);	
		decayList.Add(M);	
		decayList.Add(L);
		decayList.Add(MR);		
		decayList.Add(M);
		decayList.Add(M);	
		decayList.Add(M);	
		decayList.Add(L);
		decayList.Add(L);		
		decayList.Add(M);
		decayList.Add(L);	
		decayList.Add(ML);	
		decayList.Add(R);
		decayList.Add(M);	
		decayList.Add(M);
		decayList.Add(M);	
		decayList.Add(L);	
		decayList.Add(ML);
		decayList.Add(ML);	
		decayList.Add(ML);	
		decayList.Add(ML);	
	}

	void applyDecay () {
		Debug.Log ("DecayRate: " + decayRate);

		if(!float.IsNaN(decayRate)){
			qtyFood -= decayRate;
			qtyEnt -= decayRate;
			qtyHealth -= decayRate;
			
			petStatus.Feed = qtyFood;
			petStatus.Entertainment = qtyEnt;
			petStatus.Health = qtyHealth;
			
			user.CurrentPetStatus = petStatus;
		}

		Debug.Log (user.CurrentPetStatus.ToString ());
	}

}