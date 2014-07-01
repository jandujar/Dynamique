#if UNITY_EDITOR
namespace ProDrawCall {
	using UnityEngine;
	using UnityEditor;
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;

	/*
	  ported from:
	  http://clb.demon.fi/projects/rectangle-bin-packing

	  For more information, see
	  - Rectangle packing: http://www.gamedev.net/community/forums/topic.asp?topic_id=392413
	  - Packing lightmaps: http://www.blackpawn.com/texts/lightmaps/default.html


	  Juan Sebastian Munoz Arango
	  naruse@gmail.com
	  */

	public class Atlasser {
	    private Node root;
	    private int atlasWidth;
	    public int AtlasWidth { get { return atlasWidth; } }
	    private int atlasHeight;
	    public int AtlasHeight { get { return atlasHeight; } }
	    private List<Rect> texturePositions;
	    public List<Rect> TexturePositions { get { return texturePositions; } }

	    public Atlasser(int width, int height) {
	        texturePositions = new List<Rect>();
	        atlasWidth = 0;
	        atlasHeight = 0;
	        root = new Node();
	        root.left = root.right = null;
	        root.NodeRect = new Rect(0,0,width, height);
	    }

	    // Inserts a new texture of the given size into the atlas.
		// Running time is linear to the number of rectangles that have been already packed.
	    // returns the node that stores the newly added rectangle, or 0 if it didnt fit.
	    public Node Insert(int width, int height) {
	        Node n = Insert(root, width, height);
	        if(n == null) {
	            //Debug.Log("Couldnt insert " + t.name + " to atlas.");
	            return null;
	        }
	        texturePositions.Add(n.NodeRect);
			return n;
	    }

	    // returns A value [0, 1] denoting the ratio of total surface area that is in use.
	    // 0.0f - the atlas is totally empty, 1.0f - the atlas is full.
	    private float Occupancy() {
	        int totalSurfaceArea = atlasWidth * atlasHeight;
	        uint usedSurfaceArea = UsedSurfaceArea(root);
	        return ((float)usedSurfaceArea/(float)totalSurfaceArea);
	    }

	    public void PrintAtlasInfo() {
	        Debug.Log("Atlas Size: " + atlasWidth +"x" + atlasHeight + " Atlas occupancy: " + (100*Occupancy()).ToString("#.##")+"%");
	    }

	    public Vector2 GetAtlasSize() {
	        return new Vector2(atlasWidth, atlasHeight);
	    }


	    //list of saved states depending if textures are readable or not.
	    //pd: has to be outside, else it gets reseted when importing the assets..
	    private List<bool> isTextureReadableList = new List<bool>();
	    public void SaveAtlasToFile(string pathOfAtlas, List<Texture2D> texturesOfMaterial, List<Vector2> scales, List<Vector2> offsets) {
	        isTextureReadableList.Clear();
	        for(int i = 0; i < texturesOfMaterial.Count; i++) {
	            //check if the texture is readable or not, in case is not readable then set it temporarly to readable in order to get the pixels.
	            string texturePath = AssetDatabase.GetAssetPath(texturesOfMaterial[i]);
	            if(texturePath != "") {//check that the texture is not a generated texture.
	                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);
	                isTextureReadableList.Add(textureImporter.isReadable);
	                if(!textureImporter.isReadable) {
	                    textureImporter.isReadable = true;
	                    AssetDatabase.ImportAsset(texturePath);
	                }
	            } else {
	                isTextureReadableList.Add(true);//texture is a generated texture, no need to reimport
	            }
	        }

	        Texture2D canvas = new Texture2D(atlasWidth, atlasHeight);

	        for(int i = 0; i < texturesOfMaterial.Count; i++) {
	            Texture2D texToProcess = texturesOfMaterial[i];

	            //check that the texture has the same size than the first catched texture when atlasing (Normally is _MainTex)
	            //if the texture size that we are processing is not the same size of the atlas, then we have to resize the texture.
	            if(texToProcess.width != Mathf.RoundToInt(texturePositions[i].width) ||
	               texToProcess.height != Mathf.RoundToInt(texturePositions[i].height)) {
	                texToProcess = Utils.CopyTexture(texturesOfMaterial[i], TextureFormat.ARGB32);


	                Debug.LogWarning("Resizing texture: " + texturesOfMaterial[i].name + " from " +
	                                 texToProcess.width + "x" + texToProcess.height + " to " +
	                                 Mathf.RoundToInt(texturePositions[i].width) + "x" + Mathf.RoundToInt(texturePositions[i].height) +
	                                 " In order to make it fit in the atlas.");
	                TextureScale.Point(texToProcess, Mathf.RoundToInt(texturePositions[i].width), Mathf.RoundToInt(texturePositions[i].height));
	            }

	            texToProcess = TileManager.TileTexture(texToProcess, scales[i], offsets[i]);

	            Color[] texturePixels = texToProcess.GetPixels();

	            canvas.SetPixels(Mathf.RoundToInt(texturePositions[i].x),
	                             Mathf.RoundToInt(texturePositions[i].y),
	                             Mathf.RoundToInt(texturePositions[i].width),
	                             Mathf.RoundToInt(texturePositions[i].height),
	                             texturePixels);
	        }
	        canvas.Apply();
	        // Encode texture into PNG
	        byte[] bytes = canvas.EncodeToPNG();
	        File.WriteAllBytes(pathOfAtlas, bytes);

