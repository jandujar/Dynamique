using UnityEngine;
using System.Collections;

public class RepositionMenuObject : MonoBehaviour
{
	[SerializeField] float movementSpeed = 1f;
	[SerializeField] bool backButton = false;
	float distance;
	Vector3 newPosition;
	bool enableMovement = false;
	public Vector3 NewPosition { private get { return newPosition; } set { newPosition = value; }}
	public bool EnableMovement { private get { return enableMovement; } set { enableMovement = value; }}

	public void Reposition(Vector3 repositionVector3)
	{
		if (!backButton)
			NewPosition = repositionVector3;
		else
		{
			GameObject cameraObject = GameObject.FindGameObjectWithTag("BackgroundCamera");
			Camera camera = cameraObject.GetComponent<Camera>();
			NewPosition = new Vector3(-1750f * (camera.pixelWidth/camera.pixelHeight), 0f, -1600f);
		}

		EnableMovement = true;
	}

	void Update()
	{
		distance = Vector3.Distance(transform.localPosition, NewPosition);

		if (EnableMovement && distance > 0.01f)
		{
			transform.localPosition = Vector3.Lerp(transform.localPosition, NewPosition, Time.deltaTime * movementSpeed);
			distance = Vector3.Distance(transform.localPosition, NewPosition);
		}
		else if (EnableMovement)
			EnableMovement = false;
	}
}
