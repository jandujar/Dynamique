using UnityEngine;
using System.Collections;

public class BogeyMan : MonoBehaviour {
	
	public Transform target;
	
	public float turnSpeed = 4.0f;
	public float moveSpeed = 2.0f;
	public float stoppingDistance = 4.0f;
	public float bounceInterval = 0.25f;
	public float bounceForce = 196.0f;
	public bool alive = true;
	public float maxDamage = 1.0f;
	
	private Transform mTransform;
	private Rigidbody mRigidbody;
	private Renderer mRenderer;
	private FastSpawnObject mFastSpawnObject;
	private float mNextBounceTime;
	private float mTakenDamage;

	private bool mGrounded = false;

	void Awake() {
		mTransform = GetComponent<Transform>();	
		mRigidbody = GetComponent<Rigidbody>();
		mRenderer = GetComponentInChildren<Renderer>();
		mFastSpawnObject = GetComponent<FastSpawnObject>();
		
		mRigidbody.useGravity = false;
		mRigidbody.freezeRotation = true;
		
		// Set spawn and unspawn event listeners
		mFastSpawnObject.OnSpawn += OnSpawn;
		mFastSpawnObject.OnUnspawn += OnUnspawn;
	}
	
	void Destroy() {
		// Remove spawn and unspawn event listeners
		mFastSpawnObject.OnSpawn -= OnSpawn;
		mFastSpawnObject.OnUnspawn -= OnUnspawn;
	}
	
	void OnSpawn() {
		alive = true;
		mTakenDamage = 0.0f;
#if UNITY_3_5
		gameObject.SetActiveRecursively(true);
#endif
	}
	
	void OnUnspawn() {
#if UNITY_3_5
		gameObject.SetActiveRecursively(false);
#endif
	}
	
	void FixedUpdate() {
		if(target && alive) {
			Vector3 targetDiff = target.position - mTransform.position;
			
			// Rotate towards target
			Vector3 direction = targetDiff;
			direction.y = 0.0f;
			direction.Normalize();
			
			mTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(mTransform.forward, direction, turnSpeed * Time.deltaTime, 0.0f));
			
			// Move towards target
			if(mGrounded && targetDiff.sqrMagnitude > (stoppingDistance * stoppingDistance)) {
				Vector3 targetVelocity = mTransform.forward * moveSpeed;
				Vector3 deltaVelocity = targetVelocity - mRigidbody.velocity;

				mRigidbody.AddForce(deltaVelocity * 10.0f, ForceMode.Acceleration);
			}
			
			// Make bogeyman bounce in regular intervals
			
			if(mGrounded && Time.time > mNextBounceTime) {
				mRigidbody.AddForce(bounceForce * Vector3.up, ForceMode.Force);
				mNextBounceTime = Time.time + bounceInterval;
			}
			else {
				// Add gravity
				mRigidbody.AddForce(-9.81f * Vector3.up);
			}
		}
		
		mGrounded = false;
	}
	
	void OnCollisionStay() {
		mGrounded = true;	
	}
	
	public void TakeDamage(float damage) {
		mTakenDamage += damage;
		
		if(mTakenDamage >= maxDamage) {
			Die();
		}
	}
	
	public void Die() {
		alive = false;

		if(BogeyManSpawnManager.SharedInstance != null) {
			// Unspawn this bogeyman
			BogeyManSpawnManager.SharedInstance.UnspawnObject(mFastSpawnObject);
			
			// Spawn a bogeyman explosion to the position of this transform
			FastSpawnObject spawnedObject = BogeyManSpawnManager.SharedInstance.SpawnObject(BogeyManSpawnManager.SharedInstance.level1.bogeyManExplosion, mTransform.position, Quaternion.identity);
			
			if(spawnedObject && spawnedObject.cachedTransform) {
				spawnedObject.cachedTransform.localScale = mTransform.localScale * 2.6f;
			}
		}
	}
	
	public Vector3 GetHeadPosition() {
		return mTransform.position + 0.8f * mRenderer.bounds.size.y * Vector3.up;
	}
	
	public Transform GetTransform() {
		return mTransform;	
	}
}
