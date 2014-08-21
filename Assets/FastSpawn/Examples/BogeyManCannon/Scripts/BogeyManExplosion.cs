using UnityEngine;
using System.Collections;

public class BogeyManExplosion : MonoBehaviour {
	private Vector3[] mOriginalChildPosition;
	private FastSpawnObject mFastSpawnObject;
	private Transform mTransform;
	
	void Awake() {
		mTransform = GetComponent<Transform>();
		mFastSpawnObject = GetComponent<FastSpawnObject>();

		mFastSpawnObject.OnSpawn += OnSpawn;
		mFastSpawnObject.OnUnspawn += OnUnspawn;
		
		mOriginalChildPosition = new Vector3[mTransform.childCount];

		for(int i=0; i<mTransform.childCount; i++) {
			Transform child = mTransform.GetChild(i);

			mOriginalChildPosition[i] = child.localPosition;
		}
	}
	
	private void AddExplosionForce() {
		for(int i=0; i<mTransform.childCount; i++) {
			Rigidbody childRigidbody = mTransform.GetChild(i).GetComponent<Rigidbody>();
			if(childRigidbody != null) {
				childRigidbody.AddExplosionForce(200.0f, mTransform.position, 4.0f, -1.0f, ForceMode.Impulse);
			}
		}	
	}

	private void ResetExplosion() {
		for(int i=0; i<mTransform.childCount; i++) {
			Transform child = mTransform.GetChild(i);

			child.localPosition = mOriginalChildPosition[i];
			child.rotation = Quaternion.identity;
		}
	}

	void Destroy() {
		mFastSpawnObject.OnSpawn -= OnSpawn;
		mFastSpawnObject.OnUnspawn -= OnUnspawn;
	}
	
	void OnSpawn() {
#if UNITY_3_5
		gameObject.SetActiveRecursively(true);
#endif
		AddExplosionForce();
	}
	
	void OnUnspawn() {
		ResetExplosion();
#if UNITY_3_5
		gameObject.SetActiveRecursively(false);
#endif
	}
}
