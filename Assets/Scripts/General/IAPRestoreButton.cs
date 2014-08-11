using UnityEngine;
using System.Collections;

public class IAPRestoreButton : MonoBehaviour
{
	IAPManager iapManager;

	void Start()
	{
		GameObject iapManagerObject = GameObject.FindGameObjectWithTag("IAPManager");
		iapManager = iapManagerObject.GetComponent<IAPManager>();
	}

	void OnClick()
	{
		iapManager.RestorePurchases();
	}
}
