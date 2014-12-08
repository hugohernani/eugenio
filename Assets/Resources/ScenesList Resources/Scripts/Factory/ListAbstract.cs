using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public abstract class ListAbstract: MonoBehaviour{

	List<Item> items;
	protected readonly string PREFAB_RESOURCES = "ScenesList Resources/preFab/";
	protected readonly string ITEM_RESOURCES = "ScenesList Resources/textures/";

	RectTransform scenesContainerRect;

	public void clean(){
		removeAllFromContainer ();
		
	}
	
	void removeAllFromContainer(){
		foreach (Transform child in scenesContainerRect.transform) {
			Destroy(child.gameObject);
		}
	}
	
	public void populate(bool repopulate = false){

		scenesContainerRect = (RectTransform)GameObject.Find ("ScenesContainer").GetComponent<RectTransform> ();

		if (repopulate){
			clean();
		}else{
			clean();
			items = populateList ();
		}

		int dividerContainer = 2;
		if(items.Count >= 3){
			scenesContainerRect.offsetMax = Vector2.zero;
			scenesContainerRect.offsetMin = Vector2.zero;
			scenesContainerRect.GetComponent<VerticalLayoutGroup>().enabled = true;
		}else{
			dividerContainer = 10;
			scenesContainerRect.offsetMax = Vector2.zero;
			scenesContainerRect.offsetMin = Vector2.zero;
			scenesContainerRect.GetComponent<VerticalLayoutGroup>().enabled = false;
		}
		float posAcumulator = populateContainer (items, scenesContainerRect);
		adjustBoxSize (posAcumulator/dividerContainer);
	}
	
	protected abstract List<Item> populateList ();
	
	protected abstract float populateContainer(List<Item> items, RectTransform container);

	protected void adjustBoxSize(float posMultiplied){
		scenesContainerRect.offsetMin = new Vector2 (scenesContainerRect.offsetMin.x, -posMultiplied);
	}

	protected void hide(){
		Destroy (this);
	}

	public RectTransform getContainerItems(){
		return scenesContainerRect;
	}

	public List<Item> Items {
		get {
			return this.items;
		}
		set {
			items = value;
		}
	}

}
