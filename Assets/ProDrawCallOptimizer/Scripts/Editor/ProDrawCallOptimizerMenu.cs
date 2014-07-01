/*
  Created by:
  Juan Sebastian Munoz Arango
  naruse@gmail.com
  All rights reserved
 */

namespace ProDrawCall {
	using UnityEngine;
	using UnityEditor;
	using System;
	using System.IO;
	using System.Collections;
	using System.Collections.Generic;

	public sealed class ProDrawCallOptimizerMenu : EditorWindow {
	    private static GUIStyle errorStyle;
	    private static GUIStyle warningStyle;
	    private static GUIStyle normalStyle;
	    private static GUIStyle greenStyle;
	    private static GUIStyle shaderTextStyleGreen;//used for the most used
	    private static GUIStyle shaderTextStyleRed;//used for the not recognized shaders
	    private static GUIStyle smallTextStyle;
	    private static GUIStyle smallTextErrorStyle;

	    private static List<bool> unfoldedObjects;
	    private static List<bool> unfoldedKnownShaders;


	    private int objectsSize = 1;
	    private Atlasser generatedAtlas;

	    private static int selectedMenuOption = 0;
	    private static string[] menuOptions;

	    private static ProDrawCallOptimizerMenu window;
	    [MenuItem("Window/ProDrawCallOptimizer")]
	    private static void Init() {
	        ObjSorter.Initialize();

	        normalStyle = new GUIStyle();

	        errorStyle = new GUIStyle();
	        errorStyle.normal.textColor = Color.red;
	        warningStyle = new GUIStyle();
	        warningStyle.normal.textColor = Color.yellow;
	        warningStyle.fontSize = 8;
	        greenStyle = new GUIStyle();
	        greenStyle.normal.textColor = new Color(0, 0.35f, 0.12f);//dark green

	        shaderTextStyleGreen = new GUIStyle();
	        shaderTextStyleGreen.normal.textColor = new Color(0, 0.35f, 0.12f);//dark green
	        shaderTextStyleGreen.fontSize = 8;
	        shaderTextStyleRed = new GUIStyle();

	        shaderTextStyleRed.normal.textColor = Color.red;
	        shaderTextStyleRed.fontSize = 8;

	        smallTextStyle = new GUIStyle();
	        smallTextStyle.fontSize = 9;

	        smallTextErrorStyle = new GUIStyle();
	        smallTextErrorStyle.normal.textColor = Color.red;
	        smallTextErrorStyle.fontSize = 9;


	        window = (ProDrawCallOptimizerMenu) EditorWindow.GetWindow(typeof(ProDrawCallOptimizerMenu));
	        window.minSize = new Vector2(400, 200);
	        window.Show();

	        unfoldedObjects = new List<bool>();
	        unfoldedObjects.Add(false);
	        unfoldedKnownShaders = new List<bool>();

	        menuOptions = new string[] { "Object", "Custom Shaders" };
	        selectedMenuOption = 0;
	    }

	    private static void ReloadDataStructures() {
	        Init();
	        customAtlasName = "";
	    }

	    private bool NeedToReload() {
	        if(ObjSorter.GetObjs() == null)
	            return true;
	        else
	            return false;
	    }

	    void AdjustArraysWithObjSorter() {
	        if(unfoldedObjects.Count != ObjSorter.GetObjs().Count) {
	            int offset = ObjSorter.GetObjs().Count - unfoldedObjects.Count;
	            bool removing = false;
	            if(offset < 0) {
	                offset *= -1;
	                removing = true;
	            }
	            for(int i = 0; i < (offset < 0 ? offset*-1 : offset); i++) {
	                if(removing) {
	                    unfoldedObjects.RemoveAt(unfoldedObjects.Count-1);
	                } else {
	                    unfoldedObjects.Add(true);
	                }
	            }
	        }
	    }

