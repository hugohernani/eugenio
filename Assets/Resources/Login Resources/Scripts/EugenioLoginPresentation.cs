using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EugenioLoginPresentation : MonoBehaviour {
	int total, qty;
	RectTransform eugenioPrefab;
	Transform eugenioHolder;
	List<RectTransform> eugenios;

	void Awake () {
		eugenios = new List<RectTransform>();
		total = 0;
		qty = Random.Range(1,6);
		eugenioPrefab = Resources.Load<RectTransform> ("Login Resources/prefabs/EugenioPrefab");
		eugenioHolder = GameObject.Find ("EugenioHolder").GetComponent<RectTransform>() as Transform;
	}

	void Start(){
		StartCoroutine(Spawn(qty));
	}


	IEnumerator Spawn(int qty) {
		if(eugenios.Count < 11){
			while(qty-- > 0){
				yield return new WaitForSeconds(1f);
				RectTransform eugenio = Instantiate(eugenioPrefab, eugenioHolder.position, Quaternion.identity) as RectTransform;
				//			yield return new WaitForSeconds(1f);
				eugenio.transform.SetParent (eugenioHolder);
				
				eugenio.transform.localScale = Vector3.one;
				eugenio.transform.localPosition = new Vector3 (0, 0f, 0f);

				
				int spawnXPosition = Random.Range(-454, 20);
				eugenio.offsetMax = new Vector2(spawnXPosition, 100f);
				eugenio.offsetMin = new Vector2(spawnXPosition, 100f);
				eugenios.Add(eugenio);
			}
		}else{
			foreach(Transform transform in eugenioHolder){
				Destroy(transform.gameObject,0.5f);
			}
			eugenios.Clear();
			yield return null;
		}
		yield return null;

	}

	void OnTriggerExit2D(Collider2D other) {
		Destroy (other.gameObject);
		StartCoroutine(Spawn(Random.Range(1,2)));
	}

	void OnDestroy(){
		foreach (Transform transform in eugenioHolder.transform) {
			Destroy (transform.gameObject);
		}
	}
}
