using UnityEngine;
using System.Collections;

public class ProjectileFollow : MonoBehaviour {

	public Transform projectile; //The camera is going to follow our (eugenio)
	public Transform farLeft, farRight;

	void Update () {
		//Start the position of the camera
		Vector3 newPosition = transform.position;
		newPosition.x = projectile.position.x;
		newPosition.x = Mathf.Clamp(newPosition.x, farLeft.position.x, farRight.position.x);
		transform.position = newPosition;
	}
}