	    private bool reuseTextures = true;
	    private bool removeObjectsBeforeBaking = true;
	    private Vector2 arraysScrollPos = Vector2.zero;
	    private static string customAtlasName = "";
	    void ObjectsMenuGUI() {
	        objectsSize = ObjSorter.GetTotalSortedObjects();

	        GUI.Label(new Rect(5, 40, 145, 19),"Objects to optimize: " + objectsSize);
	        if(GUI.Button(new Rect(153, 35, 23, 12), "+"))
	            objectsSize++;
	        if(GUI.Button(new Rect(153, 48, 23, 12), "-"))
	            objectsSize--;

	        if(GUI.Button(new Rect(179, 35, 55, 25), "Clear"))
	            EmptyObjsAndTexturesArray();

	        GUI.Label(new Rect(237, 29, window.position.width - 240, 19), "Atlas name(Optional):");
	        customAtlasName = GUI.TextField(new Rect(237, 43, window.position.width-240, 17), customAtlasName);

	        //adds all objects in the scene to the utility
	        if(GUI.Button(new Rect(5, 62, 83, 30), "Add all scene\nObjects")) {
	            EmptyObjsAndTexturesArray();
	            FillArrayWithSelectedObjects(Utils.GetAllObjectsInHierarchy());
	            return;//wait for next frame to recalculate objects
	        }

	        GUI.enabled = (Selection.activeGameObject != null);
	        if(GUI.Button(new Rect(90, 62, 80, 30), "Add selected\nand children")) {
	            GameObject[] selectedGameObjects = Selection.gameObjects;

	            List<GameObject> objsToAdd = new List<GameObject>();
	            for(int i = 0; i < selectedGameObjects.Length; i++) {
	                Transform[] selectedObjs = selectedGameObjects[i].GetComponentsInChildren<Transform>(true);
	                for(int j = 0; j < selectedObjs.Length; j++)
	                    objsToAdd.Add(selectedObjs[j].gameObject);

	            }
	            FillArrayWithSelectedObjects(objsToAdd.ToArray());
	            return;
	        }

	        if(GUI.Button(new Rect(172, 62, 55, 30), "Add\nselected")) {
	            FillArrayWithSelectedObjects(Selection.gameObjects);
	            return;
	        }
	        GUI.enabled = true;

	        reuseTextures = GUI.Toggle(new Rect(228, 60, 100, 20),
	                                   reuseTextures,
	                                   "Reuse Textures");

	        /*EditorGUI.HelpBox(new Rect(237, 75, 133, 30),
	                          "If checked, each time there is an atlas baking process starting all the optimized objects get destroyed, un check this when you want manually to keep track of your optimized objects",
	                          MessageType.Info);*/
	        removeObjectsBeforeBaking = GUI.Toggle(new Rect(228, 75, 172, 30),
	                                               removeObjectsBeforeBaking,
	                                   "Remove atlassed before bake");

	        objectsSize = objectsSize < 1 ? 1 : objectsSize;//no neg size

	        ObjSorter.AdjustArraysSize(objectsSize);
	        ObjSorter.SortObjects();
	        AdjustArraysWithObjSorter();

	        arraysScrollPos = GUI.BeginScrollView(new Rect(0, 100, window.position.width, window.position.height - 138),
	                                              arraysScrollPos,
	                                              new Rect(0,0, window.position.width-20, (ObjSorter.GetTotalSortedObjects() + ObjSorter.GetObjs().Count)*(32.5f)));

	        int drawingPos = 0;
	        for(int i = 0; i < ObjSorter.GetObjs().Count; i++) {
	            string shaderName = (ObjSorter.GetObjs()[i][0] != null && ObjSorter.GetObjs()[i][0].IsCorrectlyAssembled) ? ObjSorter.GetObjs()[i][0].ShaderName : "";
	            bool shaderRecognized = ShaderManager.Instance.ShaderExists(shaderName);

	            bool positionIsAShader = (shaderName != "");
	            string shaderLabel = (i+1).ToString() + ((positionIsAShader) ? ". Shader: " : ". ") + (shaderName == "" ? "Not optimizable: " : shaderName + "." ) + " (" + ObjSorter.GetObjs()[i].Count + ")";
	            unfoldedObjects[i] = EditorGUI.Foldout(new Rect(3, drawingPos*30+(positionIsAShader ? 19 : 24), 300, 15),
	                                                   unfoldedObjects[i],
	                                                   "");
	            GUI.Label(new Rect(20, drawingPos*30+(positionIsAShader ? 19 : 24), 300, 15),
	                      shaderLabel,
	                      (shaderRecognized || !positionIsAShader) ? normalStyle : errorStyle);
	            if(positionIsAShader) {
	                if(shaderRecognized) {
	                    if(ObjSorter.GetObjs()[i].Count > 1) {//array has at least more than one texture.
	                        int aproxAtlasSize = ObjSorter.GetAproxAtlasSize(i, reuseTextures);
	                        string msg = " Aprox Atlas Size: ~(" + aproxAtlasSize + "x" + aproxAtlasSize + ")+" + (Constants.AtlasResizeFactor*100) + "%+";
	                        GUI.Label(new Rect(15, drawingPos * 30 + 33, 300, 10),
	                                  msg + ((aproxAtlasSize > Constants.MaxAtlasSize) ? " TOO BIG!!!" : ""),
	                                  (aproxAtlasSize < Constants.MaxAtlasSize) ? smallTextStyle : smallTextErrorStyle);
	                    } else {
	                        GUI.Label(new Rect(15, drawingPos*30+33, 300, 10),"Not optimizing as there needs to be at least 2 textures to atlas.", warningStyle);
	                    }
	                } else {
	                    GUI.Label(new Rect(15, drawingPos*30+33, 300, 10), "Shader not recognized/supported, add it in the custom shaders tab.", smallTextStyle);
	                }
	            }



	            if(GUI.Button(new Rect(position.width-40, drawingPos*30+23, 23,20),"X")) {
	                if(ObjSorter.GetObjs().Count > 1) {
	                    unfoldedObjects.RemoveAt(i);
	                    ObjSorter.Remove(i);
	                } else {
	                    ObjSorter.GetObjs()[0].Clear();
	                    ObjSorter.GetObjs()[0].Add(null);
	                }
	                return;
	            }
	            drawingPos++;
	            if(unfoldedObjects[i]) {
	                for(int j = 0; j < ObjSorter.GetObjs()[i].Count; j++) {
	                    GUI.Label(new Rect(20, drawingPos*30+20 + 6, 30, 25), (j+1).ToString() +":");
	                    GameObject testObj = (GameObject) EditorGUI.ObjectField(new Rect(41, drawingPos*30 + 24, 105, 17),
	                                                                            "",
	                                                                            (ObjSorter.GetObjs()[i][j] != null) ? ObjSorter.GetObjs()[i][j].GameObj : null,
	                                                                            typeof(GameObject),
	                                                                            true);
	                    //dont let repeated game objects get inserted in the list.
	                    if(testObj != null) {
	                        if(ObjSorter.GetObjs()[i][j] == null ||
	                           testObj.GetInstanceID() != ObjSorter.GetObjs()[i][j].GameObj.GetInstanceID()) {
	                            if(!ObjectRepeated(testObj))
	                                ObjSorter.GetObjs()[i][j] = new OptimizableObject(testObj);
	                            else
	                                Debug.LogWarning("Game Object " + testObj.name + " is already in the list.");
	                        }
	                    }
	                    if(ObjSorter.GetObjs()[i][j] != null) {
	                        if(ObjSorter.GetObjs()[i][j].GameObj != null) {
	                            if(ObjSorter.GetObjs()[i][j].IsCorrectlyAssembled) {
	                                if(ObjSorter.GetObjs()[i][j].MainTexture != null) {
	                                    EditorGUI.DrawPreviewTexture(new Rect(170, drawingPos*30+18, 25, 25),
	                                                                 ObjSorter.GetObjs()[i][j].MainTexture,
	                                                                 null,
	                                                                 ScaleMode.StretchToFill);

	                                    GUI.Label(new Rect(198,drawingPos*30 + 24, 105, 25),
	                                              ((ObjSorter.GetObjs()[i][j].ObjHasMoreThanOneMaterial)?"~":"")+
	                                              "(" + ObjSorter.GetObjs()[i][j].TextureSize.x +
	                                              "x" +
	                                              ObjSorter.GetObjs()[i][j].TextureSize.y + ")" +
	                                              ((ObjSorter.GetObjs()[i][j].ObjHasMoreThanOneMaterial)? "+":""));
	                                } else {
	                                    GUI.Label(new Rect(178, drawingPos*30 + 16, 85, 25),
	                                              ((ObjSorter.GetObjs()[i][j].ObjHasMoreThanOneMaterial)? "Aprox":"null"));
	                                    GUI.Label(new Rect(170,drawingPos*30 + 28, 85, 25),
	                                              "(" + ObjSorter.GetObjs()[i][j].TextureSize.x +
	                                              "x" +
	                                              ObjSorter.GetObjs()[i][j].TextureSize.y + ")" +
	                                              ((ObjSorter.GetObjs()[i][j].ObjHasMoreThanOneMaterial)? "+":""));
	                                    GUI.Label(new Rect(257,drawingPos*30 + 17, 125, 20), "No texture found;\ncreating a texture\nwith the color", warningStyle);
	                                }
	                                if(ObjSorter.GetObjs()[i][j].ObjHasMoreThanOneMaterial) {
	                                    GUI.Label(new Rect(330, drawingPos*30 + 17, 59, 30), " Multiple\nMaterials");
	                                }
	                            } else {//obj not correctly assembled, display log
	                                GUI.Label(new Rect(170, drawingPos*30 + 18, 125, 14), ObjSorter.GetObjs()[i][j].IntegrityLog[0], errorStyle);
	                                GUI.Label(new Rect(170, drawingPos*30 + 28, 125, 20), ObjSorter.GetObjs()[i][j].IntegrityLog[1], errorStyle);
	                            }
	                        } else {
	                            ObjSorter.RemoveAtPosition(i, j);
	                        }
	                    }
	                    if(GUI.Button(new Rect(150, drawingPos*30+20, 18,22), "-")) {
	                        if(ObjSorter.GetTotalSortedObjects() > 1) {
	                            ObjSorter.GetObjs()[i].RemoveAt(j);
	                        } else {
	                            ObjSorter.GetObjs()[0][0] = null;
	                        }
	                    }
	                    drawingPos++;
	                }
	            }
	        }
	        GUI.EndScrollView();
	    }


