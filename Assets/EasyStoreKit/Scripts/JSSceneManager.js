#pragma strict

//the product identifiers defined in iTunes Connect
var productIdentifiers : String[];

enum GUIState {
	Loading,
	MainUI,
};

private var guiState : GUIState = GUIState.MainUI;

private var products : StoreKitProduct[];

//message to be displayed on UI.
private var message : String;
private var restoredIdentifiers : String;

function Start () {
	ConfigureStoreKitEvents();
	//0. Assign identifiers
	EasyStoreKit.AssignIdentifiers(productIdentifiers);
}

function Update () {

}

function OnGUI() {
	GUI.skin.button.wordWrap = true;
	GUI.skin.label.wordWrap = true;
	switch(guiState) {
		case GUIState.Loading:
			ShowLoadingUI();
			break;
		case GUIState.MainUI:
			ShowMainUI();
			break;
	}
	
}

private function ShowLoadingUI() {
	GUI.Label(Rect((Screen.width - 200) * 0.5, Screen.height * 0.5, 200, Screen.height * 0.2),"Please Wait...");
}

private function ShowMainUI() {
	var controlHeight = 100;
	var padding = 10;

	//this button reassigns the identifiers. This is just a testing functionality.
	//You should assign identifiers once only.
	if(GUI.Button( Rect(0,0,Screen.width * 0.5, controlHeight), "Reassign identifiers") ) {
		EasyStoreKit.AssignIdentifiers(productIdentifiers);
	}
	
	if(GUI.Button( Rect(Screen.width * 0.5,0,Screen.width * 0.5, controlHeight), "Load Products") ) {
		//1. Check for internet reachability
		if (Application.internetReachability != NetworkReachability.NotReachable) {
			
			//2. Check if payments can be made
			if(EasyStoreKit.CanMakePayments()) {
				//nullify previously loaded products
				this.products = null;
				
				//change ui state
				guiState = GUIState.Loading;
				message = "";
				
				//3. Load products
				EasyStoreKit.LoadProducts();
				
			} else {
				message = "Application is not allowed to make payments!";
			}
		} else {
			message = "No internet connection available!";
		}
	}
	
	//display the error label
	GUI.Label(Rect(0,controlHeight + padding,Screen.width,controlHeight), message);
	
	
	//display the rest of the ui if products are available
	if(products != null) {
		
		//display restore button
		if(GUI.Button( Rect(0,2 * controlHeight + padding,Screen.width, controlHeight), "Restore Purchases") ) {
			//reset string
			restoredIdentifiers = "";
			message = "";
			//change ui state
			guiState = GUIState.Loading;
			//restore products
			EasyStoreKit.RestoreProducts();
		}
		
		//multiplier for control positioning
		var multiplier : int = 3;
		for ( var product : StoreKitProduct in products) {
			if(GUI.Button( Rect(0,multiplier * controlHeight + padding,Screen.width, controlHeight), product.ToString()) ) {
				message = "";
				//change ui state
				guiState = GUIState.Loading;
				
				//buy product
				if(EasyStoreKit.BuyProductWithIdentifier(product.identifier,1)) {
					//valid product identifier. Do nothing, the event will be called once processing is complete
				} else {
					message = "Invalid product identifier: " + product.identifier;
					guiState = GUIState.MainUI;
				}
			}
			
			multiplier++;
		}
	}
}

//EasyStoreKit event handlers
private function ConfigureStoreKitEvents() {
	EasyStoreKit.productsLoadedEvent += ProductsLoaded;
	EasyStoreKit.transactionPurchasedEvent += TransactionPurchased;
	EasyStoreKit.transactionFailedEvent += TransactionFailed;
	EasyStoreKit.transactionRestoredEvent += TransactionRestored;
	EasyStoreKit.transactionCancelledEvent += TransactionCancelled;
	EasyStoreKit.restoreCompletedEvent += RestoreCompleted;
	EasyStoreKit.restoreFailedEvent += RestoreFailed;
}

function ProductsLoaded(products : StoreKitProduct[]) {
	this.products = products;
	//change ui state
	guiState = GUIState.MainUI;
}

function TransactionPurchased(productIdentifier : String) {
	//Unlock feature based on the identifier
	//We are only updating the UI!
	message = "Successfully purchased: " + productIdentifier;
	
	guiState = GUIState.MainUI;
}

function TransactionFailed(productIdentifier : String, errorMessage : String) {
	message = "Transaction failed for : " + productIdentifier + " :" + errorMessage;
	
	guiState = GUIState.MainUI;
}

function TransactionRestored(productIdentifier : String) {
	//Unlock feature based on the identifier restored.
	//We are only updating the UI!
	restoredIdentifiers += productIdentifier + " ";
}

function TransactionCancelled(productIdentifier : String) {
	//Remove any activity indicators as the user has cancelled the transaction
	guiState = GUIState.MainUI;
}

function RestoreCompleted() {
	//change ui state
	guiState = GUIState.MainUI;
	
	//set message
	message = "Restored " + restoredIdentifiers;
}

function RestoreFailed(errorMessage : String) {
	//change ui state
	guiState = GUIState.MainUI;
	
	//set message
	message = "Restore failed: " + errorMessage;
}