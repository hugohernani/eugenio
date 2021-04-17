using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCategory : Item {

	bool available;
	int categoryId;
	List<Category> categories;
	
	public ItemCategory(string name, bool locked, List<Category> categories, int categoryId){
		base.name = name;
		this.available = locked;
		this.categories = categories;
		this.categoryId = categoryId;
	}

	public bool Available {
		get {
			return this.available;
		}
		set {
			available = value;
		}
	}

	public List<Category> Categories {
		get {
			return this.categories;
		}
		set {
			categories = value;
		}
	}

	public int CategoryId {
		get {
			return this.categoryId;
		}
		set {
			categoryId = value;
		}
	}


}
