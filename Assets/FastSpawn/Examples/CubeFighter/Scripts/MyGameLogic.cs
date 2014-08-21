using UnityEngine;
using System.Collections;

public class MyGameLogic : MonoBehaviour {
	
	public MySpawnManager mySpawnManager;
	private float nextSpawnTime = 0.0f;
	
	void Start() {
		mySpawnManager.LoadObjects(mySpawnManager.myPrefabs);
	}
	
	void Update() {
		// Spawn one cube in second if there are instances available (using spawn type IfAvailable)
		if(Time.time > nextSpawnTime) {
			mySpawnManager.SpawnObject(mySpawnManager.myPrefabs.cube, new Vector3(Random.Range(-5, 5), 15.0f, Random.Range(-5, 5)), Quaternion.LookRotation(Random.onUnitSphere));
			nextSpawnTime = Time.time + 1.0f;
		}
		
		// Check if cube is hit by a mouse click
		if(Input.GetMouseButtonDown(0)) {
			CheckIfHitCube(Input.mousePosition);
		}
		
		// Check if cube is hit by a touch
		foreach(Touch touch in Input.touches) {
			if(touch.phase == TouchPhase.Began) {
				CheckIfHitCube(touch.position);
			}
		}
	}
	
	public void CheckIfHitCube(Vector2 screenPosition) {
		Ray ray = Camera.main.ScreenPointToRay(screenPosition);
		RaycastHit hit;

		if(Physics.Raycast(ray, out hit, 64.0f)) {
			FastSpawnObject hitObject = hit.transform.GetComponent<FastSpawnObject>();
			if(hitObject) {
				// Spawn cube explosion to the cube position
				mySpawnManager.SpawnObject(mySpawnManager.myPrefabs.cubeExplosion, hitObject.transform.position, hitObject.transform.rotation);
				
				// Unspawn the cube which got hit
				mySpawnManager.UnspawnObject(hitObject);
			}
		}
	}
}
