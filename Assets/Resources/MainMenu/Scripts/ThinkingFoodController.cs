using UnityEngine;
using System.Collections;

public class ThinkingFoodController : MonoBehaviour {
	private static Animator animator;

	void Awake () {
		animator = GetComponent <Animator>();
	}

	public static void playThinkingFood () {
		animator.SetBool ("bThinkingFood", true);
	}

	public static void playNoThinkingFood () {
		animator.SetBool ("bThinkingFood", false);
	}

}
