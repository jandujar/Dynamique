using UnityEngine;
using System.Collections;

public class IAPPurchaseButton : MonoBehaviour
{
	[SerializeField] string productIdentifier;
	IAPManager iapManager;

	void Start()
	{
		GameObject iapManagerObject = GameObject.FindGameObjectWithTag("IAPManager");
		iapManager = iapManagerObject.GetComponent<IAPManager>();
	}

	void OnClick()
	{
		iapManager.InitiatePurchase(productIdentifier);
	}
}
