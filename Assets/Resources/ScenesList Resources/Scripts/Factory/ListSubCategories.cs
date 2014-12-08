using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;

public class ListSubCategories : ListAbstract {

	RectTransform itemPreFab;
	
	void Awake(){
		itemPreFab = Resources.Load<RectTransform> (PREFAB_RESOURCES + "itemText");
	}
	
	protected override System.Collections.Generic.List<Item> populateList ()
	{
		// Nothing. Never is called in this kind of Abstract list
		return null;

	}
	
	protected override float populateContainer (List<Item> items, RectTransform container)
	{
		float heightItem = 100f;
		float posAcumulator = 0f;
		// change this according to the quantity of items or scene
		for (int i = 0; i < items.Count; i++) {
			RectTransform newItem = Instantiate (itemPreFab, container.transform.position, Quaternion.identity) as RectTransform;
			
			int newInt = i;
			
			ItemCategory itemTask = items[newInt] as ItemCategory;
			
			newItem.GetComponent<Text>().text = itemTask.Name; // text
			
			if(itemTask.Available){
				
				Destroy(newItem.GetComponentInChildren<Image>());

				UnityAction actionRepobulate = () => {callScene(itemTask);};
				newItem.GetComponent<Button>().onClick.AddListener(actionRepobulate);
			}
			
			newItem.parent = container.transform;
			
			newItem.offsetMax = Vector2.zero;
			newItem.offsetMin = Vector2.zero;
			
			newItem.offsetMin = new Vector2(newItem.offsetMin.x, -posAcumulator);
			newItem.offsetMax = new Vector2(newItem.offsetMax.x, -posAcumulator);
			
			posAcumulator += heightItem;
		}
		return posAcumulator;
	}

	void callScene(ItemCategory item){
		User user = User.getInstance;

		user.CurrentTask = user.getTask(user.CurrentCategory.Id, user.CurrentSubCategory.Id);
		
		Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));
		Application.LoadLevel ("Operacoes");

	}
	
}