	    void AdjustFoldedArraySizeWithShaderManager() {
	        if(unfoldedKnownShaders.Count != ShaderManager.Instance.CustomShaders.Count) {
	            int offset = ShaderManager.Instance.CustomShaders.Count - unfoldedKnownShaders.Count;
	            bool removing = false;
	            if(offset < 0) {
	                offset *= -1;
	                removing = true;
	            }
	            for(int i = 0; i < (offset < 0 ? offset*-1 : offset); i++) {
	                if(removing)
	                    unfoldedKnownShaders.RemoveAt(unfoldedKnownShaders.Count-1);
	                else
	                    unfoldedKnownShaders.Add(true);
	            }
	        }
	    }

	    private Vector2 arraysShadersScrollPos = Vector2.zero;
	    void CustomShadersGUI() {

	        if(GUI.Button(new Rect(5,35, 80, 28), "Add Custom\nShader")) {
	            ShaderManager.Instance.CustomShaders.Insert(0,"");
	            ShaderManager.Instance.CustomShadersTexturesDefines.Insert(0,new List<string>());
	            ShaderManager.Instance.CustomShadersTexturesDefines[0].Add("");
	        }
	        if(GUI.Button(new Rect(window.position.width/2 - 46,35, 85, 28), "Save Custom\nShaders")) {
	            ShaderManager.Instance.SaveCustomShaders();
	        }
	        if(GUI.Button(new Rect(window.position.width - 100,35, 95, 28), "Reload Custom\nShader")) {
	            ShaderManager.Instance.LoadCustomShaders();
	        }

	        AdjustFoldedArraySizeWithShaderManager();
	        arraysShadersScrollPos = GUI.BeginScrollView(new Rect(0, 90, window.position.width, window.position.height - 127),
	                                                     arraysShadersScrollPos,
	                                                     new Rect(0,0, window.position.width - 20, (ShaderManager.Instance.GetTotalShaderDefines() + ShaderManager.Instance.CustomShaders.Count)*(32.5f)));

	            int drawingPos = 0;
	            for(int i = 0; i < ShaderManager.Instance.CustomShaders.Count; i++) {
	                unfoldedKnownShaders[i] = EditorGUI.Foldout(new Rect(3, drawingPos*30+25, 10, 15),
	                                                unfoldedKnownShaders[i],
	                                                (i+1).ToString() + ". ");
	                ShaderManager.Instance.CustomShaders[i] = EditorGUI.TextField(new Rect(32, drawingPos*30+27, 260, 15),
	                                                                              ShaderManager.Instance.CustomShaders[i]);

	                if(GUI.Button(new Rect(position.width-40, drawingPos*30+26, 24,15),"X")) {
	                    ShaderManager.Instance.CustomShaders.RemoveAt(i);
	                    ShaderManager.Instance.CustomShadersTexturesDefines.RemoveAt(i);
	                    return;//return to recalculate the unfolded array.
	                }
	                drawingPos++;
	                if(unfoldedKnownShaders[i]) {
	                    for(int j = 0; j < ShaderManager.Instance.CustomShadersTexturesDefines[i].Count; j++) {
	                        GUI.Label(new Rect(20, drawingPos*30+20 + 6, 30, 25), (j+1).ToString() +":");
	                        ShaderManager.Instance.CustomShadersTexturesDefines[i][j] = EditorGUI.TextField(new Rect(41, drawingPos*30 + 24, 200, 17),
	                                                                                                        ShaderManager.Instance.CustomShadersTexturesDefines[i][j]).Replace(" ", string.Empty);

	                        if(GUI.Button(new Rect(250, drawingPos*30+20, 18,22), "-")) {
	                            if(ShaderManager.Instance.CustomShadersTexturesDefines[i].Count > 1) {
	                                ShaderManager.Instance.CustomShadersTexturesDefines[i].RemoveAt(j);
	                            } else {
	                                ShaderManager.Instance.CustomShadersTexturesDefines.RemoveAt(i);
	                                ShaderManager.Instance.CustomShaders.RemoveAt(i);
	                            }
	                            return;
	                        }
	                        if(j == ShaderManager.Instance.CustomShadersTexturesDefines[i].Count-1) {
	                            if(GUI.Button(new Rect(273, drawingPos*30+20, 22,22), "+")) {
	                                ShaderManager.Instance.CustomShadersTexturesDefines[i].Add("");
	                            }
	                        }
	                        drawingPos++;
	                    }
	                }
	            }
	        GUI.EndScrollView();
	    }


