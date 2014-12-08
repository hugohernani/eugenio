using UnityEngine;
using System.Collections;

public class Game {

	int id;
	string name;
	bool available;

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

	public override string ToString ()
	{
		return string.Format ("[Game: Id={0}, Name={1}, Available={2}]", Id, Name, Available);
	}



}
