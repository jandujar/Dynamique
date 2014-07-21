using UnityEngine;
using System.Collections;

public class DestroyOnCollision : MonoBehaviour
{

	void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.tag == "Object")
			Destroy(collision.gameObject);
	}
}