	    void OnGUI() {
	        if(NeedToReload())
	            ReloadDataStructures();

	        selectedMenuOption = GUI.SelectionGrid(new Rect(5,8,window.position.width-10, 20), selectedMenuOption, menuOptions, 2);

	        switch(selectedMenuOption) {
	            case 0:
	                ObjectsMenuGUI();
	                break;
	            case 1:
	                CustomShadersGUI();
	                break;
	            default:
	                Debug.LogError("Unrecognized menu option: " + selectedMenuOption);
	                break;
	        }

	        if(GUI.Button(new Rect(5, window.position.height - 35, window.position.width/2 - 10, 33), "Clear Atlas")) {
	            GameObject[] objsInHierarchy = Utils.GetAllObjectsInHierarchy();
	            foreach(GameObject obj in objsInHierarchy) {
	                if(obj.name.Contains(Constants.OptimizedObjIdentifier)) {
	                    DestroyImmediate(obj);
	                } else {
	                    if(obj.GetComponent<MeshRenderer>() != null)
	                        obj.GetComponent<MeshRenderer>().enabled = true;
	                }
	            }

	            // delete the folder where the atlas reside.
	            string folderOfAtlas = EditorApplication.currentScene;
				if(folderOfAtlas == "") { //scene is not saved yet.
					folderOfAtlas = Constants.NonSavedSceneFolderName + ".unity";
					Debug.LogWarning("WARNING: Scene has not been saved, clearing baked objects from NOT_SAVED_SCENE folder");
				}
	            folderOfAtlas = folderOfAtlas.Substring(0, folderOfAtlas.Length-6) + "-Atlas";//remove the ".unity"
	            if(Directory.Exists(folderOfAtlas)) {
	                FileUtil.DeleteFileOrDirectory(folderOfAtlas);
	                AssetDatabase.Refresh();
	            }

	        }

	        GUI.enabled = CheckEmptyArray(); //if there are no textures deactivate the GUI
	        if(GUI.Button(new Rect(window.position.width/2 , window.position.height - 35, window.position.width/2 - 5, 33), "Bake Atlas")) {

	            //Remove objects that are already optimized and start over.
	            if(removeObjectsBeforeBaking) {
	                GameObject[] objsInHierarchy = Utils.GetAllObjectsInHierarchy();
	                foreach(GameObject obj in objsInHierarchy) {
	                    if(obj.name.Contains(Constants.OptimizedObjIdentifier))
	                        GameObject.DestroyImmediate(obj);
	                }
	            }


	            List<Rect> texturePositions = new List<Rect>();//creo que esto puede morir porque el atlasser tiene adentro un rect.
	            string progressBarInfo = "";
	            float pace = 1/(float)ObjSorter.GetRecognizableShadersCount();
	            float progress = pace;

	            Node resultNode = null;//nodes for the tree for atlasing
	            for(int shaderIndex = 0; shaderIndex < ObjSorter.GetObjs().Count; shaderIndex++) {
	                EditorUtility.DisplayProgressBar("Optimization in progress...", progressBarInfo, progress);
	                progress += pace;

	                texturePositions.Clear();
	                TextureReuseManager textureReuseManager = new TextureReuseManager();

	                string shaderToAtlas = (ObjSorter.GetObjs()[shaderIndex][0] != null && ObjSorter.GetObjs()[shaderIndex][0].IsCorrectlyAssembled) ? ObjSorter.GetObjs()[shaderIndex][0].ShaderName : "";
	                progressBarInfo = "Processing shader " + shaderToAtlas + "...";
	                int atlasSize = ObjSorter.GetAproxAtlasSize(shaderIndex, reuseTextures);
	                if(ShaderManager.Instance.ShaderExists(shaderToAtlas) && ObjSorter.GetObjs()[shaderIndex].Count > 1 &&
	                   atlasSize < Constants.MaxAtlasSize) {//check the generated atlas size doesnt exceed max supported texture size
	                    generatedAtlas = new Atlasser(atlasSize, atlasSize);
	                    int resizeTimes = 1;
	                    for(int j = ObjSorter.GetObjs()[shaderIndex].Count-1; j >= 0; j--) {//start from the largest to the shortest textures
	                        //before atlassing multiple materials obj, combine it.
	                        if(ObjSorter.GetObjs()[shaderIndex][j].ObjHasMoreThanOneMaterial) {
	                            progressBarInfo = "Combining materials...";
	                            ObjSorter.GetObjs()[shaderIndex][j].ProcessAndCombineMaterials();
	                        }


	                        Vector2 textureToAtlasSize = ObjSorter.GetObjs()[shaderIndex][j].TextureSize;

	                        if(reuseTextures) {
	                            //if texture is not registered already
	                            if(!textureReuseManager.TextureRefExists(ObjSorter.GetObjs()[shaderIndex][j])) {
	                                //generate a node
	                                resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
	                                                                   Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
	                                if(resultNode != null) { //save node if fits in atlas
	                                    textureReuseManager.AddTextureRef(ObjSorter.GetObjs()[shaderIndex][j], resultNode.NodeRect, j);
	                                }
	                            }
	                        } else {
	                            resultNode = generatedAtlas.Insert(Mathf.RoundToInt((textureToAtlasSize.x != Constants.NULLV2.x) ? textureToAtlasSize.x : Constants.NullTextureSize),
	                                                               Mathf.RoundToInt((textureToAtlasSize.y != Constants.NULLV2.y) ? textureToAtlasSize.y : Constants.NullTextureSize));
	                        }

	                        if(resultNode == null) {
	                            int resizedAtlasSize = atlasSize + Mathf.RoundToInt((float)atlasSize * Constants.AtlasResizeFactor * resizeTimes);
	                            generatedAtlas = new Atlasser(resizedAtlasSize, resizedAtlasSize);
	                            j = ObjSorter.GetObjs()[shaderIndex].Count;//Count and not .Count-1 bc at the end of the loop it will be substracted j-- and we want to start from Count-1

	                            texturePositions.Clear();
	                            textureReuseManager.ClearTextureRefs();
	                            resizeTimes++;
	                        } else {
	                            if(reuseTextures) {
	                                texturePositions.Add(textureReuseManager.GetTextureRefPosition(ObjSorter.GetObjs()[shaderIndex][j]));
	                            } else {
	                                texturePositions.Add(resultNode.NodeRect);//save the texture rectangle
	                            }
	                        }
	                    }

	                    progressBarInfo = "Saving textures to atlas...";
	                    Material atlasMaterial = CreateAtlasMaterialAndTexture(shaderToAtlas, shaderIndex, textureReuseManager);
	                    progressBarInfo = "Remapping coordinates...";

	                    ObjSorter.OptimizeDrawCalls(ref atlasMaterial,
	                                                shaderIndex,
	                                                generatedAtlas.GetAtlasSize().x,
	                                                generatedAtlas.GetAtlasSize().y,
	                                                texturePositions,
	                                                reuseTextures,
	                                                textureReuseManager);
	                }
	            }

	            //after the game object has been organized, remove the combined game objects.
	            for(int shaderIndex = 0; shaderIndex < ObjSorter.GetObjs().Count; shaderIndex++) {
	                for(int j = ObjSorter.GetObjs()[shaderIndex].Count-1; j >= 0; j--) {
	                    if(ObjSorter.GetObjs()[shaderIndex][j].ObjWasCombined)
	                        ObjSorter.GetObjs()[shaderIndex][j].ClearCombinedObject();
	                }
	            }

	            EditorUtility.ClearProgressBar();
	            AssetDatabase.Refresh();//reimport the created atlases so they get displayed in the editor.
	        }
	    }

