using UnityEngine;
using System.Collections;

public class RotateCamera : MonoBehaviour {
	
	private Transform mTransform;
	
	void Awake() {
		mTransform = GetComponent<Transform>();
	}
	
	void FixedUpdate() {
		mTransform.RotateAround(Vector3.zero, Vector3.up, 12.0f * Time.deltaTime);
	}
}
