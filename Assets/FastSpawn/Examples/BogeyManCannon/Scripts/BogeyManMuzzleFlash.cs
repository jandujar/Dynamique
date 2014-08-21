using UnityEngine;
using System.Collections;

public class BogeyManMuzzleFlash : MonoBehaviour {

	private FastSpawnObject mFastSpawnObject;
	private MeshRenderer mRenderer;

	void Awake() {
		mRenderer = GetComponentInChildren<MeshRenderer>();

		mFastSpawnObject = GetComponent<FastSpawnObject>();

		mFastSpawnObject.OnSpawn += OnSpawn;
		mFastSpawnObject.OnUnspawn += OnUnspawn;
	}

	void Destroy() {
		mFastSpawnObject.OnSpawn -= OnSpawn;
		mFastSpawnObject.OnUnspawn -= OnUnspawn;
	}
	
	void OnSpawn() {
		mRenderer.material.color = new Color(1, 1, 1, 1);
#if UNITY_3_5
		gameObject.SetActiveRecursively(true);
#endif
	}
	
	void OnUnspawn() {
#if UNITY_3_5
		gameObject.SetActiveRecursively(false);
#endif
	}
	
	void Update() {
		if(mRenderer) {
			// Unspawn this object after it is shown for a small while
			float alpha = Mathf.Lerp(mRenderer.material.color.a, 0.0f, Time.time * 0.025f);
			if(alpha < 0.05f) {
				BogeyManSpawnManager.SharedInstance.UnspawnObject(mFastSpawnObject);	
			}
			else {
				mRenderer.material.color = new Color(1, 1, 1, alpha);
			}
		}
	}
}