	    private Material CreateAtlasMaterialAndTexture(string shaderToAtlas, int shaderIndex, TextureReuseManager textureReuseManager) {
	        string fileName = ((customAtlasName == "") ? "Atlas " : (customAtlasName + " ")) + shaderToAtlas.Replace('/','_');
	        string folderToSaveAssets = EditorApplication.currentScene;
			if(folderToSaveAssets == "") { //scene is not saved yet.
				folderToSaveAssets = Constants.NonSavedSceneFolderName + ".unity";
				Debug.LogWarning("WARNING: Scene has not been saved, saving baked objects to: NOT_SAVED_SCENE folder");
			}
	        folderToSaveAssets = folderToSaveAssets.Substring(0, folderToSaveAssets.Length-6) + "-Atlas";//remove the ".unity"
	        if(!Directory.Exists(folderToSaveAssets)) {
	            Directory.CreateDirectory(folderToSaveAssets);
	            AssetDatabase.Refresh();
	        }

	        string atlasTexturePath = folderToSaveAssets + Path.DirectorySeparatorChar + fileName;
	        //create the material in the project and set the shader material to shaderToAtlas
	        Material atlasMaterial = new Material(Shader.Find(shaderToAtlas));
	        //save the material to the project view
	        AssetDatabase.CreateAsset(atlasMaterial, atlasTexturePath + "Mat.mat");
	        AssetDatabase.Refresh();
	        //load a reference from the project view to the material (this is done to be able to set the texture to the material in the project view)
	        atlasMaterial = (Material) AssetDatabase.LoadAssetAtPath(atlasTexturePath + "Mat.mat", typeof(Material));

	        List<string> shaderDefines = ShaderManager.Instance.GetShaderTexturesDefines(shaderToAtlas);
	        for(int k = 0; k < shaderDefines.Count; k++) {//go trough each property of the shader.
	            List<Texture2D> texturesOfShader = ObjSorter.GetTexturesToAtlasForShaderDefine(shaderIndex, shaderDefines[k]);//Get thtextures for the property shderDefines[k] to atlas them
	            List<Vector2> scales = ObjSorter.GetScalesToAtlasForShaderDefine(shaderIndex, shaderDefines[k]);
	            List<Vector2> offsets = ObjSorter.GetOffsetsToAtlasForShaderDefine(shaderIndex, shaderDefines[k]);
	            if(reuseTextures) {
	                texturesOfShader = Utils.FilterTexsByIndex(texturesOfShader, textureReuseManager.GetTextureIndexes());
	                scales = Utils.FilterVec2ByIndex(scales, textureReuseManager.GetTextureIndexes());
	                offsets = Utils.FilterVec2ByIndex(offsets, textureReuseManager.GetTextureIndexes());
	            }

	            generatedAtlas.SaveAtlasToFile(atlasTexturePath + k.ToString() + ".png", texturesOfShader, scales, offsets);//save the atlas with the retrieved textures
	            AssetDatabase.Refresh();
	            Texture2D tex = (Texture2D) AssetDatabase.LoadAssetAtPath(atlasTexturePath + k.ToString() + ".png", typeof(Texture2D));

	            atlasMaterial.SetTexture(shaderDefines[k], //set property shderDefines[k] for shader shaderToAtlas
	                                     tex);
	        }
	        return atlasMaterial;
	    }

