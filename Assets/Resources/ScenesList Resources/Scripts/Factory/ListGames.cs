using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

public class ListGames : ListAbstract {

	RectTransform itemPreFab;
	string resourceGameItem;
	User user;
	
	void Awake(){
		user = User.getInstance;
		itemPreFab = Resources.Load<RectTransform> (PREFAB_RESOURCES + "itemImage");
		resourceGameItem = ITEM_RESOURCES + "ItemsGames/";
	}
	
	protected override List<Item> populateList ()
	{
		List<Game> games = user.Games;
		
		List<Item> items = new List<Item> ();
		
		foreach (Game game in games) {
			Item item = new GameItem(
				game.Name,
				Resources.Load<Sprite> (resourceGameItem + game.Name),
				game.Available
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
			
			GameItem gameItem = items[newInt] as GameItem;
			
			newItem.GetComponent<Image>().sprite = gameItem.Sprite;

			if(gameItem.Available){
				Destroy(newItem.GetChild(0).gameObject);
				UnityAction clickAction = () => {callScene(gameItem);};
				newItem.GetComponentInChildren<Button>().onClick.AddListener(clickAction);
			}

			newItem.SetParent(container.transform);
			
			newItem.offsetMax = Vector2.zero;
			newItem.offsetMin = Vector2.zero;
			
			newItem.offsetMin = new Vector2(newItem.offsetMin.x, -posAcumulator);
			newItem.offsetMax = new Vector2(newItem.offsetMax.x, -posAcumulator);
			
			posAcumulator += heightItem;
		}
		return posAcumulator;
	}
	
	void callScene(GameItem item){
		user.CurrentGame = user.getGame (item.Name);
		user.startSavingGame ();

		Debug.Log (user.CurrentGame.ToString ());
		
		Application.LoadLevel(user.CurrentGame.Name);
	}

}