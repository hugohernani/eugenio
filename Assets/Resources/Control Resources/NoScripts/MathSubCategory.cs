public class MathSubCategory : Category {

	int subStage;
	int value;
	bool integer;
	bool random;
	Category category;
	
	public int SubStage {
		get {
			return this.subStage;
		}
		set {
			subStage = value;
		}
	}
	
	public int Value {
		get {
			return this.value;
		}
		set {
			this.value = value;
		}
	}

	public bool Integer {
		get {
			return this.integer;
		}
		set {
			integer = value;
		}
	}
	
	public Category Category {
		get {
			return this.category;
		}
		set {
			category = value;
		}
	}

	public bool Random {
		get {
			return this.random;
		}
		set {
			random = value;
		}
	}

	public override string ToString ()
	{
		string formatedString = "";
		if(this.random){
			formatedString = "Subfase " + subStage + "\nOperaçoes aleatorias";
		}else{
			formatedString = "Subfase " + SubStage + "\nOperaçoes com o numero " + Value;
		}
		return (formatedString);
	}

}
