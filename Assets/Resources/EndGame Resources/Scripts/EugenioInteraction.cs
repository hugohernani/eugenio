using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EugenioInteraction : MonoBehaviour {

	public Animator anim;
	public Texture2D eugenioFeliz;
	public Texture2D eugenioTriste;
	public Texture2D eugenioNormal;
	private Image eugenioImage;

	void Awake(){
		eugenioImage = GameObject.Find ("Eugenio").GetComponent<Image> ();
	}

	public void EugenioNormal(){
		eugenioImage.material.mainTexture = eugenioNormal;
	}

	public void EugenioHappy(){
		eugenioImage.material.mainTexture = eugenioFeliz;
	}

	public void EugenioSad(){
		eugenioImage.material.mainTexture = eugenioTriste;
	}

	public void EugenioNoAnimation(){
		anim.SetTrigger ("No");
	}

	public void EugenioYesAnimation(){
		anim.SetTrigger ("Yes");
	}
}