	    //checks if a gameObject is already in the list.
	    private bool ObjectRepeated(GameObject g) {
	        if(g == null)
	            return false;
	        int instanceID = g.GetInstanceID();
	        for(int i = 0; i < ObjSorter.GetObjs().Count; i++) {
	            for(int j = 0; j < ObjSorter.GetObjs()[i].Count; j++) {
	                if(ObjSorter.GetObjs()[i][j] != null && instanceID == ObjSorter.GetObjs()[i][j].GameObj.GetInstanceID())
	                    return true;
	            }
	        }
	        return false;
	    }

	    private bool CheckEmptyArray() {
	        for(int i = 0; i < ObjSorter.GetObjs().Count; i++)
	            if(ObjSorter.GetObjs()[i].Count > 1)//check that at least there are 2 objects (regardless if tex are null)
	                return true;
	        return false;
	    }

	    void OnInspectorUpdate() {
	        Repaint();
	    }

	    private void EmptyObjsAndTexturesArray() {
	        objectsSize = 1;
	        ObjSorter.AdjustArraysSize(objectsSize);
	        for(int i = 0; i < ObjSorter.GetObjs().Count; i++) {
	            for(int j = 0; j < ObjSorter.GetObjs()[i].Count; j++) {
	                ObjSorter.GetObjs()[i][j] = null;
	            }
	            ObjSorter.GetObjs()[i].Clear();
	        }
	    }

