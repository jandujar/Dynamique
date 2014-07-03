using UnityEngine;
using System.Collections;

public class RandomRotation : MonoBehaviour
{
	[SerializeField] float minRandom = -0.5f;
	[SerializeField] float maxRandom = 0.5f;

	void Start()
	{
		float xRot = Random.Range(minRandom, maxRandom);
		float yRot = Random.Range(minRandom, maxRandom);
		float zRot = Random.Range(minRandom, maxRandom);
		rigidbody.angularVelocity = new Vector3(xRot, yRot, zRot);
	}
}
