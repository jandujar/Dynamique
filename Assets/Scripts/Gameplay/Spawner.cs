using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
	[SerializeField] GameObject objectPrefab;
	[SerializeField] GameObject spawnEffect;
	bool canSpawn = true;

	public void TriggerSpawn(float spawnWaitTime)
	{
		if (canSpawn)
			StartCoroutine(Spawn(spawnWaitTime));
	}

	IEnumerator Spawn(float spawnWaitTime)
	{
		canSpawn = false;
		yield return new WaitForSeconds(spawnWaitTime);
		Fabric.EventManager.Instance.PostEvent("SFX_Spawn", Fabric.EventAction.PlaySound);
		Instantiate(spawnEffect, transform.position, transform.rotation);
		var clone = (GameObject)Instantiate(objectPrefab, transform.position, transform.rotation);
		clone.name = "Object " + Random.Range(0, 1000).ToString();
		canSpawn = true;
	}
}
