using UnityEngine;
using System.Collections;

public class ItemImages : Item {

	bool available;
	Sprite sprite;

	public enum CategoryImages: int{
		ContarOrdernarMedir = 1,
		Adicao = 2
	}
	
	// TODO adjust with real intervals
	public enum CategoryStageInitial: int{
		Stage1 = 1,
		Stage2 = 21,
		Stage3 = 41
	}
	
	// TODO adjust with real intervals
	public enum CategoryStageFinal: int{
		Stage1 = 20,
		Stage2 = 40,
		Stage3 = 60
	}

	public ItemImages (string name, Sprite sprite, bool available)
	{
		base.name = name;
		this.sprite = sprite;
		this.available = available;
	}

	public Sprite getSprite{
		get {
			return this.sprite;
		}
	}

	public bool Available {
		get {
			return this.available;
		}
	}

}
