using UnityEngine;
using System.Collections;

public class DragDropItem : MonoBehaviour {

	RectTransform rectTransformItSelf;
	bool isOut = false;
	Vector3 initialPosition;

	void Awake(){
		rectTransformItSelf = (RectTransform)transform;
		initialPosition = rectTransformItSelf.localPosition;
	}

	void OnTriggerExit2D(Collider2D other){
		isOut = true;
	}

	void OnTriggerEnter2D(Collider2D other){
		isOut = false;
	}

	void Update() {
		if(isOut){
			rectTransformItSelf.position = initialPosition;
//			rectTransformItSelf.position = Vector3.Lerp (initialPosition, initialPosition, Time.deltaTime);
		}else{
			if (Input.GetMouseButton(0)){
				
				RaycastHit2D hit = Physics2D.Raycast (Input.mousePosition, rectTransformItSelf.position, 100, 1 << LayerMask.NameToLayer("Item"));
				if(hit.collider != null){
					rectTransformItSelf.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
				}
			}
		}
	}
}
