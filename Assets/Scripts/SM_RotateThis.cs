using UnityEngine;
using System.Collections;

public class SM_RotateThis : MonoBehaviour
{
	[SerializeField] float rotationSpeedX =90f;
	[SerializeField] float rotationSpeedY = 0f;
	[SerializeField] float rotationSpeedZ = 0f;
	[SerializeField] bool local = true;

	void Update()
	{
		if (local)
			transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime);
		else
			transform.Rotate(new Vector3(rotationSpeedX, rotationSpeedY, rotationSpeedZ) * Time.deltaTime, Space.World);
	}
}
