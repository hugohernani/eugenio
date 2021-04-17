using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Category {

	int id;
	bool available;
	List<Category> subCategories;

	public Category(){
		this.available = false;
	}

	public int Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public bool Available {
		get {
			return this.available;
		}
		set {
			available = value;
		}
	}

	public List<Category> SubCategories {
		get {
			return this.subCategories;
		}
		set {
			subCategories = value;
		}
	}
}
