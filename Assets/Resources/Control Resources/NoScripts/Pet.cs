using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Pet{

	int userId;
	float health;
	float entertainment;
	float feed;
	float experience;

	public Pet(){

	}

	public Pet (float feed, float health, float entertainment)
	{
		this.health = health;
		this.entertainment = entertainment;
		this.feed = feed;
	}

	public int UserId {
		get {
			return this.userId;
		}
		set {
			userId = value;
		}
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

	public override string ToString ()
	{
		return string.Format ("[Pet: Health={0}, Entertainment={1}, Feed={2}, Experience={3}]", Health, Entertainment, Feed, Experience);
	}


}
