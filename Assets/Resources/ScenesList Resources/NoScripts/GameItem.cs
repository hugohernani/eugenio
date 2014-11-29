using UnityEngine;
using System.Collections;

public class GameItem : Item {

	Sprite sprite;

	public GameItem(string name, Sprite sprite)
	{
		base.name = name;
		this.sprite = sprite;
	}

	public Sprite Sprite {
		get {
			return this.sprite;
		}
	}


}
