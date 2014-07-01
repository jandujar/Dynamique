﻿/*
  Created by Juan Sebastian Munoz
  naruse@gmail.com
  All rights reserved
 */

#if UNITY_EDITOR
namespace ProDrawCall {
	using UnityEngine;
	using System.Collections;

	public static class Constants {

	    //private static string pathToSaveGeneratedAssets = "ProDrawCallOptimizer/ProDrawCallGeneratedAssets/";
	    //public static string PathToSaveAssets { get { return pathToSaveGeneratedAssets; } }

	    private static string pathForCustomShaders = "ProDrawCallOptimizer/CustomShaders.pdo";
	    public static string PathForCustomShaders { get { return pathForCustomShaders; } }


	    private static Vector2 nullV2 = new Vector2(-99, -99);
	    public static Vector2 NULLV2 { get { return nullV2; } }


	    //size of generated textures
	    private static int nullTextureSize = 32; //in px
	    public static int NullTextureSize { get { return nullTextureSize; } }

	    //Max Atlas size, DONT change this value, max texture permited by unity is 10k x 10k so use 9k + ~5% for atlas expansion
	    private static int maxAtlasSize = 9000; //in px
	    public static int MaxAtlasSize { get { return maxAtlasSize; } }

	    private static string optimizedObjIdentif = "=>ODCObj";
	    public static string OptimizedObjIdentifier { get { return optimizedObjIdentif; } }

	    private static int maxNumberOfAtlasses = 5;//max number of atlasses that can be created per shader.
	    public static int MaxNumberOfAtlasses { get { return maxNumberOfAtlasses; } }

	    public static float atlasResizeFactor = 0.025f;//scale in % the atlas will be resized when rectangle nodes dont fit.
	    public static float AtlasResizeFactor { get { return atlasResizeFactor; } }

	    private static int maxTextureSize = 9990;//px
	    public static int MaxTextureSize { get { return maxTextureSize; } }

		private static string nonSavedSceneFolderName = "Assets/NOT_SAVED_SCENE";
		public static string NonSavedSceneFolderName { get { return nonSavedSceneFolderName; } }

	}
}
#endif