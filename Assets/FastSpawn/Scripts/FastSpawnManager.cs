using UnityEngine;
using System.Collections;
using System.Reflection;
using pmjo.FastSpawn;

public class FastSpawnManager : MonoBehaviour {
	public Transform spawnRoot;
	public bool unspawnObjectWhenInitialized = true;
	public bool suppressDebugMessages = false;
	
	private FastSpawn mFastSpawn;
	
	void Awake() {
		mFastSpawn = new FastSpawn();	
		
		if(mFastSpawn != null && mFastSpawn.Initialize(suppressDebugMessages)) {
			if(Debug.isDebugBuild && !suppressDebugMessages) {
				Debug.Log(mFastSpawn.GetProductName() + " " + mFastSpawn.GetVersion() + " instance initialized.");	
			}	
		}
	}
	
	void OnDestroy() {
		UnloadAllObjects();
	}

	public void LoadObjects(params object[] prefabs) {
		if(mFastSpawn != null) {
			for(int i=0; i<prefabs.Length; i++) {
				if(prefabs[i] != null) {
					// Is it a single item
					if(prefabs[i].GetType().Equals(typeof(FastSpawnObject))) {
						FastSpawnObject prefab = (FastSpawnObject) prefabs[i];
						
						if(prefab != null) {
							mFastSpawn.CreateInstancePool(prefab.transform, prefab.spawnType, prefab.spawnCount, OnNewInstanceCreated);
						}
					}
					// Might be multiple items (use reflection to figure out the children)
					else {
						foreach(FieldInfo fieldInfo in prefabs[i].GetType().GetFields()) {
							object fieldValue = fieldInfo.GetValue(prefabs[i]);
							
							if(fieldValue != null && fieldValue.GetType().Equals(typeof(FastSpawnObject))) {
								FastSpawnObject prefab = (FastSpawnObject) fieldValue;
							
								if(prefab != null) {		
									mFastSpawn.CreateInstancePool(prefab.transform, prefab.spawnType, prefab.spawnCount, OnNewInstanceCreated);									
								}
							}
						}
					}
				}
			}
		}
	}

	public void UnloadObjects(params object[] prefabs) {
		if(mFastSpawn != null) {
			for(int i=0; i<prefabs.Length; i++) {
				if(prefabs[i] != null) {
					// Is it a single item
					if(prefabs[i].GetType().Equals(typeof(FastSpawnObject))) {
						FastSpawnObject prefab = (FastSpawnObject) prefabs[i];
						
						if(prefab != null) {
							mFastSpawn.DeleteInstancePool(prefab.transform);
						}
					}
					// Might be multiple items (use reflection to figure out the children)
					else {
						foreach(FieldInfo fieldInfo in prefabs[i].GetType().GetFields()) {
							object fieldValue = fieldInfo.GetValue(prefabs[i]);
	
							if(fieldValue != null && fieldValue.GetType().Equals(typeof(FastSpawnObject))) {
								FastSpawnObject prefab = (FastSpawnObject) fieldValue;
							
								if(prefab != null) {
									mFastSpawn.DeleteInstancePool(prefab.transform);
								}
							}
						}
					}
				}
			}
		}
	}
	
	public void UnloadAllObjects() {
  		if(mFastSpawn != null) {
			mFastSpawn.DeleteInstancePools();	
		}
	}

	public FastSpawnObject SpawnObject(FastSpawnObject prefab, Vector3 position, Quaternion rotation) {
		FastSpawnObject prefabInstance = null;
		
		if(mFastSpawn != null && prefab != null) {
			Transform instance = mFastSpawn.SpawnObject(prefab.transform, position, rotation);
			
			if(instance) {
				prefabInstance = instance.GetComponent<FastSpawnObject>();

				if(prefabInstance) {
					if(prefabInstance.spawnType == FastSpawn.SpawnType.Always) {
#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
						if(prefabInstance.gameObject.active) {
							prefabInstance.Unspawn();
						}
#else
						if(prefabInstance.gameObject.activeSelf) {
							prefabInstance.Unspawn();	
						}
#endif
					}
	
#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
					prefabInstance.gameObject.active = true;
#else						
					prefabInstance.gameObject.SetActive(true);
#endif

					prefabInstance.Spawn();
				}
			}
		}
		
		return prefabInstance;
	}
	
	public void UnspawnObject(FastSpawnObject prefabInstance) {
		if(mFastSpawn != null & prefabInstance != null && prefabInstance.originalPrefab != null) {
			mFastSpawn.UnspawnObject(prefabInstance.cachedTransform, prefabInstance.originalPrefab.transform);
		
			prefabInstance.Unspawn();

#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
			prefabInstance.gameObject.active = false;
#else						
			prefabInstance.gameObject.SetActive(false);
#endif
		}
	}
	
	private void OnNewInstanceCreated(Transform transform, Transform originalPrefab) {
		if(transform && originalPrefab) {
			FastSpawnObject prefabInstance = transform.GetComponent<FastSpawnObject>();
			
			if(prefabInstance) {
				prefabInstance.Initialize(transform, originalPrefab.GetComponent<FastSpawnObject>());

				if(unspawnObjectWhenInitialized) {
					prefabInstance.Unspawn();	
				}
				
				transform.parent = spawnRoot;

#if UNITY_2_6 || UNITY_2_6_1 || UNITY_3_0 || UNITY_3_0_0 || UNITY_3_1 || UNITY_3_2 || UNITY_3_3 || UNITY_3_4 || UNITY_3_5
				transform.gameObject.active = false;
#else						
				transform.gameObject.SetActive(false);
#endif	
			}
		}
	}
}