using UnityEngine;
using System.Collections;

public class ListFactory: MonoBehaviour{

	public ListAbstract showList(string category, GameObject go = null){

		ListAbstract list = null;

		if(category == "food"){
			list = go.gameObject.AddComponent<ListFood>();
		}else if(category == "task"){
			list = go.gameObject.AddComponent<ListTaskCategories>();
		}else if(category == "subCategories"){
			list = go.gameObject.AddComponent<ListSubCategories>();
		}else if(category == "Images"){
			list = go.gameObject.AddComponent<ListImages>();
		}else if(category == "Games"){
			list = go.gameObject.AddComponent<ListGames>();
		}else{
			throw new ExitGUIException();
		}

		return list;

	}



}
