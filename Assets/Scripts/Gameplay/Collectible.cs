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
			gameController.CollectiblesCollected++;
			DestroyStar();
		}
	}

	public void DestroyStar()
	{
		Fabric.EventManager.Instance.PostEvent("SFX_Star", Fabric.EventAction.PlaySound);
		gameObject.rigidbody.velocity = new Vector3(0f, 0f, 0f);
		Instantiate(collectExplosion, transform.position, transform.rotation);
		collider.enabled = false;
		
		foreach(GameObject livingEffect in livingEffects)
			Destroy(livingEffect);
		
		Destroy(gameObject);
	}
}
