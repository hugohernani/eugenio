using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EugenioSceneControl : MonoBehaviour {

	public Slider foodSlider;
	public Slider healthSlider;
	public Slider entSlider;
	public Slider xpSlider;

	void Awake(){

		User user = User.getInstance;
		MainCategory userCurrentCategory = ((MainCategory)user.CurrentCategory);
		
		if (userCurrentCategory != null) {
			int diffLevel = userCurrentCategory.Level - user.Level_pet;

			Debug.Log("DiffLevel: " + diffLevel.ToString());
			
			if(diffLevel > 0){
				EugenioController eugenioController = GetComponent<EugenioController>();

				while(diffLevel > 0){
					user.Level_pet++;
					eugenioController.upgradeUser();
					diffLevel--;
				}

				user.CurrentStage = 1;
				
				GameObject applicationManager = GameObject.FindGameObjectWithTag ("ApplicationManager");
				DataAccess dataAccess = applicationManager.GetComponent<DataAccess> ();
				dataAccess.trySaveUserInformationsOnDB ();

			}
			
		}else{
			Debug.Log("There is not userCurrentCategory");
		}
	}

	public static void updatePoints (int value) {
		StarTextController starTextController = StarTextController.instance;
		StarImageController starImageController = StarImageController.instance;	
		int result = starTextController.playStarText(value);
		if(result >= 0){
			starImageController.playStarImage();
		}
	}

	public void updateStatusSliders (float qtyHealth, float qtyEnt, float qtyFood)
	{
		healthSlider.value = qtyHealth;
		entSlider.value = qtyEnt;
		foodSlider.value = qtyFood;
	}

	public void updateExperienceSlider(int expValue){
		xpSlider.value = expValue;
	}


}
