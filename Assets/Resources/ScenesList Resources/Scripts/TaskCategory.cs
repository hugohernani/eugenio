public class TaskCategory{

	int id;
	string name;
	string type;
	int initialValue;
	int finalValue;
	int stage;
	bool available;

	public TaskCategory ()
	{
		stage = -1;
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

	public string Name {
		get {
			return this.name;
		}
		set {
			name = value;
		}
	}

	public string Type {
		get {
			return this.type;
		}
		set {
			type = value;
		}
	}

	public int InitialValue {
		get {
			return this.initialValue;
		}
		set {
			initialValue = value;
		}
	}

	public int FinalValue {
		get {
			return this.finalValue;
		}
		set {
			finalValue = value;
		}
	}

	public int Stage {
		get {
			return this.stage;
		}
		set {
			stage = value;
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

	public override string ToString() 
	{
		return (name + " " + initialValue.ToString() + "a" + finalValue.ToString());
	}

}
