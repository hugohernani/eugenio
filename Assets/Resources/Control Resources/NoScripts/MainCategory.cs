using System.Collections.Generic;
public class MainCategory: Category{

	int level;
	string name;
//	string type;
	int initialValue;
	int finalValue;
	int stage;

	public MainCategory ()
	{
		stage = 1;
	}

	public int Stage {
		get {
			return this.stage;
		}
		set {
			stage = value;
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

//	public string Type {
//		get {
//			return this.type;
//		}
//		set {
//			type = value;
//		}
//	}

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

	public int Level {
		get {
			return this.level;
		}
		set {
			level = value;
		}
	}

	public override string ToString() 
	{
		string formatedString = "";
		if(stage == 1){ // Supposing that the category has only one stage it is not necessary to show that.
			// In the other hand, categories which has more than one will have starting showing the formatedString with|from stage 2.
			formatedString = name + " " + initialValue.ToString() + "a" + finalValue.ToString();
		}else{
			formatedString = name + " " + initialValue.ToString() + "a" + finalValue.ToString() + "\nFase " + stage.ToString();
		}
		return formatedString;
	}

}
