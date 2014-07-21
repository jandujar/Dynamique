using UnityEngine;
using System.Collections;

public class EndPoint : MonoBehaviour
{
	[SerializeField] GameObject parentEffectObject;
	[SerializeField] GameObject[] livingEffects;

	void Start()
	{
		StartCoroutine(WaitAndActivate());
	}

	IEnumerator WaitAndActivate()
	{
		yield return new WaitForSeconds(0.1f);
		parentEffectObject.SetActive(true);
	}

	public void StopEffects()
	{
		foreach(GameObject livingEffect in livingEffects)
			livingEffect.gameObject.particleSystem.enableEmission = false;
	}
}
