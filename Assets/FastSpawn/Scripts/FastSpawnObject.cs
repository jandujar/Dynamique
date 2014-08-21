using UnityEngine;
using System.Collections;
using pmjo.FastSpawn;

public class FastSpawnObject : MonoBehaviour {
	public event FastSpawn.SpawnDelegate OnSpawn;
	public event FastSpawn.UnspawnDelegate OnUnspawn;

	public FastSpawn.SpawnType spawnType = FastSpawn.SpawnType.Always;
	public int spawnCount = 24;

	public Transform cachedTransform { get; private set; }
	public FastSpawnObject originalPrefab { get; private set; }

	public void Initialize(Transform transform, FastSpawnObject prefab) {
		originalPrefab = prefab;
		cachedTransform = transform;
	}
	
	public void Spawn() {	
		if(OnSpawn != null) {
			OnSpawn();
		}
	}
	
	public void Unspawn() {
		if(OnUnspawn != null) {
			OnUnspawn();
		}
	}
}