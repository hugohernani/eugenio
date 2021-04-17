using UnityEngine;
using System.Collections;

public class GameItem : Item {

	Sprite sprite;
	bool available;

	public GameItem(string name, Sprite sprite, bool available)
	{
		base.name = name;
		this.sprite = sprite;
		this.available = available;
	}

	public Sprite Sprite {
		get {
			return this.sprite;
		}
	}

	public bool Available{
		get {
			return this.available;
		}
	}


}
