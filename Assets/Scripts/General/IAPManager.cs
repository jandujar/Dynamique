using UnityEngine;
using System.Collections;

public class IAPManager : MonoBehaviour
{
	//The product identifiers defined in iTunes Connect
	[SerializeField] string[] productIdentifiers;
	GameCenterManager gameCenterManager;
	MenuStateManager menuStateManager;
	TweenAlpha purchaseTween;
	UILabel purchaseLabel;
	GameObject OKButton;
	string restoredIdentifiers;
	
	public enum IAPState {Loading, Normal, Error, Success, Restored};
	public IAPState iapState = IAPState.Normal;
	StoreKitProduct[] products;

	void Start()
	{
		StartCoroutine(WaitAndFind());
		ConfigureStoreKitEvents();
		EasyStoreKit.AssignIdentifiers(productIdentifiers);
		EasyStoreKit.LoadProducts();
	}

	IEnumerator WaitAndFind()
	{
		yield return new WaitForSeconds(0.1f);

		GameObject gameCenterManagerObject = GameObject.FindGameObjectWithTag("GameCenterManager");
		gameCenterManager = gameCenterManagerObject.GetComponent<GameCenterManager>();

		GameObject menuStateManagerObject = GameObject.FindGameObjectWithTag("MenuStateManager");

		if (menuStateManagerObject != null)
			menuStateManager = menuStateManagerObject.GetComponent<MenuStateManager>();
		
		GameObject purchaseTweenObject = GameObject.FindGameObjectWithTag("PurchaseTween");
		purchaseTween = purchaseTweenObject.GetComponent<TweenAlpha>();
		
		GameObject purchaseLabelObject = GameObject.FindGameObjectWithTag("PurchaseLabel");
		purchaseLabel = purchaseLabelObject.GetComponent<UILabel>();
		
		OKButton = GameObject.FindGameObjectWithTag("OKButton");
		
		purchaseLabel.text = "An error occured\nduring the transaction.\nPlease try again.";
	}

	public void SetIAPLayout()
	{
		switch(iapState)
		{
		case IAPState.Normal:
			if (OKButton != null)
			{
				OKButton.SetActive(false);
				purchaseTween.from = purchaseTween.value;
				purchaseTween.to = 0f;
				purchaseTween.duration = 0.5f;
				purchaseTween.delay = 0f;
				purchaseTween.ResetToBeginning();
				purchaseTween.PlayForward();
			}
			break;
		case IAPState.Loading:
			OKButton.SetActive(false);
			purchaseLabel.text = "Loading...";
			purchaseTween.from = purchaseTween.value;
			purchaseTween.to = 1f;
			purchaseTween.duration = 0.5f;
			purchaseTween.delay = 0f;
			purchaseTween.ResetToBeginning();
			purchaseTween.PlayForward();
			break;
		case IAPState.Error:
			OKButton.SetActive(true);
			purchaseTween.from = purchaseTween.value;
			purchaseTween.to = 1f;
			purchaseTween.duration = 0.5f;
			purchaseTween.delay = 0f;
			purchaseTween.ResetToBeginning();
			purchaseTween.PlayForward();
			break;
		case IAPState.Success:
			OKButton.SetActive(true);
			purchaseTween.from = purchaseTween.value;
			purchaseTween.to = 1f;
			purchaseTween.duration = 0.5f;
			purchaseTween.delay = 0f;
			purchaseTween.ResetToBeginning();
			purchaseTween.PlayForward();
			break;
		case IAPState.Restored:
			OKButton.SetActive(true);
			purchaseLabel.text = "Purchases Restored";
			purchaseTween.from = purchaseTween.value;
			purchaseTween.to = 1f;
			purchaseTween.duration = 0.5f;
			purchaseTween.delay = 0f;
			purchaseTween.ResetToBeginning();
			purchaseTween.PlayForward();
			break;
		}
	}

	public void InitiatePurchase(string productIdentifier)
	{
		iapState = IAPState.Loading;
		SetIAPLayout();

		if (Application.internetReachability != NetworkReachability.NotReachable)
		{
			if(EasyStoreKit.CanMakePayments())
			{
				if(EasyStoreKit.BuyProductWithIdentifier(productIdentifier, 1))
				{
					//Valid product identifier. Do nothing, the event will be called once processing is complete.
				}
				else
				{
					Debug.Log("Invalid product identifier: " + productIdentifier);
					purchaseLabel.text = "An error occured\nduring the transaction.\nPlease try again.";
					iapState = IAPState.Error;
					SetIAPLayout();
				}
			}
			else 
			{
				Debug.Log("Application is not allowed to make payments");
				purchaseLabel.text = "Error: Application not\nallowed to make payments";
				iapState = IAPState.Error;
				SetIAPLayout();
			}
		} 
		else 
		{
			Debug.Log("No internet connection available");
			purchaseLabel.text = "Error: No internet connection";
			iapState = IAPState.Error;
			SetIAPLayout();
		}
	}

