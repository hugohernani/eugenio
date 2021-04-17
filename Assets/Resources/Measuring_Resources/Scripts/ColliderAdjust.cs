using UnityEngine;
using System.Collections;

public class ColliderAdjust : MonoBehaviour {

	BoxCollider2D boxCollider2D;

	void Awake(){
		boxCollider2D = GetComponent<BoxCollider2D> ();
	}

	void Start(){
		Rect itselfRect = ((RectTransform)transform).rect;
		boxCollider2D.size = new Vector2(itselfRect.size.x, itselfRect.size.y);
	}



}
