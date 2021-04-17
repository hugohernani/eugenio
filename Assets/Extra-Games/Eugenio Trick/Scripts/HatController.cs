using UnityEngine;
using System.Collections;

public class HatController : MonoBehaviour {

	public Camera cam;

	private float maxWidth;
	private bool canControl;

	// Use this for initialization
	void Start () {
		if(cam == null) cam = Camera.main;
		canControl = false;
		setMaxWidth();
	}
	
	void setMaxWidth() {
		Vector3 upperCorner = new Vector3(Screen.width, Screen.height, 0.0f);
		float hatWidth = renderer.bounds.extents.x;
		maxWidth = cam.ScreenToWorldPoint(upperCorner).x - hatWidth;
	}
	
	// FixedUpdate is called once per physics timestep
	void FixedUpdate () {
		if(canControl) {
			Vector3 rawPosition = cam.ScreenToWorldPoint(Input.mousePosition);
			Vector3 targetPosition = new Vector3(rawPosition.x, 0.0f, 0.0f);
			float targetWidth = Mathf.Clamp(targetPosition.x, -maxWidth, maxWidth);

			targetPosition = new Vector3(targetWidth, targetPosition.y, targetPosition.z);
			rigidbody2D.MovePosition(targetPosition);
		}
	}

	public void ToggleControl(bool toggle) {
		canControl = toggle;
	}
}
