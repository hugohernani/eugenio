using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountingItemsControl : MonoBehaviour {


	public void destroyItems ()
	{
		Debug.Log ("here");
		Destroy (this.gameObject.GetComponent<Image> (), 6f);
		Destroy (this.gameObject.transform.parent.gameObject, 10f);
	}
}
