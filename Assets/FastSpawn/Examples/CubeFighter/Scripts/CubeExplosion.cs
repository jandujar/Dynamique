using UnityEngine;
using System.Collections;

public class CubeExplosion : MonoBehaviour {
	
	private FastSpawnObject mFastSpawnObject;
	private Vector3[] mOriginalChildPosition;

	void Awake() {
		mFastSpawnObject = GetComponent<FastSpawnObject>();
		
		if(mFastSpawnObject != null) {
			mFastSpawnObject.OnSpawn += OnSpawn;
			mFastSpawnObject.OnUnspawn += OnUnspawn;
		}

		mOriginalChildPosition = new Vector3[transform.childCount];

		for(int i=0; i<transform.childCount; i++) {
			Transform child = transform.GetChild(i);

			mOriginalChildPosition[i] = child.localPosition;
		}
	}
	
	void OnDestroy() {
		if(mFastSpawnObject != null) {
			mFastSpawnObject.OnSpawn -= OnSpawn;
			mFastSpawnObject.OnUnspawn -= OnUnspawn;
		}
	}
	
	private void ResetExplosion() {
		for(int i=0; i<transform.childCount; i++) {
			Transform child = transform.GetChild(i);

			child.localPosition = mOriginalChildPosition[i];
			child.rotation = Quaternion.identity;
		}
	}
	
	private void AddExplosionForce() {
		for(int i=0; i<transform.childCount; i++) {
			Rigidbody childRigidbody = transform.GetChild(i).GetComponent<Rigidbody>();
			if(childRigidbody != null) {
				childRigidbody.AddExplosionForce(10.0f, transform.position, 2.0f, 2.0f, ForceMode.Impulse);
			}
		}	
	}
	
	private void OnSpawn() {
		// On older Unity versions we want to enable child objets too
#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
		transform.gameObject.SetActiveRecursively(true);
#endif
		
		// Add explosion force to the child objects
		AddExplosionForce();
	}
	
	private void OnUnspawn() {
		// Reset child objects to their original position
		ResetExplosion();

		// On older Unity versions we want to disable child objets too
#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
		transform.gameObject.SetActiveRecursively(false);
#endif
	}
}
