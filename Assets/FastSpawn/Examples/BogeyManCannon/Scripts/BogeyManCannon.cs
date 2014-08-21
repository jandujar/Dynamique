using UnityEngine;
using System.Collections;

public class BogeyManCannon : MonoBehaviour {
	
	public float range = 10.0f;
	public float cooldown = 0.5f;
	public float rotationSpeed = 2.0f;
	public float shootStartAngle = 5.0f;

	public Transform body;
	public Transform barrel;
	public Transform ballSpawnPoint;
	
	private Transform mTransform;
	private BogeyMan mTarget;
	private float mWeaponReadyTime;
	
	// Use this for initialization
	void Awake() {
		mTransform = GetComponent<Transform>();
		mWeaponReadyTime = Time.time + cooldown;
	}
	
	// Update is called once per frame
	void Update () {
		// If there is no target, or the target is not alive, find nearest target in range
		if(mTarget == null || (mTarget != null && !mTarget.alive)) {
			Collider[] colliders = Physics.OverlapSphere(mTransform.position, range);
			
			float nearestDistance = float.MaxValue;
			
			for(int i=0; i<colliders.Length; i++) {
				BogeyMan bogeyMan = colliders[i].GetComponent<BogeyMan>();
				
				if(bogeyMan) {
					float sqrDistance = (mTransform.position - bogeyMan.GetTransform().position).sqrMagnitude;
					
					if(sqrDistance < nearestDistance) {
						nearestDistance = sqrDistance;
						mTarget = bogeyMan;
					}
				}
			}
		}
		// Aim target
		else {
			Vector3 diff = mTarget.GetHeadPosition() - mTransform.position + Vector3.up * 0.5f;

			// Horizontal rotation
				
			Vector3 dir = diff;		
			dir.y = 0.0f;
			dir.Normalize();
			
			Quaternion newRotation = Quaternion.LookRotation(dir);
				
			body.rotation = Quaternion.Slerp(body.rotation, newRotation, Time.deltaTime * rotationSpeed);	
			
			// Vertical rotation
				
			Vector3 jointDistance = mTarget.GetHeadPosition() - barrel.position;
			
			Vector3 normal = Vector3.Cross(jointDistance.normalized, barrel.right);
	
			Vector3 normalPos = mTarget.GetHeadPosition() - normal;
	
			float angle = -AngleSigned((normalPos - barrel.position).normalized , body.forward, Vector3.right);
	
			angle = Mathf.Clamp(angle, -45.0f, 10.0f);
			
			angle = Mathf.LerpAngle(barrel.localRotation.eulerAngles.x, angle, Time.deltaTime * rotationSpeed);
			
			barrel.localRotation = Quaternion.Euler(new Vector3(angle, 0, 0));

			// If the weapon is ready and the angle is close enough
			
			if(Vector3.Angle(dir, body.forward) < shootStartAngle && Time.time > mWeaponReadyTime) {
				if(BogeyManSpawnManager.SharedInstance != null) {
					
					// Spawn muzzle flash to the tip of the cannon barrel
					BogeyManSpawnManager.SharedInstance.SpawnObject(BogeyManSpawnManager.SharedInstance.level1.muzzleFlash, ballSpawnPoint.position, ballSpawnPoint.rotation);					
					
					// Spawn a cannon ball to the thip of the cannon barrel
					FastSpawnObject spawnedObject = BogeyManSpawnManager.SharedInstance.SpawnObject(BogeyManSpawnManager.SharedInstance.level1.cannonBall, ballSpawnPoint.position, ballSpawnPoint.rotation);					
					if(spawnedObject) {
						BogeyManCannonBall cannonBall = spawnedObject.GetComponent<BogeyManCannonBall>();
						
						cannonBall.GetRigidbody().AddForce(ballSpawnPoint.forward * 1500.0f, ForceMode.Force); 
						
						mWeaponReadyTime = Time.time + cooldown;
					}
				}
			}
		}
	}
	
	private float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n) {
		return Mathf.Atan2(Vector3.Dot(n, Vector3.Cross(v1, v2)), Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
	}
}
