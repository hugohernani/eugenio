using UnityEngine;
using System.Collections;

public class SelfDestruct : MonoBehaviour {

	//To delete the explosion(clone)
	public float lifeTime;

	void Start () {
		Destroy(gameObject, lifeTime);
	}
}
