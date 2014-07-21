/// <summary>
/// Time Manager.
/// Time.deltaTime independent Time.timeScale Lerp.
/// Author: Fliperamma | Fabio Paes Pedro
/// </summary>
///
///

using UnityEngine;
using System.Collections;

public class CustomTimeManager : MonoBehaviour
{
	/// <summary>
	/// Editor Mode == true will update the Editor for debugging
	/// </summary>
	public bool editorMode = false;
	// Public variables for Editor mode
	public float customTimeManagerScale = 1f;
	public bool customTimeManagerIsPaused = false;
	public bool customTimeManagerWillPause = false;
	public bool customTimeManagerIsFading = false;
	
	/// <summary>
	/// CustomTimeManager is Paused or not
	/// </summary>
	public static bool IsPaused = false;
	/// <summary>
	/// CustomTimeManager is Fading (from _minScale to _scale or vice-versa)
	/// </summary>
	public static bool IsFading = false;
	/// <summary>
	/// CustomTimeManager will pause after fading (is going from _scale to _minScale)
	/// </summary>
	public static bool WillPause = false;
	
	
	static float _scale = 1f;
	static float _fadeToScaleDifference = 0f;
	static float _scaleToFade = 0f;
	static float _deltaTime = 0f;
	static float _lastTime = 0f;
	static float _maxScale = 3f;
	static float _minScale = 0f;
	static bool _fadeToScaleIsGreater = false;
	static CustomTimeManager _instance;
	void Awake()
	{
		// Forcing editor mode to true on Unity and false on production
		editorMode = Application.isEditor;
		// You don't need mode than one instance of CustomTimeManager, if this is a new instance it will be destroyed
		////////////////////////////////////////
		////////////////////////////////////////
		//
		// Always use CustomTimeManager.METHOD (The methods are static)
		//
		////////////////////////////////////////
		////////////////////////////////////////
		if(_instance != null){
			Debug.LogError("You already have a CustomTimeManager Instance");
			Destroy(this);
		}else{
			_instance = this;
		}
		Scale = Time.timeScale;
		StartCoroutine("UpdateDeltaTime");
	}
	
	
	
	/// <summary>
	/// Time.timeScale independent deltaTime
	/// </summary>
	/// <value>
	/// time.timeScale independent Delta Time
	/// </value>
	public static float DeltaTime
	{
		get{
			return _deltaTime;
		}
	}
	
	/// <summary>
	/// Getter and Setter for the CustomTimeManager Scale (time.timeScale). This will set IsPaused to true automatically if the scale == 0f
	/// </summary>
	/// <value>
	/// Scale (Time.timeScale)
	/// </value>
	public static float Scale
	{
		get{
			return _scale;
		}
		set{
			_scale = value;
			_scale = _scale < _minScale ? _minScale : _scale > _maxScale ? _maxScale : _scale;
			Time.timeScale = _scale;
			IsPaused = _scale <= 0.001f;
			if(IsPaused){
				_scale = 0f;
				WillPause = false;
			}
			if(_instance.editorMode)_instance.UpdateEditor();
		}
	}
	
	/// <summary>
	/// Pause toggle (Changes the "IsPaused" flag from true to false and vice-versa)
	/// </summary>
	/// </param>
	/// <param name='time'>
	/// Time until Pause or Play
	/// </param>
	/// <param name='playScale'>
	/// Play scale.
	/// </param>
	public static void TogglePause(float time = 0f, float playScale = -1f)
	{
		StopStepper();
		// WillPause == true means that a "Pause" was already called: this will make "WillPause" change to false and call "Play" function. 
		// Else just toggle.
		WillPause = WillPause == true ? false : !IsPaused;
		if(WillPause){
			Pause (time);
		}else{
			Play (time, playScale);
		}
	}
	
	static void StopStepper()
	{
		_instance.StopCoroutine("FadeStepper");
	}
	
	/// <summary>
	/// CustomTimeManager Pause
	/// </summary>
	/// <param name='time'>
	/// Fading time ultil Time.timeScale == 0f
	/// </param>
	public static void Pause(float time = 0f)
	{		
		if(Mathf.Approximately(time, 0f)){
			WillPause = false;
			_instance.StopCoroutine("FadeStepper");
			Scale = 0f;
		}else{
			WillPause = true;
			FadeTo(0f, time);
		}
	}
	
	/// <summary>
	/// CustomTimeManager Play 
	/// </summary>
	/// <param name='time'>
	/// Fading time until Time.timeScale == scale param
	/// </param>
	/// <param name='scale'>
	/// Final scale for Time.timeScale
	/// </param>
	public static void Play(float time = 0f, float scale = 1f)
	{
		if(Mathf.Approximately(time, 0f)){
			_instance.StopCoroutine("FadeStepper");
			Scale = scale;
		}else{
			FadeTo(scale, time);
		}
	}
	
	/// <summary>
	/// CustomTimeManager Scale Fading.
	/// </summary>
	/// <param name='scale'>
	/// The final Time.timeScale
	/// </param>
	/// <param name='time'>
	/// The transition time to reach the desired scale
	/// </param>
	public static void FadeTo(float scale, float time)
	{
		_instance.StopCoroutine("FadeStepper");
		_scaleToFade = scale;
		_fadeToScaleDifference = scale-_scale;
		_fadeToScaleIsGreater = _fadeToScaleDifference > 0f;
		float scalePerFrame = _fadeToScaleDifference/time;
		_instance.StartCoroutine("FadeStepper", scalePerFrame);
	}
	
	/// <summary>
	/// Coroutine to fade the Unity's timeScale
	/// </summary>
	IEnumerator FadeStepper(float scalePerFrame)
	{
		bool achieved = false;
		IsFading = true;
		while(achieved == false){
			Scale += scalePerFrame*_deltaTime;
			if(_fadeToScaleIsGreater){
				achieved = _scale >= _scaleToFade;
			}else{
				achieved = _scale <= _scaleToFade;
			}
			yield return null;
		}
		Scale = _scaleToFade;
		IsFading = false;
		WillPause = false;
		if(_instance.editorMode)_instance.UpdateEditor();
	}
	
	/// <summary>
	/// Updating our internal _deltaTime
	/// </summary>
	IEnumerator UpdateDeltaTime()
	{
		while(true){
			float timeSinceStartup = Time.realtimeSinceStartup;
			_deltaTime = timeSinceStartup-_lastTime;
			_lastTime = timeSinceStartup;
			yield return null;
		}
	}
	
	/// <summary>
	/// Updates the public variables for Editor visualization
	/// You can remove this if you want to, it's not necessary at all but you may find it useful for debugging 
	/// </summary>
	void UpdateEditor()
	{
		_instance.customTimeManagerScale = _scale;
		_instance.customTimeManagerIsFading = IsFading;
		_instance.customTimeManagerIsPaused = IsPaused;
		_instance.customTimeManagerWillPause = WillPause;
		
	}
	
}
