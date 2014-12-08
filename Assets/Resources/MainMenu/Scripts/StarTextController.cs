using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class StarTextController : MonoBehaviour {
	private static StarTextController mInstance = null;
	EugenioController eugenioController;
	public float time;
	private Text text;
	public Text text2;
	private int result;
	
	public static StarTextController instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = GameObject.FindObjectOfType(typeof(StarTextController)) as StarTextController;

				if (mInstance == null)
				{
					mInstance = new GameObject("StarTextController").AddComponent<StarTextController>();
				}
			}
			return mInstance;
		}
	}

	void Awake () {
		if (mInstance == null)
		{
			mInstance = this as StarTextController;
		}
		text = GetComponent <Text>();
	}

	public void updateStarValue(int value){
		text2.text = value.ToString();
	}

	public int changeStarText (int value) {
		result = Int32.Parse(text2.text) - value;
		if(result > 0){
			text.text = value.ToString();
			text2.text = result.ToString();
		}
		return result;
	}
}
