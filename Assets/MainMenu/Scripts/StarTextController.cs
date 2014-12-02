using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class StarTextController : MonoBehaviour {
	private static StarTextController mInstance = null;
	private Animator animator;
	public float time;
	private Text text;
	public Text text2;
	private int result;

	User user;


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

		animator = GetComponent <Animator>();
		text = GetComponent <Text>();

		user = User.getInstance;
		text2.text = user.Stars_qty.ToString();

	}

	public void playStarText (int value) {
		result = Int32.Parse(text2.text) - value;

		if(result < 0){
			// TODO David, change this according to your animation name to deal with not enough money to buy food.
			instance.StartCoroutine(instance.playAnimation("NOT_ENOUGH_MONEY", time));
		}else{
			text.text = value.ToString();
			instance.StartCoroutine(instance.playAnimation("bStarTextAnim", time));
			user.Stars_qty = result;
			text2.text = result.ToString();
		}
	}

	IEnumerator playAnimation (string var, float time) {
		animator.SetBool (var, true);
		yield return new WaitForSeconds(time);
		animator.SetBool (var, false);
	}
}
