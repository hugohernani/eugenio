using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

public class ListFood : ListAbstract {

	bool chosen;
	int foodValue;

	RectTransform containerFoodPrefab;
	RectTransform containerFood;

	RectTransform mainContainer;

	RectTransform itemPreFabN;
	string resourceFoodImages;


	void Awake(){
		itemPreFabN = Resources.Load<RectTransform> (PREFAB_RESOURCES + "itemToBuy");
		resourceFoodImages = ITEM_RESOURCES + "itemsFood/";
		containerFoodPrefab = Resources.Load<RectTransform> (PREFAB_RESOURCES + "ChosenItem");
	}

	protected override List<Item> populateList ()
	{
		User user = User.getInstance;
		List<Dictionary<string,string>> dItems = user.AvailableFoods;

		List<Item> items = new List<Item> ();

		foreach (Dictionary<string,string> dItem in dItems) {
			Item item = new ItemFood(
				dItem["name"],
				Resources.Load<Sprite>(resourceFoodImages + dItem["name"]),
				int.Parse(dItem["value"])
				);
			items.Add(item);
		}

		return items;

	}

	protected override float populateContainer (List<Item> items, RectTransform container)
	{

		float heightItem = 100f;
		float posAcumulator = 0f;
		for (int i = 0; i < items.Count; i++) {
			RectTransform newItem = Instantiate (itemPreFabN, container.transform.position, Quaternion.identity) as RectTransform;

			int newInt = i;

			ItemFood itemFood = items[newInt] as ItemFood;

			UnityAction clickAction = () => {clickFood(itemFood);};

			newItem.FindChild("Icon").GetComponent<Image>().sprite = itemFood.Icon;
			newItem.FindChild("Description").GetComponent<Text>().text = itemFood.Name;
			Transform starIcon = newItem.FindChild("StarIcon");
			starIcon.GetComponentInChildren<Text>().text = itemFood.Value.ToString();
			newItem.GetComponent<Button>().onClick.AddListener(clickAction);
			
			newItem.parent = container.transform;
			
			newItem.offsetMax = Vector2.zero;
			newItem.offsetMin = Vector2.zero;
			
			newItem.offsetMin = new Vector2(newItem.offsetMin.x, -posAcumulator);
			newItem.offsetMax = new Vector2(newItem.offsetMax.x, -posAcumulator);

			posAcumulator += heightItem;
			
		}

		return posAcumulator;
	}

	void clickFood(ItemFood food){

		clean ();

		mainContainer = GameObject.Find ("CanvasList").GetComponent<RectTransform> ();

		containerFood = Instantiate (containerFoodPrefab, mainContainer.transform.position, Quaternion.identity) as RectTransform;
		containerFood.FindChild ("ItemName").GetComponent<Text> ().text = food.Name;
		containerFood.FindChild ("ItemIcon").GetComponent<Image> ().sprite = food.Icon;
		containerFood.FindChild ("ImageValue").GetComponentInChildren<Text> ().text = food.Value.ToString ();

		UnityAction clickConfirm = () => {FoodBought(containerFood.gameObject, true, food);};
		UnityAction clickCancel = () => {FoodBought(containerFood.gameObject, false, food);};

		containerFood.FindChild("ConfirmButton").GetComponent<Button>().onClick.AddListener(clickConfirm);
		containerFood.FindChild("CancelButton").GetComponent<Button>().onClick.AddListener(clickCancel);

		containerFood.parent = mainContainer.transform;
		containerFood.offsetMin = Vector2.zero;
		containerFood.offsetMax = Vector2.zero;

	}

	void FoodBought(GameObject containerGO, bool bought, ItemFood food){
		this.chosen = true;
		if(bought){
			this.foodValue = food.Value;
		}else
			this.foodValue = -1;

		Destroy (containerGO);
		Destroy (this);

	}

	public IEnumerator getResult(Action<int> result){
		while(!this.chosen){
			yield return new WaitForSeconds(1f);
		}
		result (foodValue);
		yield break;
	}

}
