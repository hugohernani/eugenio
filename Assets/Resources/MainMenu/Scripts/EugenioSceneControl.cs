using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class EugenioSceneControl : MonoBehaviour {

	public Slider foodSlider;
	public Slider healthSlider;
	public Slider entSlider;
	public Slider xpSlider;

	EugenioController eugenioController;
	Thoughts thoughtsController;
	Animator baloonAnimator;

	string thinkingAnimationName;

	void Awake(){

		baloonAnimator = GameObject.Find ("Baloon").GetComponent<Animator> ();

		StarTextController starTextController = StarTextController.instance;
		StarImageController starImageController = StarImageController.instance;

		eugenioController = GetComponent<EugenioController>();
		thoughtsController = GetComponentInChildren<Thoughts> ();

		User user = User.getInstance;
		MainCategory userCurrentCategory = ((MainCategory)user.CurrentCategory);
		
		if (userCurrentCategory != null) {
			int diffLevel = userCurrentCategory.Level - user.Level_pet;

			if(diffLevel > 0){

				while(diffLevel > 0){
					user.Level_pet++;
					eugenioController.upgradeUser();
					diffLevel--;
				}

				user.CurrentStage = 1;
				
				GameObject applicationManager = GameObject.FindGameObjectWithTag ("ApplicationManager");
				DataAccess dataAccess = applicationManager.GetComponent<DataAccess> ();
				dataAccess.getAvailableFoods(user.Level_pet);
				dataAccess.getAvailableGames(user.Level_pet);
				dataAccess.trySaveUserInformationsOnDB ();

			}
		}

		starTextController.updateStarValue (user.Stars_qty);
	}

	void Start(){
		InvokeRepeating ("Thinking", 60f, 60f);
	}

	void Thinking(){
		if(foodSlider.value < 0.5){
			thinkingAnimationName = "thinkingFood";
			InvokeRepeating("thinkingAnimation", 0f,120f);
		}else if(healthSlider.value < 0.5){
			thinkingAnimationName = "thinkingHealth";
			InvokeRepeating("thinkingAnimation", 0f, 120f);
		}else if(entSlider.value < 0.5){
			thinkingAnimationName = "thinkingEntertainment";
			InvokeRepeating("thinkingAnimation", 0f, 120f);
		}else{
			thinkingAnimationName = "";
			if(IsInvoking("thinkingAnimation")){
				CancelInvoke("thinkingAnimation");
			}
		}
	}

	void thinkingAnimation(){
		if(thinkingAnimationName != ""){
			thoughtsController.playCustomAnimationTrigger(baloonAnimator, "thinking");
			thoughtsController.playCustomAnimationTrigger(thinkingAnimationName);
		}
	}

	public void foodClick(ManagerList managerList){
		StartCoroutine
		(
				managerList.FoodsList (FoodBought)
		);

	}

	int FoodBought(int value){
		StarTextController instanceStarTextController = StarTextController.instance;
		StarImageController starImageController = StarImageController.instance;
		
		int result = instanceStarTextController.changeStarText(value);
		
		if(result >= 0){
			eugenioController.playEathing(3f);
			starImageController.playStarImage();
			User user = User.getInstance;
			user.Stars_qty = result;

			UpdateFood();
		}else{
//			eugenioController.playNegation(); // TODO
		}

		return result;
	}

	void UpdateFood(){
		// TODO David
	}

	public void MedicineTaken(){
		// TODO Logic Medicine
	}

	public void Bathed(){
		// TODO Logic after Bathing
	}

	public void updateStatusSliders (float qtyHealth, float qtyEnt, float qtyFood)
	{
		healthSlider.value = qtyHealth;
		entSlider.value = qtyEnt;
		foodSlider.value = qtyFood;
	}

	public void updateExperienceSlider(float expValue){
		xpSlider.value = expValue;
	}

}