	    private void OnDidOpenScene() {
	        //unfold all the objects to automatically clear objs from other scenes
	        for(int i = 0; i < unfoldedObjects.Count; i++)
	            unfoldedObjects[i] = true;

	    }

	    //Fills the array of textures with the selected objects in the hierarchy view
	    //adds to the end all the objects.
	    private void FillArrayWithSelectedObjects(GameObject[] arr) {
	        //dont include already optimized objects
	        List<GameObject> filteredArray = new List<GameObject>();
	        for(int i = 0; i < arr.Length; i++)
	            if(!arr[i].name.Contains(Constants.OptimizedObjIdentifier))
	                filteredArray.Add(arr[i]);
	            else
	                Debug.LogWarning("Skipping " + arr[i].name + " game object as is already optimized.");


	        bool filledTexture = false;
	        for(int i = 0; i < filteredArray.Count; i++) {
	            filledTexture = false;
	            for(int j = 0; j < ObjSorter.GetObjs().Count; j++) {
	                for(int k = 0; k < ObjSorter.GetObjs()[j].Count; k++) {
	                    if(ObjSorter.GetObjs()[j][k] == null) {
	                        if(!ObjectRepeated(filteredArray[i])) {
	                            ObjSorter.GetObjs()[j][k] = new OptimizableObject(filteredArray[i]);
	                            filledTexture = true;
	                            break;
	                        } else {
	                            Debug.LogWarning("Game Object " + filteredArray[i].name + " is already in the list.");
	                        }
	                    }
	                }
	                if(filledTexture)
	                    break;
	            }
	            //if we didnt find an empty spot in the array, lets just add it to the texture list.
	            if(!filledTexture) {
	                if(!ObjectRepeated(filteredArray[i])) {
	                    ObjSorter.AddObject(filteredArray[i]);//adds also null internally to increase space for textures
	                    filledTexture = true;
	                    objectsSize++;
	                } else {
	                    Debug.LogWarning("Game Object " + filteredArray[i].name + " is already in the list.");
	                }
	            }
	        }
	    }
	}
}