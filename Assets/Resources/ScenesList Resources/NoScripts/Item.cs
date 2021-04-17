using UnityEngine;
using System.Collections;
using UnityEngine.UI;

	public abstract class Item{

	protected int id;
	protected string name;

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


}
