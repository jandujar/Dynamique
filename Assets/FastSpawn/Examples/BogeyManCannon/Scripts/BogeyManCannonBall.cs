using UnityEngine;
using System.Collections;

public class BogeyManCannonBall : MonoBehaviour {
	
	public float explosionRadius = 2.0f;

	private FastSpawnObject mFastSpawnObject;
	private Transform mTransform;
	private Rigidbody mRigidbody;
	
	void Awake() {
		mFastSpawnObject = GetComponent<FastSpawnObject>();		
		mFastSpawnObject.OnSpawn += OnSpawn;
		
		mTransform = GetComponent<Transform>();
		mRigidbody = GetComponent<Rigidbody>();
		
		mRigidbody.useGravity = false;
	}
	
	void FixedUpdate() {
		// Use custom, lower gravity
		mRigidbody.AddForce(-1.5f * Vector3.up);
	}
	
	void Destroy() {
		mFastSpawnObject.OnSpawn -= OnSpawn;
	}
	
	void OnSpawn() {
		mRigidbody.velocity = Vector3.zero;
	}

	void OnCollisionEnter(Collision collision) {
		if(BogeyManSpawnManager.SharedInstance != null) {
			// Unspawn this cannon ball when it hits something
			BogeyManSpawnManager.SharedInstance.UnspawnObject(mFastSpawnObject);
			
			// Spawn cannon ball explosion to the position of this transform
			BogeyManSpawnManager.SharedInstance.SpawnObject(BogeyManSpawnManager.SharedInstance.level1.cannonBallExplosion, mTransform.position, Quaternion.identity);
			
			// Give some damage to any bogeymans in range
			Collider[] hitColliders = Physics.OverlapSphere(mTransform.position, explosionRadius);
		
			for(int i=0; i<hitColliders.Length; i++) {
				BogeyMan bogeyMan = hitColliders[i].GetComponent<BogeyMan>();
				
				if(bogeyMan) {
					bogeyMan.TakeDamage(1.0f);
				}
			}
		}
	}
	
	public Rigidbody GetRigidbody() {
		return mRigidbody;	
	}
}
