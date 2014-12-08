using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ListImages : ListAbstract {

	RectTransform itemPreFab;
	string resourceImageItem;
	User user;

	void Awake(){
		user = User.getInstance;
		itemPreFab = Resources.Load<RectTransform> (PREFAB_RESOURCES + "itemImage");
		resourceImageItem = ITEM_RESOURCES + "ItemsTasks/";
	}
	
	protected override List<Item> populateList ()
	{
		List<Task> tasks = user.TasksByCategory (user.CurrentCategory.Id);

		List<Item> items = new List<Item> ();


		foreach (Task task in tasks) {
			Item item = new ItemImages(
					task.Name,
					Resources.Load<Sprite> (resourceImageItem + task.Name),
					task.Available
					);
			items.Add(item);
		}

		return items;
	}
	
	protected override float populateContainer (List<Item> items, RectTransform container)
	{
		float heightItem = 100f;
		float posAcumulator = 0f;
		// change this according to the quantity of items or scene
		for (int i = 0; i < items.Count; i++) {
			RectTransform newItem = Instantiate (itemPreFab, container.transform.position, Quaternion.identity) as RectTransform;
			
			int newInt = i;
			
			ItemImages itemImage = items[newInt] as ItemImages;
			
			newItem.GetComponent<Image>().sprite = itemImage.getSprite;

			if(itemImage.Available){
				Destroy(newItem.GetChild(0).gameObject);
				UnityAction clickAction = () => {callScene(itemImage);};
				newItem.GetComponentInChildren<Button>().onClick.AddListener(clickAction);
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


	void callScene(ItemImages item){

		user.CurrentTask = user.getTask (item.Name, user.CurrentCategory.Id);

		user.StartSavingTask();

		Application.LoadLevel (item.Name);

		GameObject persistenceTask = Resources.Load<GameObject> ("All_Task/prefab/TASK_RUNNING");

		Instantiate (persistenceTask);

		Destroy(GameObject.FindGameObjectWithTag("MAIN_SCENE_OBJECT"));
	}


}
