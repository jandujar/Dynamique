using UnityEngine;
using System.Collections;

public class CollectibleSpawner : MonoBehaviour
{
	[SerializeField] Collectible collectible;

	public void Spawn()
	{
		Instantiate(collectible, transform.position, transform.rotation);
	}
}
