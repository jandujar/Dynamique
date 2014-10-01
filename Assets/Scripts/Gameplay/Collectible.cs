using UnityEngine;
using System.Collections;

public class Collectible : MonoBehaviour
{
	[SerializeField] GameObject collectExplosion;
	[SerializeField] GameObject[] livingEffects;
	GameController gameController;
	bool isTriggered = false;

	void Start()
	{
		var gameControllerObject = GameObject.FindGameObjectWithTag("GameController");
		
		if (gameControllerObject != null)
			gameController = gameControllerObject.GetComponent<GameController>();
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Object" && !isTriggered)
		{
			isTriggered = true;
			Fabric.EventManager.Instance.PostEvent("SFX_Star", Fabric.EventAction.PlaySound);
			gameController.CollectiblesCollected++;
			DestroyStar();
		}
	}

	public void DestroyStar()
	{
		gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);
		Instantiate(collectExplosion, transform.position, transform.rotation);
		collider.enabled = false;
		
		foreach(GameObject livingEffect in livingEffects)
			Destroy(livingEffect);
		
		Destroy(gameObject, 1f);
	}
}
