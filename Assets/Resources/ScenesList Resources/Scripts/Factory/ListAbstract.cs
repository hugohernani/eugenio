using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class ListAbstract: MonoBehaviour{

	List<Item> items;
	protected readonly string PREFAB_RESOURCES = "ScenesList Resources/preFab/";
	protected readonly string ITEM_RESOURCES = "ScenesList Resources/textures/";

	RectTransform scenesContainerRect;
	//	protected RectTransform itemPrefab;

	public static bool SHOWING = false;

	public void clean(){

		removeAllFromContainer ();
		
	}
	
	void removeAllFromContainer(){
		foreach (Transform child in scenesContainerRect.transform) {
			Destroy(child.gameObject);
		}
	}
	
	public int populate(bool repopulate = false){

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
		return 	finishClick ();
	}
	
	protected abstract List<Item> populateList ();
	
	protected abstract float populateContainer(List<Item> items, RectTransform container);
	
	protected abstract int finishClick (int value = -1);
	
	protected void adjustBoxSize(float posMultiplied){
		scenesContainerRect.offsetMin = new Vector2 (scenesContainerRect.offsetMin.x, -posMultiplied);
	}

	protected void justHide(){
		ListAbstract.SHOWING = false;
	}

	protected void hide(){
		ListAbstract.SHOWING = false;

		Destroy (this);
	}

	public int finish ()
	{
		return finishClick();
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
