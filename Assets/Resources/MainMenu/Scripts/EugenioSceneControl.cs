using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class EugenioSceneControl : MonoBehaviour {
	public Slider foodSlider;
	public Slider healthSlider;
	public Slider entSlider;
	public Slider xpSlider;

	User user;
	EugenioController eugenioController;
	Thoughts thoughtsController;
	Animator baloonAnimator;

	string thinkingAnimationName;

	const int max_increase_value = 20;
	List<int> foodsPrices;

	void Awake(){
		foodsPrices = new List<int> ();

		baloonAnimator = GameObject.Find ("Baloon").GetComponent<Animator> ();

		StarTextController starTextController = StarTextController.instance;
		StarImageController starImageController = StarImageController.instance;

		eugenioController = GetComponent<EugenioController>();
		thoughtsController = GetComponentInChildren<Thoughts> ();

		user = User.getInstance;
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
				dataAccess.getAvailableFoods();
				dataAccess.getAvailableGames();
				dataAccess.trySaveUserInformationsOnDB ();

			}
		}

		starTextController.updateStarValue (user.Stars_qty);
	}

	void Start(){
		foreach(Dictionary<string, string> dicFood in user.AvailableFoods){
			foodsPrices.Add(int.Parse(dicFood["value"]));
		}

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

	void updateStatus(int fixedValueIncrease, Slider sliderStatus){
	
		int level = user.Level_pet + 1; // to deal with level 0.
		float porcentage = fixedValueIncrease / level;
		float p_increase = max_increase_value * porcentage/100;
		float final_result = sliderStatus.value + p_increase;

		sliderStatus.value = final_result;
		updateStatusSliders ();
	}

	void updateStatus(int valueToSearch, ref List<int> listReference, Slider sliderStatus){
		int indice_from_1 = listReference.IndexOf (valueToSearch) + 1; // to deal with level 0
		int level = user.Level_pet + 1; // to deal with level 0.
		float porcentage = indice_from_1 / level;
		float value_food_slider_increased = max_increase_value * porcentage/100;
		float final_result = sliderStatus.value + value_food_slider_increased;
		
		sliderStatus.value = final_result;
		updateStatusSliders ();
	}

	bool starAnimation(int value, ref List<int> list, Slider sliderStatus,
	                  Func<float, bool> animationMethod){
		if(sliderStatus.maxValue == sliderStatus.value){
			eugenioController.playNegation();
			return false;
		}
		StarTextController instanceStarTextController = StarTextController.instance;
		StarImageController starImageController = StarImageController.instance;
		
		int result = instanceStarTextController.changeStarText(value);

		if(result > 0){
			animationMethod(3f);
			starImageController.playStarImage();
			user.Stars_qty = result;

			if(list.Count != 0){
				updateStatus(value, ref list, sliderStatus);
			}else{
				updateStatus(value, sliderStatus);
			}
			return true;
		}else{
			eugenioController.playNegation();
			return false;
		}

	}

	public void foodClick(ManagerList managerList){
		StartCoroutine
			(
				managerList.FoodsList (FoodBought)
			);
	}

	int FoodBought(int value){

		bool result = starAnimation (value, ref foodsPrices, foodSlider, eugenioController.playEathing);
		if(!result){
			return -1;
		}
		return 1;

	}

	public void MedicineTaken(){
		int value = Mathf.FloorToInt((user.Level_pet + 2) / 2); // TODO
		List<int> emptyList = new List<int>();

		bool success = starAnimation(value, ref emptyList, healthSlider, eugenioController.playTakingMedicine);

	}
	
	public void Bathed(){
		Debug.Log (Mathf.FloorToInt((user.Level_pet + 1) / 2));
		int value = Mathf.FloorToInt((user.Level_pet + 2) / 2); // TODO
		List<int> emptyList = new List<int>();

		bool success = starAnimation (value, ref emptyList, healthSlider, eugenioController.playBathing);
	}
	
	void updatePet(Pet pet){
		user.CurrentPetStatus = pet;
	}

	void updateStatusSliders(){
		Pet pet = user.CurrentPetStatus;
		pet.Health = healthSlider.value;
		pet.Entertainment = entSlider.value;
		pet.Feed = foodSlider.value;

		updatePet (pet);
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
