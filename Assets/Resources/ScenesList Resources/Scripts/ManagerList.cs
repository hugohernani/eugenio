using UnityEngine;
using System.Collections.Generic;

public class ManagerList : MonoBehaviour {

	public RectTransform RectTransformScrollView;

	ListFactory factory;

	int result;
	bool error = false;

	Vector2 minLimit;
	Vector2 maxLimit;

	string category = "";

	public enum TypeList: int{
		Tasks = 1,
		Foods = 2,
		Games = 3,
	}

	void Start(){

		minLimit = RectTransformScrollView.offsetMin;
		maxLimit = RectTransformScrollView.offsetMax;

	}

	public void FoodsList(){
		showList (TypeList.Foods);
	}

	public void TasksList(){
		showList (TypeList.Tasks);
	}

	public void GamesList(){
		showList (TypeList.Games);
	}

	void showList(string category){

		this.category = category;
		factory = gameObject.AddComponent<ListFactory> ();
		ListAbstract.SHOWING = true;
		
		ListAbstract list = null;
		try {
			list = (ListAbstract) factory.showList (category, gameObject);
			list.populate ();
		} catch (ExitGUIException) {
			ListAbstract.SHOWING = false;
			error = true;
			throw new System.Exception("Tipo de lista invalido. Tente 'task' ou 'food'\n" +
			                           "Ou use alguns dos inteiros em ManagerList.TypeList.");
		}
	}

	void showList(TypeList typeList){

		category = "";
		switch ((int) typeList) {
		case (int) TypeList.Foods:
			category = "food";
			break;
		case (int) TypeList.Tasks:
			category = "task";
			break;
		case (int) TypeList.Games:
			category = "Games";
			break;
		}
		ListAbstract.SHOWING = true;
		factory = gameObject.AddComponent<ListFactory> ();
		
		ListAbstract list = (ListAbstract) factory.showList (category, gameObject);
		
		list.populate ();
	}


	public int catchResult(){
		if(error)
			throw new System.Exception("There was a error. Try to recreate the list");
		else
			return result;
	}

	void Update(){
		if(ListAbstract.SHOWING){
			bringContainer();
		}else{
			hideContainer();
			if(ListFood.Bought){
				ListFood.Bought = false;
				result = ListFood.food.Value;
				MainMenu.updatePoints(result);
			}else{
				result = -1;
			}
		}
	}

	void bringContainer(){
		if (RectTransformScrollView.offsetMin.y >= 1){
			RectTransformScrollView.offsetMax = Vector2.Lerp(RectTransformScrollView.offsetMax, Vector2.zero, Time.deltaTime * 2.2f);
			RectTransformScrollView.offsetMin = Vector2.Lerp(RectTransformScrollView.offsetMin, Vector2.zero, Time.deltaTime * 2.2f);
		}else{
			RectTransformScrollView.offsetMax = Vector2.zero;
			RectTransformScrollView.offsetMin = Vector2.zero;
		}
	}

	void hideContainer(){
		if (RectTransformScrollView.offsetMin.y <= minLimit.y - 50){
			RectTransformScrollView.offsetMax = Vector2.Lerp(RectTransformScrollView.offsetMax, maxLimit, Time.deltaTime * 2.2f);
			RectTransformScrollView.offsetMin = Vector2.Lerp(RectTransformScrollView.offsetMin, minLimit, Time.deltaTime * 2.2f);
		}else{
			RectTransformScrollView.offsetMax = maxLimit;
			RectTransformScrollView.offsetMin = minLimit;
		}

		Destroy (factory);
//		Destroy (ListFactory);

	}

}
