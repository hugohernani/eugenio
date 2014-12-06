using UnityEngine;
using System.Collections;

public class ThinkingPlayController : MonoBehaviour {
	private static Animator animator;

	void Awake () {
		animator = GetComponent <Animator>();
	}

	public static void playThinkingPlay () {
		animator.SetBool ("bThinkingPlay", true);
	}

	public static void playNoThinkingPlay () {
		animator.SetBool ("bThinkingPlay", false);
	}

}
