using UnityEngine;
using System.Collections;

public class AsteroidFastSpawn : FastSpawnManager
{
	[System.Serializable]
	public class Asteroids
	{
		public FastSpawnObject asteroid1;
		public FastSpawnObject asteroid2;
		public FastSpawnObject asteroid3;
		public FastSpawnObject asteroid4;
	}
	
	public Asteroids asteroids;
}
