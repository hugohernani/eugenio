using UnityEngine;
using System.Collections;

public class ItemTask : Item {

	string categoryTask;
	int initial;
	int final;
	bool available;

	public enum CategoryTasks: int{
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

	public ItemTask(string name, string category, int initial, int final, bool locked){
		base.name = name;
		this.categoryTask = category;
		this.initial = initial;
		this.final = final;
		this.available = locked;
	}


	public string CategoryTask {
		get {
			return this.categoryTask;
		}
		set {
			categoryTask = value;
		}
	}

	public int Initial {
		get {
			return this.initial;
		}
		set {
			initial = value;
		}
	}

	public int Final {
		get {
			return this.final;
		}
		set {
			final = value;
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


}
