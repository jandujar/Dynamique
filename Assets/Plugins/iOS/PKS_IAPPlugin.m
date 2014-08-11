//
//  PKSIAPPlugin.m
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#import "PKS_IAPPlugin.h"
#import "PKS_Utility.h"

@interface PKS_IAPPlugin() {
    PKS_IAPHelper *_iapHelper;
    NSMutableDictionary *_identifierProductMapping;
}
@end

@implementation PKS_IAPPlugin
-(id)initWithProductIdentifiers:(NSSet *)identifiers {
    self = [super init];
    if (self) {
        _iapHelper = [[PKS_IAPHelper alloc] initWithProductIdentifiers:identifiers];
        _iapHelper.delegate = self;
        
        _identifierProductMapping = [[NSMutableDictionary alloc] init];
    }
    
    return self;
}

-(void)dealloc {
    [_iapHelper release];
    [_identifierProductMapping release];
    [super dealloc];
}

#pragma mark - Public methods
-(void)requestProducts {
    [_iapHelper requestProducts];
}

- (bool)buyProductWithIdentifier:(NSString *)productIdentifier quantity:(NSInteger)quantity{
    bool result = false;
    
    SKProduct *product = [_identifierProductMapping valueForKey:productIdentifier];
    
    if (product) {
        result = true;
        [_iapHelper buyProduct:product quantity:quantity];
    } else {
        NSLog(@"Invalid identifier %@",productIdentifier);
    }
    
    return result;
}

//restore product
- (void) restoreCompletedTransactions {
    [_iapHelper restoreCompletedTransactions];
}

-(StoreKitProduct) detailsForProductWithIdentifier:(NSString*)identifier {
    SKProduct *product = [_identifierProductMapping valueForKey:identifier];
    StoreKitProduct result;
    if (product) {
        result.localizedTitle = [PKS_Utility CStringCopy:product.localizedTitle];
        result.localizedDescription = [PKS_Utility CStringCopy:product.localizedDescription];
        NSLocale *priceLocale = product.priceLocale;
        NSNumberFormatter *formatter = [[NSNumberFormatter alloc] init];
        formatter.locale = priceLocale;
        result.priceSymbol = [PKS_Utility CStringCopy:[formatter currencySymbol]];
        result.localPrice = [PKS_Utility CStringCopy:[product.price stringValue]];
        [formatter release];
        result.identifier = [PKS_Utility CStringCopy:product.productIdentifier];
    }
    
    return result;
}

#pragma mark -
static char* sGameObjectCString;
-(void)setGameObjectName:(NSString *)gameObjectName {
    if (sGameObjectCString != NULL) {
        free(sGameObjectCString);
        sGameObjectCString = NULL;
    }
    if (gameObjectName != nil) {
        sGameObjectCString = [PKS_Utility CStringCopy:gameObjectName];
    }
}

#pragma mark - PKSIAPHelperDelegate
-(void) productsLoaded:(NSArray*)products invalidIdentifiers:(NSArray*)invalidIdentifiers {
    //set up the mapping dictionary
    for (SKProduct *product in products) {
        [_identifierProductMapping setValue:product forKey:product.productIdentifier];
    }
    
    //signal unity.
    //Send a semicolon separated list of valid product identifiers
    NSArray *productIdentifierArray = [_identifierProductMapping allKeys];
    NSMutableString *string = [NSMutableString string];
    
    for (int index = 0; index < productIdentifierArray.count; index++) {
        
        [string appendString:[productIdentifierArray objectAtIndex:index]];
        
        //append ; after every identifier except for the last element
        if (index != productIdentifierArray.count - 1) {
            [string appendString:@";"];
        }
    }
    
    UnitySendMessage(sGameObjectCString, "productsLoaded", [PKS_Utility CStringCopy:string]);
}

-(void) transactionPurchased:(SKPaymentTransaction*)transaction {
    NSString *productIdentifier = transaction.payment.productIdentifier;    
    //send the identifier
    UnitySendMessage(sGameObjectCString, "transactionPurchased", [PKS_Utility CStringCopy:productIdentifier]);
}

-(void) transactionFailed:(SKPaymentTransaction*)transaction {
    NSString *errorDescription = transaction.error.localizedDescription;
    NSLog(@"transaction failed %@",errorDescription);
    
    NSString *productIdentifier = transaction.payment.productIdentifier;
    
    NSString *string = [NSString stringWithFormat:@"%@;%@",productIdentifier,errorDescription];
    
    //send the identifier & error semicolon separated
    UnitySendMessage(sGameObjectCString, "transactionFailed", [PKS_Utility CStringCopy:string]);
}

//called when transaction is cancelled or not allowed or server verification failed
-(void)transactionCancelled:(SKPaymentTransaction *)transaction {
    NSString *productIdentifier = transaction.payment.productIdentifier;
    UnitySendMessage(sGameObjectCString, "transactionCancelled", [PKS_Utility CStringCopy:productIdentifier]);
}

-(void) transactionRestored:(SKPaymentTransaction*)transaction {
    NSString *productIdentifier = transaction.originalTransaction.payment.productIdentifier;
    //send the identifier
    UnitySendMessage(sGameObjectCString, "transactionRestored", [PKS_Utility CStringCopy:productIdentifier]);
}

-(void) restoreProcessFailed:(NSError *)error {
    UnitySendMessage(sGameObjectCString, "restoreProcessFailed",[PKS_Utility CStringCopy: error.localizedDescription]);
}

-(void) restoreProcessCompleted {
    UnitySendMessage(sGameObjectCString, "restoreProcessCompleted","");    
}
@end
