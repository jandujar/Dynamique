using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
//621385364

public class AppraterScript : MonoBehaviour {

	[DllImport("__Internal")]
	private static extern void setupKTAppRater(string appid,string title,string message,string button1Title,string button2Title,
		string button3Title,bool shouldAlwaysShow,int days,int gamePlays,bool shouldAutoShow,string objectName);
	[DllImport("__Internal")]
	private static extern void presentRateAlert();
	[DllImport("__Internal")]
	private static extern void openURL();
	
	[SerializeField]
	private AppRaterModel model = new AppRaterModel();

	static AppraterScript sharedInstance = null;
	
	void Awake () {
		if (sharedInstance == null) {
			sharedInstance = this;
			DontDestroyOnLoad (gameObject);
		}
		else {
			Destroy(gameObject);
		}
	}
	
	void Start () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			StartCoroutine(callRater());
		}
	}

	public static AppraterScript SharedController () {
		return sharedInstance;
	}

	public string AppId {
		get {
			return model.Appid;
		}
		set {
			model.Appid = value; 
		}
	}

	IEnumerator callRater () {
		yield return new WaitForSeconds(.3f);
		string thirdButton = null;
		if (model_.IsThirdButton) {
			thirdButton = model_.NeverRemindTitle;
		}
		
		int days = GetDays();
		int gamePlays = GetGamePlays();
		bool shouldShow = false;
		bool shouldAutoShow = false;
		
		if (model_.ShouldAlwaysShow == AppRaterModel.Show.Yes) {
			shouldShow = true;
		}
		if (model_.ShouldAutoShow == AppRaterModel.Show.Yes) {
			shouldAutoShow = true;
		}
		
		setupKTAppRater(""+model_.Appid,model_.ReviewTitle,model_.ReviewMessage,model_.RateNowTitle,model_.RateLaterTitle,
			thirdButton,shouldShow,days,gamePlays,shouldAutoShow,gameObject.name);
	}
	
	int GetDays () {
		int returnValue = 0;
		switch (model_.NumberOfDays) {
			case AppRaterModel.Number.One:
				returnValue = 1;	
				break;
			case AppRaterModel.Number.Two:
				returnValue = 2;	
				break;
			case AppRaterModel.Number.Three:
				returnValue = 3;	
				break;
			case AppRaterModel.Number.Four:
				returnValue = 4;	
				break;
			case AppRaterModel.Number.Five:
				returnValue = 5;	
				break;
			case AppRaterModel.Number.Six:
				returnValue = 6;	
				break;
			case AppRaterModel.Number.Seven:
				returnValue = 7;	
				break;
			case AppRaterModel.Number.Eight:
				returnValue = 8;	
				break;
			case AppRaterModel.Number.Nine:
				returnValue = 9;	
				break;
			case AppRaterModel.Number.Ten:
				returnValue = 10;	
				break;
		}
		return returnValue;
	}
	
	int GetGamePlays () {
		int returnValue = 0;
		switch (model_.NumberOfGamePlays) {
			case AppRaterModel.Number.One:
				returnValue = 1;	
				break;
			case AppRaterModel.Number.Two:
				returnValue = 2;	
				break;
			case AppRaterModel.Number.Three:
				returnValue = 3;	
				break;
			case AppRaterModel.Number.Four:
				returnValue = 4;	
				break;
			case AppRaterModel.Number.Five:
				returnValue = 5;	
				break;
			case AppRaterModel.Number.Six:
				 returnValue= 6;	
				break;
			case AppRaterModel.Number.Seven:
				returnValue = 7;	
				break;
			case AppRaterModel.Number.Eight:
				returnValue = 8;	
				break;
			case AppRaterModel.Number.Nine:
				returnValue = 9;	
				break;
			case AppRaterModel.Number.Ten:
				returnValue = 10;	
				break;
		}
		return returnValue;
	}
	
	public AppRaterModel model_ {
		set {
			model = value;
		}
		get {
			return model;
		}
	}
	
	public static void ShowRaterPopup () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			presentRateAlert();
		}
	}
	public void ShowRaterPopupJS () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			presentRateAlert();
		}
	}

	public static void OpenRateURL () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			openURL();
		}
	}
	public void OpenRateURLJS () {
		if (Application.platform == RuntimePlatform.IPhonePlayer) {
			openURL();
		}
	}

	void ReviewNowPressed (string val) {
		print ("review now");
	}

	void ReviewLaterPressed (string val) {
		print ("review later");
	}

	void NeverRemindPressed (string val) {
		print ("never remind");
	}
}
