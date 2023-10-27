using System;
using System.Collections;
using System.Collections.Generic;
// using Facebook.Unity;
using FunGames.Sdk.Analytics;
using FunGames.Sdk.Analytics.Helpers;
using FunGames.Sdk.RemoteConfig;
using FunGamesSdk;
using FunGamesSdk.FunGames.AppTrackingManager;
using FunGamesSdk.FunGames.Gdpr;
// using GoogleMobileAds.Ump;
// using GoogleMobileAds.Ump.Api;
// using AppsFlyerSDK;
// using OgurySdk;
using UnityEngine;
using FunGamesSdk.FunGames.Analytics.Helpers;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class FunGamesManager : MonoBehaviour
{
    public static event Action FunGameFinishInitializeSDKs;
    public static FunGamesManager _instance;
    public static BannerPosition defaultBannerPosition
    {
        get
        {
            return FunGamesSettings.LoadedSettings.bannerPosition;
        }
    }
    // ConsentForm _consentForm;


    private void Awake()
    {
        if (_instance == null)
        {

            _instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Rest of your Awake code
            FunGamesMax.Awake();
        }
        else
        {
            Destroy(this);
        }
    }
    public bool forceATT = false;
    // Start is called before the first frame update
    void Start()
    {
#if UNITY_IOS && !UNITY_EDITOR
        Version currentVersion = new Version(Device.systemVersion);
        Version iOSATT = new Version("14.5");
        Debug.Log("IOS device version : " + currentVersion + " target version : " + iOSATT);
        if (currentVersion >= iOSATT || forceATT)
        {
            FunGamesAnalytics.NewDesignEvent("ShowingATT", "true");
            FunGamesAppTrackingTransparency._instance.RequestAuthorizationAppTrackingTransparency(FinishTracking);
        }
        else
        {
            FinishTracking();   
        }
#else
        FinishTracking();

#endif

    }

    public static bool isSDKsInitialized;

    //     private static IEnumerator InitializeSDKs()
    //     {
    // #if UNITY_EDITOR
    //         isSDKsInitialized = true;
    //         FunGameFinishInitializeSDKs?.Invoke();
    // #endif
    //         Firebasehelper.InitializeFirebase();
    //         while (!Firebasehelper.isFirebaseInitialized)
    //         {
    //             yield return null;
    //         }
    //         Debug.Log("Initializing Firebasehelper : Done !");
    //         SubscriptionHandler.Instance.Initialize();
    //         while (!SubscriptionHandler.isSubscriptionHandlerInitialized)
    //         {
    //             yield return null;
    //         }
    //         Debug.Log("Initializing SubscriptionHandler : Done !");
    //         SubscriptionHandler.Instance.LogIn(Firebasehelper.currentUser.UserId);
    //         while (!SubscriptionHandler.isSubscriptionHandlerLogedIn)
    //         {
    //             yield return null;
    //         }
    //         Debug.Log("Customer Loge In : Done !");

    //         isSDKsInitialized = true;
    //         FunGameFinishInitializeSDKs?.Invoke();
    //     }
    const string ADS_PERSONALIZATION_CONSENT = "ads";
    private IEnumerator ShowGDPRConsentDialogAndWait()
    {

        yield return SimpleGDPR.WaitForDialog(new GDPRConsentDialog()
                .AddSectionWithToggle(ADS_PERSONALIZATION_CONSENT, "Ads Personalization", "When enabled, you'll see ads that are more relevant to you. Otherwise, you will still receive ads, but they will no longer be tailored toward you.")
                .AddSectionWithToggle("UnderAge", "Are you Under Age (16)", null, false)
                .AddPrivacyPolicy("https://www.ouazgames.com/privacy.html")

                );

        bool doNotSell = SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes;
        bool SetIsAgeRestrictedUsers = SimpleGDPR.GetConsentState("UnderAge") == SimpleGDPR.ConsentState.Yes;
        PlayerPrefs.SetInt("GDPR.ConsentState", doNotSell ? 1 : 0);
        PlayerPrefs.SetInt("GDPR.SetDoNotSell", doNotSell ? 0 : 1);
        PlayerPrefs.SetInt("GDPR.SetIsAgeRestrictedUser", SetIsAgeRestrictedUsers ? 1 : 0);
        MaxSdk.SetHasUserConsent(doNotSell);
        MaxSdk.SetDoNotSell(!doNotSell);
        MaxSdk.SetIsAgeRestrictedUser(SetIsAgeRestrictedUsers);
        FunGamesMax.Start(); // Initialize Ads
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        FunGamesMax._OnApplicationPause(pauseStatus);
    }
    Action callback;
    public void DelayInvoke(float delay, Action _callback)
    {
        callback = _callback;
        Invoke("DelayInvokeMethod", delay);
    }
    void DelayInvokeMethod()
    {
        callback?.Invoke();
    }

    static IEnumerator _FinishTracking()
    {
        GameAnalyticsHelpers.Initialize();
        yield return null;
#if UNITY_IOS && !UNITY_EDITOR
        if(FunGamesAppTrackingTransparency.isAuthorizeTracking())
            FunGamesAnalytics.NewDesignEvent("AllowTracking", "true");
#endif
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        // _instance.StartCoroutine(InitializeSDKs());
        Debug.Log("Start() FunGamesManager.Start");
        FunGamesMax.Start();
        TenjinHelpers.Initialize();
        // FunGamesApiAnalytics.Initialize();

        /*if (settings.useMax)
        {
            FunGamesMax.InitializeAds();
        }*/
        if (settings.useOgury)
        {
            Debug.Log("Initialize Ogury");
            FunGamesThumbail.Start();
        }

        // if (settings.useAppsFlyer)
        // {
        //     AppsFlyer.setIsDebug(settings.isDebug);
        //     AppsFlyer.initSDK(settings.devkey, settings.appID);
        //     AppsFlyer.startSDK();
        // }
        FunGamesAnalytics.NewDesignEvent("SDK's", "FinishTracking");
        // FacebookHelpers.Initialize();
    }



    public static void FinishTracking()
    {
        _instance.StartCoroutine(_FinishTracking());
    }
    public static bool IsInEurope()
    {
        string ip = new System.Net.WebClient().DownloadString("https://api.ipify.org");
        string response = new System.Net.WebClient().DownloadString($"https://ipapi.co/{ip}/country/");

        // Check if the country is in Europe
        if (response == "GB" || response == "DE" || response == "FR" || response == "ES" || response == "IT" || response == "NL")
        {
            return true;
        }

        return false;
    }
}
