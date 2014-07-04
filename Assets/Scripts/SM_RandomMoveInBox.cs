using UnityEngine;
using System.Collections;

public class SM_RandomMoveInBox : MonoBehaviour
{
	[SerializeField] float xspeed = 1f;
	[SerializeField] float yspeed = 1.5f;
	[SerializeField] float zspeed = 2f;
	
	[SerializeField] float speedDeviation = 0f;
	
	[SerializeField] float xDim = 0.3f;
	[SerializeField] float yDim = 0.3f;
	[SerializeField] float zDim = 0.3f;

	void Start()
	{
		transform.localPosition = new Vector3(0, 0, 0);
		
		xspeed += (Random.Range(-1f, 1f) * speedDeviation);
		yspeed += (Random.Range(-1f, 1f) * speedDeviation);
		zspeed += (Random.Range(-1f, 1f) * speedDeviation);
	}

	void Update()
	{
		transform.Translate(new Vector3(xspeed,yspeed,zspeed) * Time.deltaTime);
		
		if(transform.localPosition.x > xDim)
		{
			xspeed =- (Mathf.Abs(xspeed));
		}
		
		if(transform.localPosition.x < -xDim)
		{
			xspeed = Mathf.Abs(xspeed);
		}
		
		if(transform.localPosition.y > yDim)
		{
			yspeed =- (Mathf.Abs(yspeed));
		}
		
		if(transform.localPosition.y < -yDim)
		{
			yspeed = Mathf.Abs(yspeed);
		}
		
		if(transform.localPosition.z > zDim)
		{
			zspeed =- (Mathf.Abs(zspeed));
		}
		
		if(transform.localPosition.z < -zDim)
		{
			zspeed = Mathf.Abs(zspeed);
		}
	}
}
