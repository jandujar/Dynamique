using UnityEngine;
using System.Collections;

public class BogeyManLogic : MonoBehaviour {
	
	public Transform cannon;
	public Transform[] spawnPoints;

	private BogeyManSpawnManager mSpawnManager;
	
	private float smallBogeyManSpawnTime;
	private float smallBogeyManSpawnDelay = 3.0f;

	private float mediumBogeyManSpawnTime;
	private float mediumBogeyManSpawnDelay = 40.0f;

	private float bigBogeyManSpawnTime;
	private float bigBogeyManSpawnDelay = 60.0f;
	
	void Awake() {
		mSpawnManager = GetComponent<BogeyManSpawnManager>();
		
		smallBogeyManSpawnTime = Time.time/* + smallBogeyManSpawnDelay*/;
		mediumBogeyManSpawnTime = Time.time + mediumBogeyManSpawnDelay;
		bigBogeyManSpawnTime = Time.time + bigBogeyManSpawnDelay;
	}

	void Update () {
		// Check if should try to spawn a small bogeyman
		if(Time.time > smallBogeyManSpawnTime) {
			Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			Vector3 dir = cannon.position - position;
			dir.y = 0.0f;
				
			// Spawn small bogeyman
			FastSpawnObject spawnedObject = mSpawnManager.SpawnObject(mSpawnManager.level1.smallBogeyMan, position, Quaternion.LookRotation(dir));
			
			if(spawnedObject) {
				BogeyMan bogeyMan = spawnedObject.GetComponent<BogeyMan>();
			
				if(bogeyMan) {
					bogeyMan.target = cannon;
				}
			}
			
			smallBogeyManSpawnTime = Time.time + smallBogeyManSpawnDelay;
		}
		
		// Check if should try spawn a medium bogeyman
		if(Time.time > mediumBogeyManSpawnTime) {
			Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			Vector3 dir = cannon.position - position;
			dir.y = 0.0f;
			
			// Spawn medium bogeyman
			FastSpawnObject spawnedObject = mSpawnManager.SpawnObject(mSpawnManager.level1.mediumBogeyMan, position, Quaternion.LookRotation(dir));
			
			if(spawnedObject) {
				BogeyMan bogeyMan = spawnedObject.GetComponent<BogeyMan>();
			
				if(bogeyMan) {
					bogeyMan.target = cannon;
				}
			}
			
			mediumBogeyManSpawnTime = Time.time + mediumBogeyManSpawnDelay;
		}
		
		// Check if should try spawn a big bogeyman
		if(Time.time > bigBogeyManSpawnTime) {
			Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
			Vector3 dir = cannon.position - position;
			dir.y = 0.0f;
			
			// Spawn big bogeyman
			FastSpawnObject spawnedObject = mSpawnManager.SpawnObject(mSpawnManager.level1.bigBogeyMan, position, Quaternion.LookRotation(dir));
			
			if(spawnedObject) {
				BogeyMan bogeyMan = spawnedObject.GetComponent<BogeyMan>();
			
				if(bogeyMan) {
					bogeyMan.target = cannon;
				}
			}
			
			bigBogeyManSpawnTime = Time.time + bigBogeyManSpawnDelay;
		}
	}
}