	        //Debug.Log("Saving atlas to: " + name);

	        //after finishing all the reading of the textures, revert the readable/not readable state of each of the textures processed.
	        for(int i = 0; i < isTextureReadableList.Count; i++) {
	            //set the texture as not readable
	            if(!isTextureReadableList[i]) {
	                string texturePath = AssetDatabase.GetAssetPath(texturesOfMaterial[i]);
	                TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);
	                textureImporter.isReadable = false;
	                AssetDatabase.ImportAsset(texturePath);
	            }
	        }
	    }

	    private uint UsedSurfaceArea(Node node) {
	        if (node.left != null || node.right != null) {
	            uint usedSurfaceArea = (uint) (node.NodeRect.width * node.NodeRect.height);
	            if (node.left != null)
	                usedSurfaceArea += UsedSurfaceArea(node.left);
	            if (node.right != null)
	                usedSurfaceArea += UsedSurfaceArea(node.right);

	            return usedSurfaceArea;
	        }
	        // This is a leaf node, it doesn't constitute to the total surface area.
	        return 0;
	    }

	    //Running time is linear to the number of rectangles already packed. Recursively calls itself.
	    //returns null If the insertion didn't succeed.
	    private Node Insert(Node node, int width, int height) {
	        // If this node is an internal node, try both leaves for possible space.
	        // (The rectangle in an internal node stores used space, the leaves store free space)
	        if (node.left != null || node.right != null) {
	            if (node.left != null) {
	                Node newNode = Insert(node.left, width, height);
	                if (newNode != null)
	                    return newNode;
	            }
	            if (node.right != null) {
	                Node newNode = Insert(node.right, width, height);
	                if (newNode != null)
	                    return newNode;
	            }
	            return null; // Didn't fit into either subtree!
	        }

	        // This node is a leaf, but can we fit the new rectangle here?
	        if (width > node.NodeRect.width || height > node.NodeRect.height)
	            return null; // Too bad, no space.

	        // The new cell will fit, split the remaining space along the shorter axis,
	        // that is probably more optimal.
	        int w = (int)node.NodeRect.width - width;
	        int h = (int)node.NodeRect.height - height;
	        node.left = new Node();
	        node.right = new Node();
	        if (w <= h) { // Split the remaining space in horizontal direction.
	            node.left.NodeRect = new Rect(node.NodeRect.x + width, node.NodeRect.y, w, height);

	            node.right.NodeRect = new Rect(node.NodeRect.x, node.NodeRect.y + height, node.NodeRect.width, h);
	        } else { // Split the remaining space in vertical direction.
	            node.left.NodeRect = new Rect(node.NodeRect.x, node.NodeRect.y + height, width, h);

	            node.right.NodeRect = new Rect(node.NodeRect.x + width, node.NodeRect.y, w, node.NodeRect.height);
	        }
	        // Note that as a result of the above, it can happen that node->left or node->right
	        // is now a degenerate (zero area) rectangle. No need to do anything about it,
	        // like remove the nodes as "unnecessary" since they need to exist as children of
	        // this node (this node can't be a leaf anymore).

	        // This node is now a non-leaf, so shrink its area - it now denotes
	        // *occupied* space instead of free space. Its children spawn the resulting
	        // area of free space.
	        node.NodeRect = new Rect(node.NodeRect.x, node.NodeRect.y, width, height);

	        //calculate the texture atlas texture size as it increases
	        if(atlasWidth < node.NodeRect.x + width)
	            atlasWidth = (int)node.NodeRect.x + width;
	        if(atlasHeight < node.NodeRect.y + height)
	            atlasHeight = (int)node.NodeRect.y + height;

	        return node;
	    }
	}
}
#endif