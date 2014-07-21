using UnityEngine;
using System.Collections;

public class DeactivateOnAwake : MonoBehaviour
{

	void Awake()
	{
		gameObject.SetActive(false);
	}
}
