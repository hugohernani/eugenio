using UnityEngine;
using System.Collections;

public class ItemFood : Item {

	Sprite icon;
	int value;

	public ItemFood (string name, Sprite icon, int value)
	{
		base.name = name;
		this.icon = icon;
		this.value = value;
	}
	
	
	public Sprite Icon {
		get {
			return this.icon;
		}
		set {
			icon = value;
		}
	}
	
	public int Value {
		get {
			return this.value;
		}
		set {
			this.value = value;
		}
	}


}
