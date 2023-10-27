using System;
using System.Collections.Generic;
using FunGamesSdk;
using UnityEngine;
// using UnityEngine.Purchasing;
using UnityEngine.UI;

public class NoAdsSubscription : MonoBehaviour
{

    public static event Action onPurchased;
    public static event Action<string> onPurchasedFaild;
    public static event Action onRestored;
    public static event Action<string> onRestoreFaild;



    public static bool isNoAds => false;





    public static void BuySubscription()
    {
        // BUY 
        // SubscriptionHandler.Instance.PurchaseNoAds(() =>
        // {
        //     Debug.Log($"Purchase Complete");
        //     // isNoAds = true;

        //     onPurchased?.Invoke();
        // },
        // (error) =>
        // {
        //     Debug.Log($"Purchase Faild: {error}");
        //     onPurchasedFaild?.Invoke(error);
        // },
        // () =>
        // {
        //     Debug.Log($"Purchase cancelled");
        //     onPurchasedFaild?.Invoke("Purchase cancelled");
        // });
    }

    public static void RestorePurchase()
    {
        // BUY 
        // SubscriptionHandler.Instance.RestorePurchase(() =>
        // {
        //     Debug.Log($"Restore Complete");
        //     // isNoAds = true;

        //     onPurchased?.Invoke();
        // },
        // (error) =>
        // {
        //     Debug.Log($"Restore Faild: {error}");
        //     onPurchasedFaild?.Invoke(error);
        // });
    }








}

