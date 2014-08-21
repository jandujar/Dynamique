public class BogeyManSpawnManager : FastSpawnManager {

    [System.Serializable]
    public class Level1 {
        public FastSpawnObject smallBogeyMan;
		public FastSpawnObject mediumBogeyMan;
		public FastSpawnObject bigBogeyMan;
		
		public FastSpawnObject muzzleFlash;
		public FastSpawnObject cannonBallExplosion;
		public FastSpawnObject bogeyManExplosion;
		
		public FastSpawnObject cannonBall;
    }
	
	public Level1 level1;
	
	void Start() {
		// Load all objects on start
    	LoadObjects(level1);
	}
		
	// Singleton
	private static BogeyManSpawnManager sharedInstance;
	
	public static BogeyManSpawnManager SharedInstance {
	    get {
	        if(sharedInstance == null) {
	            sharedInstance = (BogeyManSpawnManager)FindObjectOfType(typeof(BogeyManSpawnManager));
	        }
	
	        return sharedInstance;
	    }
	}
	
	public void OnApplicationQuit() {
	    sharedInstance = null;
	}
}
