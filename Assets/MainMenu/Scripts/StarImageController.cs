using UnityEngine;
using System.Collections;

public class StarImageController : MonoBehaviour {
	private static StarImageController mInstance = null;
	private Animator animator;
	public float time;
	
	public static StarImageController instance
	{
		get
		{
			if (mInstance == null)
			{
				mInstance = GameObject.FindObjectOfType(typeof(StarImageController)) as StarImageController;

				if (mInstance == null)
				{
					mInstance = new GameObject("StarImageController").AddComponent<StarImageController>();
				}
			}
			return mInstance;
		}
	}

	void Awake () {
		if (mInstance == null)
		{
			mInstance = this as StarImageController;
		}

		animator = GetComponent <Animator>();
	}

	void Update () {
	}

	public void playStarImage () {
		instance.StartCoroutine(playAnimation("bStarImageAnim", time));	
	}

	IEnumerator playAnimation (string var, float time) {
		animator.SetBool (var, true);
		yield return new WaitForSeconds(time);
		animator.SetBool (var, false);
	}
}
