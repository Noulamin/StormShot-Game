using System;
using System.Collections;
using System.Collections.Generic;
using FunGames.Sdk.Analytics;
using FunGames.Sdk.Analytics.Helpers;
using FunGames.Sdk.RemoteConfig;
using FunGamesSdk;
using FunGamesSdk.FunGames.AppTrackingManager;
using FunGamesSdk.FunGames.Gdpr;
// using OgurySdk;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class FunGamesManager : MonoBehaviour
{
    public static FunGamesManager _instance;


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
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        // First thing to init is Max but not ads
        if (settings.useMax)
        {
            // MaxSdkCallbacks.OnSdkInitializedEvent += MaxIniCallback;
            Debug.Log("Terms of Service has been accepted: " + SimpleGDPR.IsTermsOfServiceAccepted);
            Debug.Log("Ads personalization consent state: " + SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT));
            Debug.Log("Is user possibly located in the EEA: " + SimpleGDPR.IsGDPRApplicable);
            if (PlayerPrefs.GetInt("GDPR.ConsentState", 0) == 1)
            {
                MaxSdk.SetHasUserConsent(true);
                MaxSdk.SetDoNotSell(PlayerPrefs.GetInt("GDPR.SetDoNotSell", 0) == 1);
                MaxSdk.SetIsAgeRestrictedUser(PlayerPrefs.GetInt("GDPR.SetIsAgeRestrictedUser", 0) == 1);

                FunGamesMax.Start();
            }
            else
            {
                StartCoroutine(ShowGDPRConsentDialogAndWait());
            }

        }
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
#endif
    }
    const string ADS_PERSONALIZATION_CONSENT = "ads";
    private IEnumerator ShowGDPRConsentDialogAndWait()
    {

        yield return SimpleGDPR.WaitForDialog(new GDPRConsentDialog()
                .AddSectionWithToggle(ADS_PERSONALIZATION_CONSENT, "Ads Personalization", "When enabled, you'll see ads that are more relevant to you. Otherwise, you will still receive ads, but they will no longer be tailored toward you.")
                .AddSectionWithToggle("UnderAge", "Are you Under Age (16)")
                .AddPrivacyPolicy("https://www.ouazgames.com/privacy-policy")
                );

        bool doNotSell = SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT) == SimpleGDPR.ConsentState.Yes;
        bool SetIsAgeRestrictedUsers = SimpleGDPR.GetConsentState("UnderAge") == SimpleGDPR.ConsentState.Yes;
        PlayerPrefs.SetInt("GDPR.ConsentState", 1);
        PlayerPrefs.SetInt("GDPR.SetDoNotSell", doNotSell ? 1 : 0);
        PlayerPrefs.SetInt("GDPR.SetIsAgeRestrictedUser", SetIsAgeRestrictedUsers ? 1 : 0);
        MaxSdk.SetHasUserConsent(true);
        MaxSdk.SetDoNotSell(doNotSell);
        MaxSdk.SetIsAgeRestrictedUser(SetIsAgeRestrictedUsers);
        FunGamesMax.Start();
    }


    private void OnApplicationPause(bool pauseStatus)
    {
        FunGamesMax._OnApplicationPause(pauseStatus);
    }



    public static void FinishTracking()
    {
#if UNITY_IOS && !UNITY_EDITOR
        if(FunGamesAppTrackingTransparency.isAuthorizeTracking())
            FunGamesAnalytics.NewDesignEvent("AllowTracking", "true");
#endif
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");

        TenjinHelpers.Initialize();
        GameAnalyticsHelpers.Initialize();
        // FunGamesApiAnalytics.Initialize();

        /*if (settings.useMax)
        {
            FunGamesMax.InitializeAds();
        }*/
        if (settings.useOgury)
        {
            Debug.Log("Initialize Ogury");
            // new GameObject("OguryCallbacks", typeof(OguryCallbacks));

            // Ogury.Start(settings.oguryAndroidAssetKey, settings.oguryIOSAssetKey);

            FunGamesThumbail.Start();
        }
        // FunGamesFB.Start();
    }
}
