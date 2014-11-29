using UnityEngine;
using System.Collections;

public class Pet{

	float health;
	float entertainment;
	float feed;
	float experience;

	public Pet (float feed, float health, float entertainment)
	{
		this.health = health;
		this.entertainment = entertainment;
		this.feed = feed;
	}

	public float Health {
		get {
			return this.health;
		}
		set {
			health = value;
		}
	}

	public float Entertainment {
		get {
			return this.entertainment;
		}
		set {
			entertainment = value;
		}
	}

	public float Feed {
		get {
			return this.feed;
		}
		set {
			feed = value;
		}
	}

	public float Experience {
		get {
			return this.experience;
		}
		set {
			experience = value;
		}
	}


}
