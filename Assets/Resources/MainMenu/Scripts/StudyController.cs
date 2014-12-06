using UnityEngine;
using System.Collections;

public class ChangeSceneController : MonoBehaviour {
	
	public void changeToScene (string sceneToChangeTo) {
		Application.LoadLevel(sceneToChangeTo);
	}
}
