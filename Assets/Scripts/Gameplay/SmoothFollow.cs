using UnityEngine;
using System.Collections;

public class SmoothFollow : MonoBehaviour
{
	[SerializeField] Transform target;
	float distance = 10.0f;
	float height = 5.0f;
	float heightDamping = 2.0f;
	float rotationDamping = 3.0f;
	
	void LateUpdate()
	{
		// Early out if we don't have a target
		if (!target)
			return;
		
		// Calculate the current rotation angles
		var wantedRotationAngle = target.eulerAngles.y;
		var wantedHeight = target.position.y + height;
		
		var currentRotationAngle = transform.eulerAngles.y;
		var currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
		
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
		
		// Convert the angle into a rotation
		var currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
		
		// Set the height of the camera
		var correctedPositionHeight = transform.position;
		correctedPositionHeight.y = currentHeight;
		transform.position = correctedPositionHeight;
		
		// Always look at the target
		transform.LookAt(target);
	}
}
