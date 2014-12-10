using UnityEngine;
using System.Collections;

public class Game {

	int id;
	string name;
	bool available;
	int currentScore;
	int currentRecord;

	public int Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
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

	public int CurrentScore {
		get {
			return this.currentScore;
		}
		set {
			currentScore = value;
		}
	}

	public int CurrentRecord {
		get {
			return this.currentRecord;
		}
		set {
			currentRecord = value;
		}
	}

	public override string ToString ()
	{
		return string.Format ("[Game: Id={0}, Name={1}, Available={2}, CurrentScore={3}, CurrentRecord={4}]", Id, Name, Available, CurrentScore, CurrentRecord);
	}


}