	public void RestorePurchases()
	{
		iapState = IAPState.Loading;
		SetIAPLayout();
		restoredIdentifiers = "";
		EasyStoreKit.RestoreProducts();
	}

	//EasyStoreKit event handlers
	void ConfigureStoreKitEvents()
	{
		EasyStoreKit.productsLoadedEvent += ProductsLoaded;
		EasyStoreKit.transactionPurchasedEvent += TransactionPurchased;
		EasyStoreKit.transactionFailedEvent += TransactionFailed;
		EasyStoreKit.transactionRestoredEvent += TransactionRestored;
		EasyStoreKit.transactionCancelledEvent += TransactionCancelled;
		EasyStoreKit.restoreCompletedEvent += RestoreCompleted;
		EasyStoreKit.restoreFailedEvent += RestoreFailed;
	}
	
	public void ProductsLoaded(StoreKitProduct[] products)
	{
		Debug.Log("Products loaded....");
		iapState = IAPState.Normal;
		SetIAPLayout();
	}

	public void TransactionPurchased(string productIdentifier)
	{
		switch(productIdentifier)
		{
		case "anti_gravity":
			EncryptedPlayerPrefs.SetInt("Stage 2 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Anti-Gravity'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_anti_gravity_levels", 100f);
			break;
		case "worm_hole":
			EncryptedPlayerPrefs.SetInt("Stage 3 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Worm Hole'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_worm_hole_levels", 100f);
			break;
		case "chaos_theory":
			EncryptedPlayerPrefs.SetInt("Stage 4 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Chaos Theory'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_chaos_theory_levels", 100f);
			break;
		default:
			Debug.LogError("Not valid product identifier....");
			purchaseLabel.text = "An error occured\nduring the transaction.\nPlease try again.";
			iapState = IAPState.Error;
			SetIAPLayout();
			break;
		}

		PlayerPrefs.Save();
		iapState = IAPState.Success;
		SetIAPLayout();
		menuStateManager.SetState();
	}
	
	public void TransactionFailed(string productIdentifier, string errorMessage)
	{
		Debug.Log("Transaction failed for: " + productIdentifier + " :" + errorMessage);
		purchaseLabel.text = "An error occured\nduring the transaction.\nPlease try again.";
		iapState = IAPState.Error;
		SetIAPLayout();
	}
	
	public void TransactionRestored(string productIdentifier)
	{
		restoredIdentifiers += productIdentifier + " ";

		switch(productIdentifier)
		{
		case "anti_gravity":
			EncryptedPlayerPrefs.SetInt("Stage 2 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Anti-Gravity'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_anti_gravity_levels", 100f);
			break;
		case "worm_hole":
			EncryptedPlayerPrefs.SetInt("Stage 3 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Worm Hole'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_worm_hole_levels", 100f);
			break;
		case "chaos_theory":
			EncryptedPlayerPrefs.SetInt("Stage 4 Unlocked", 1);
			purchaseLabel.text = "Purchase Successful!\n''Chaos Theory'' Levels Unlocked.";
			gameCenterManager.SubmitAchievement("unlock_chaos_theory_levels", 100f);
			break;
		default:
			Debug.LogError("Not valid product identifier....");
			purchaseLabel.text = "An error occured\nduring the transaction.\nPlease try again.";
			iapState = IAPState.Error;
			SetIAPLayout();
			break;
		}
	}

	public void TransactionCancelled(string productIdentifier)
	{
		Debug.Log("Transaction Cancelled for: " + productIdentifier);
		iapState = IAPState.Normal;
		SetIAPLayout();
	}

	public void RestoreCompleted()
	{
		Debug.Log("Restore Complete: " + restoredIdentifiers);
		PlayerPrefs.Save();
		iapState = IAPState.Restored;
		SetIAPLayout();
		menuStateManager.SetState();
	}

	public void RestoreFailed(string errorMessage)
	{
		Debug.Log("Error during restore: " + errorMessage);
		purchaseLabel.text = "An error occured\nduring restore.\nPlease try again.";
		iapState = IAPState.Error;
		SetIAPLayout();
	}
}
