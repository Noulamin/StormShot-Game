using System;
using System.Collections;
using System.Collections.Generic;
using FunGames.Sdk.Analytics;
using FunGames.Sdk.Analytics.Helpers;
using FunGames.Sdk.RemoteConfig;
using FunGamesSdk;
using FunGamesSdk.FunGames.AppTrackingManager;
using FunGamesSdk.FunGames.Gdpr;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;
// using OgurySdk;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class FunGamesManager : MonoBehaviour
{
    public static FunGamesManager _instance;
    ConsentForm _consentForm;


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
        // var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        // First thing to init is Max but not ads

        Debug.Log("Start() FunGamesManager.Start");
        FunGamesMax.Start();
        var debugSettings = new ConsentDebugSettings
        {
            // Geography appears as in EEA for debug devices.
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
                {
                    "92B693FAC54D04811E1B02FB748E0CFC"
                }
        };

        // Here false means users are not under age.
        ConsentRequestParameters request = new ConsentRequestParameters
        {

            ConsentDebugSettings = debugSettings,
        };

        // Check the current consent information status.
        ConsentInformation.Update(request, OnConsentInfoUpdated);
        // Debug.Log("Terms of Service has been accepted: " + SimpleGDPR.IsTermsOfServiceAccepted);
        // Debug.Log("Ads personalization consent state: " + SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT));
        // Debug.Log("Is user possibly located in the EEA: " + SimpleGDPR.IsGDPRApplicable);


        void OnConsentInfoUpdated(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                return;
            }

            if (ConsentInformation.IsConsentFormAvailable())
            {
                LoadConsentForm();
            }
            // If the error is null, the consent information state was updated.
            // You are now ready to check if a form is available.
        }
        void LoadConsentForm()
        {
            // Loads a consent form.
            ConsentForm.Load(OnLoadConsentForm);
        }
        void OnLoadConsentForm(ConsentForm consentForm, FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                return;
            }

            // The consent form was loaded.
            // Save the consent form for future requests.
            _consentForm = consentForm;

            // You are now ready to show the form.
            if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
            {
                _consentForm.Show(OnShowForm);
            }
        }


        void OnShowForm(FormError error)
        {
            if (error != null)
            {
                // Handle the error.
                UnityEngine.Debug.LogError(error);
                return;
            }

            // Handle dismissal by reloading form.
            LoadConsentForm();
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
