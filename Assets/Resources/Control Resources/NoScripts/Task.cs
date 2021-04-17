using UnityEngine;

public class Task{

	private int id;
	private string name;
	private int categoryId;
	private int subCategoryId;
	private int subStage;
	private bool available;

	public Task ()
	{
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

	public int CategoryId {
		get {
			return this.categoryId;
		}
		set {
			categoryId = value;
		}
	}

	public int SubCategoryId{
		get{
			return this.subCategoryId;
		}
		set {
			subCategoryId = value;
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
		return string.Format ("[Task: Id={0}, Name={1}, CategoryId={2}, SubCategoryId={3}, Available={4}]", Id, Name, CategoryId, SubCategoryId, Available);
	}

}
