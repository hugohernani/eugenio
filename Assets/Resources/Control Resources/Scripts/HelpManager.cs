using UnityEngine;
using System.Collections;

public class HelpManager : MonoBehaviour {

	GameObject sceneMenuHelp;
	int hintPosition;

	void Awake(){
		hintPosition = 1;
		if(Application.loadedLevelName == "MainMenu"){
			GameObject menuHelpResources = Resources.Load<GameObject>("MainMenu/prefab/MenuHelp");
			sceneMenuHelp = Instantiate(menuHelpResources, gameObject.transform.position, Quaternion.identity) as GameObject;			                                                                                                                 ;
		}else if(Application.loadedLevelName == "Operation"){
			Debug.Log("Add Operation prefab animations to the Help Dialog");
		}else if(Application.loadedLevelName == "Contar"){
			GameObject contarHelpResources = Resources.Load<GameObject>("Counting Resources/PreFabs/HelpCounting");
			sceneMenuHelp = Instantiate(contarHelpResources, gameObject.transform.position, Quaternion.identity) as GameObject;
		}
		sceneMenuHelp.transform.parent = gameObject.transform;

		Acordar ();
	}

	protected virtual void Acordar(){
		// Do nothing
	}

	void Start(){
		Iniciar ();
	}

	void FixedUpdate(){
		if(Input.GetKeyDown(KeyCode.RightArrow)){
			nextHint();
		}else if(Input.GetKeyDown(KeyCode.LeftArrow)){
			previousHint();
		}

		Atualizar ();
	}

	protected virtual void Atualizar(){
		// In Child
	}

	protected virtual void Iniciar(){
		// Do nothing
	}

	void OnMouseOver(){
		MouseSobre ();
	}

	protected virtual void MouseSobre(){
		// Do nothing
	}

	protected virtual void nextHint(){
		// Treat in Child
	}

	protected virtual void previousHint(){
		// Treat in Child
	}

	protected int HintPosition {
		get {
			return this.hintPosition;
		}
		set {
			hintPosition = value;
		}
	}

	public void Destroy(){
		Destroy (gameObject);
	}

}
