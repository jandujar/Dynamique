using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject[] levels;

	void Awake()
	{
		Instantiate(levels[0], transform.position, transform.rotation);
	}
}
