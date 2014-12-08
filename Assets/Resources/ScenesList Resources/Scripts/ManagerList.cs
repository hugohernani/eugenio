using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ManagerList : MonoBehaviour {

	public RectTransform RectTransformScrollView;

	ListFactory factory;

	bool error = false;

	Vector2 minLimit;
	Vector2 maxLimit;

	string category = "";

	ListAbstract list;

	public enum TypeList: int{
		Tasks = 1,
		Foods = 2,
		Games = 3,
	}

	void Start(){

		minLimit = RectTransformScrollView.offsetMin;
		maxLimit = RectTransformScrollView.offsetMax;

	}

	public IEnumerator FoodsList(Func<int, int> method){
		showList (TypeList.Foods);
		yield return StartCoroutine (bringContainer());
		
		yield return StartCoroutine(GetComponent<ListFood>().getResult((result) => {
			if(result != -1){
				method(result);
			}
		}));
		yield return StartCoroutine (hideContainer ());
	}

	public void TasksList(){
		StartCoroutine (bringContainer());
		showList (TypeList.Tasks);
	}

	public void GamesList(){
		StartCoroutine (bringContainer());
		showList (TypeList.Games);
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
		factory = gameObject.AddComponent<ListFactory> ();
		
		list = (ListAbstract) factory.showList (category, gameObject);
		
		list.populate ();
	}

	IEnumerator bringContainer(){
		while(RectTransformScrollView.offsetMin.y >= 1){
			RectTransformScrollView.offsetMax = Vector2.Lerp(RectTransformScrollView.offsetMax, Vector2.zero, Time.deltaTime * 2.2f);
			RectTransformScrollView.offsetMin = Vector2.Lerp(RectTransformScrollView.offsetMin, Vector2.zero, Time.deltaTime * 2.2f);
			yield return true;
		}
		RectTransformScrollView.offsetMax = Vector2.zero;
		RectTransformScrollView.offsetMin = Vector2.zero;
		yield break;
	}

	IEnumerator hideContainer(){
		while(RectTransformScrollView.offsetMin.y <= minLimit.y - 50){
			RectTransformScrollView.offsetMax = Vector2.Lerp(RectTransformScrollView.offsetMax, maxLimit, Time.deltaTime * 2.2f);
			RectTransformScrollView.offsetMin = Vector2.Lerp(RectTransformScrollView.offsetMin, minLimit, Time.deltaTime * 2.2f);
			yield return true;
		}
		RectTransformScrollView.offsetMax = maxLimit;
		RectTransformScrollView.offsetMin = minLimit;

		Destroy (factory);
		yield break;
	}

}
