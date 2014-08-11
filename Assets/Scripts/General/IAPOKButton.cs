using UnityEngine;
using System.Collections;

public class IAPOKButton : MonoBehaviour
{
	IAPManager iapManager;

	void Start()
	{
		GameObject iapManagerObject = GameObject.FindGameObjectWithTag("IAPManager");
		iapManager = iapManagerObject.GetComponent<IAPManager>();
	}

	void OnClick()
	{
		iapManager.iapState = IAPManager.IAPState.Normal;
		iapManager.SetIAPLayout();
	}
}
