using UnityEngine;
using System.Collections;

public class ThoughtController : MonoBehaviour {
	private static Animator animator;

	void Awake () {
		animator = GetComponent <Animator>();
	}

	public static void playThinking () {
		animator.SetBool ("bThinking", true);
	}

	public static void playNoThinking () {
		animator.SetBool ("bThinking", false);
	}

}
