using UnityEngine;
using System.Collections;

public class EndPoint : MonoBehaviour
{
	[SerializeField] GameObject[] livingEffects;

	public void StopEffects()
	{
		foreach(GameObject livingEffect in livingEffects)
			livingEffect.gameObject.particleSystem.enableEmission = false;
	}
}
