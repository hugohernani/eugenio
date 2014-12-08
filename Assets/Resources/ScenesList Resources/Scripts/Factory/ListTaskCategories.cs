using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ListTaskCategories : ListAbstract {

	RectTransform itemPreFab;

	void Awake(){
		itemPreFab = Resources.Load<RectTransform> (PREFAB_RESOURCES + "itemText");
	}

	protected override System.Collections.Generic.List<Item> populateList ()
	{
		User user = User.getInstance;
		List<Category> mainCategories = user.Categories;

		List<Item> items = new List<Item> ();
		
		foreach (MainCategory category in mainCategories) {
			Item item = new ItemCategory(
				category.ToString(),
				category.Available,
				category.SubCategories,
				category.Id
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

			ItemCategory itemTask = items[newInt] as ItemCategory;

			newItem.GetComponent<Text>().text = itemTask.Name; // text

			if(itemTask.Available){

				Destroy(newItem.GetComponentInChildren<Image>());

				UnityAction actionRepobulate = () => {repopulate(itemTask);};
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


	void repopulate(ItemCategory item){

		ListFactory factory = gameObject.GetComponent<ListFactory> ();
		ListAbstract tempList;
		User user = User.getInstance;
		user.CurrentCategory = user.getCategory (item.CategoryId);

		if(item.Categories != null){ // It has SubCategories

			List<Item> items = new List<Item> ();

			foreach (Category category in item.Categories) {
				Task task = user.getTask(item.CategoryId, category.Id);

				MathSubCategory subCategory = (MathSubCategory) user.getSubCategory(category.Id);
				user.CurrentSubCategory = subCategory;

				string nameItem = task.Name + "\n" + subCategory.ToString();

				Item ItemCategory = new ItemCategory(
					nameItem,
					task.Available,
					null,
					item.CategoryId);

				items.Add(ItemCategory);

			}

			tempList = factory.showList("subCategories", gameObject);
			tempList.Items = items;

			tempList.populate(true);
		}else{
			tempList = factory.showList ("Images", gameObject);
			tempList.populate(false);
		}

	}
}
