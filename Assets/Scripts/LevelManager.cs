using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
	[SerializeField] GameObject tempLevel;

	void Awake()
	{
		Instantiate(tempLevel, transform.position, transform.rotation);
	}
}
