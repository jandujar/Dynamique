using UnityEngine;
using System.Collections;

public class SM_DestroyThisTimed : MonoBehaviour
{
	[SerializeField] float destroyTime = 5f;

	void Start()
	{
		Destroy(gameObject, destroyTime);
	}
}
