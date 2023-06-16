using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.Events;
using System.Collections;
using Yodo1.MAS;

public partial class SROptions
{

    [System.ComponentModel.Category("YODO")]
    public void ShowYODOAdInspector()
    {

        // MobileAds.OpenAdInspector((AdError) =>
        // {

        // });
        Yodo1U3dMas.ShowDebugger();

    }

}
public class AdsManager : MonoBehaviour
{

    // public string YOUR_APP_KEY = "";
    public static event System.Action onAdmobInitialized;

    public static int timingBonus = 5;
    public static int timingBonusShowTime = 5;
    public static int dailyBonus = 1;

    public static int timingInterval = 180;

    public static bool actionTimersEnabled = false;
    public static bool isAdmobInitialized;


    // private BannerView bannerView;
    private Yodo1U3dBannerAdView bannerAdView;

    public GameObject waitingPanel;
    // public InterstitialAd interstitial;
    // public RewardedAd rewardedAd;

    public int bannerUnits = 10;
    public string privacyLinks = "https://policies.google.com/privacy";

    public static Action onRewardedAdClosed, onUserEarnedReward;
    public static AdsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // FunGamesMax.bannerPosition = MaxSdkBase.BannerPosition.BottomCenter;
            // FunGamesMax.OnFunGamesInitialized += Init;
            Yodo1U3dInterstitialAd.GetInstance().autoDelayIfLoadFail = true;
            Yodo1AdBuildConfig config =
            new Yodo1AdBuildConfig().enableUserPrivacyDialog(true).privacyPolicyUrl
            (privacyLinks);
            Yodo1U3dMas.SetAdBuildConfig(config);

            DontDestroyOnLoad(gameObject);
            return;
        }

        Destroy(gameObject);
    }

    private void OnEnable()
    {
        UnityEngine.SceneManagement.SceneManager.activeSceneChanged += (SceneOld, SceneNew) =>
        {
            Init();
        };
    }



    private void Start()
    {
        Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) =>
        {
            print("[Yodo1 Mas] OnSdkInitializedEvent, success:" + success + ", error: " + error.ToString());
            if (success)
            {
                print("[Yodo1 Mas] The initialization has succeeded");
                isAdmobInitialized = true;
                Init();
            }
            else
            {
                print("[Yodo1 Mas] The initialization has failed");
            }
        };
        Yodo1U3dMas.InitializeMasSdk();
    }

    void Init()
    {
        print("AdsManager :: Init");
        // IronSourceEvents.onSdkInitializationCompletedEvent += SdkInitializationCompletedEvent;
        // // IronSource.Agent.init(IronSourceAdUnits.BANNER);
        // IronSourceInitilizer.AutoInitialize();
        SdkInitializationCompletedEvent();
        // #if UNITY_EDITOR
        // #endif
        // Advertisements.Instance.Initialize();
    }
    private const string ADS_PERSONALIZATION_CONSENT = "Ads";


    private void UnityAnalyticsButtonClicked()
    {

    }


    private void SdkInitializationCompletedEvent()
    {

        // RequestConfiguration requestConfiguration =
        //     new RequestConfiguration.Builder()
        //     .SetSameAppKeyEnabled(true).build();
        // MobileAds.SetRequestConfiguration(requestConfiguration);


        // MobileAds.Initialize((i) =>
        // {
        //     if (FunGamesMax._USE_APPOPEN_ADMOBOnly)
        //     {
        //         Invoke("initAppOpen", 0.1f);
        //     }
        //     FunGamesMax.isAdmobInitialized = true;
        //     if (FunGamesMax._USE_BANNER_ADMOBOnly)
        //     {
        //         FunGamesMax.InitializeBannerAds();
        //     }
        //     else
        //     {
        //         Invoke("ShowBanner", 0.2f);
        //     }
        //     onAdmobInitialized?.Invoke();
        // });
        if (!AppOpenInitialized)
        {
            AppOpenAdManager.Instance.RequestAppOpen();
            OnAppStateChanged();
            AppOpenInitialized = true;
        }
        InitializeInterstitial();
        this.InitializeRewardedAds();
        Yodo1U3dInterstitialAd.GetInstance().LoadAd();
        Yodo1U3dRewardAd.GetInstance().LoadAd();
    }
    public static bool AppOpenInitialized;
    void initAppOpen()
    {
        // AppOpenAdManager.Instance.LoadAd();
        // AppStateEventNotifier.AppStateChanged += InonAppStateChanged;
    }

    public static bool isAppOpenInitiaed;
    public void OnAppStateChanged()
    {

        // if (!FunGamesMax._USE_APPOPEN_ADMOBOnly)
        // {
        //     FunGamesMax.showAppOpen();
        // }
        // else
        // {
        //     InonAppStateChanged();
        // }
        // COMPLETE: Show an app open ad if available.
        AppOpenAdManager.Instance.ShowAppOpen();

    }
    void InonAppStateChanged()
    {
        // if (state == AppState.Foreground)
        // {
        //     // COMPLETE: Show an app open ad if available.
        //     AppOpenAdManager.Instance.ShowAdIfAvailable();
        // }
    }


    private void InitializeInterstitial()
    {
        // Instantiate
        Yodo1U3dInterstitialAd.GetInstance();

        // Ad Events
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadedEvent += OnInterstitialAdLoadedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenFailedEvent += OnInterstitialAdOpenFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdClosedEvent += OnInterstitialAdClosedEvent;
    }

    private void OnInterstitialAdLoadedEvent(Yodo1U3dInterstitialAd ad)
    {
        print("[Yodo1 Mas] OnInterstitialAdLoadedEvent event received");
    }

    private void OnInterstitialAdLoadFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnInterstitialAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnInterstitialAdOpenedEvent(Yodo1U3dInterstitialAd ad)
    {
        print("[Yodo1 Mas] OnInterstitialAdOpenedEvent event received");
    }

    private void OnInterstitialAdOpenFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnInterstitialAdOpenFailedEvent event received with error: " + adError.ToString());
        // Load the next ad
        Yodo1U3dInterstitialAd.GetInstance().LoadAd();
        waitingPanel.SetActive(false);
        _onClosed?.Invoke();
        _onClosed = null;
    }

    private void OnInterstitialAdClosedEvent(Yodo1U3dInterstitialAd ad)
    {
        print("[Yodo1 Mas] OnInterstitialAdClosedEvent event received");
        waitingPanel.SetActive(false);
        _onClosed?.Invoke();
        _onClosed = null;
        // Load the next ad
        Yodo1U3dInterstitialAd.GetInstance().LoadAd();
    }

    public void ShowBanner()
    {

        bannerShow = true;
        // Clean up banner before reusing
        if (bannerAdView != null)
        {
            bannerAdView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerAdView = new Yodo1U3dBannerAdView(Yodo1U3dBannerAdSize.Banner, Yodo1U3dBannerAdPosition.BannerBottom | Yodo1U3dBannerAdPosition.BannerHorizontalCenter);

        // Ad Events
        bannerAdView.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        bannerAdView.OnAdFailedToLoadEvent += OnBannerAdFailedToLoadEvent;
        bannerAdView.OnAdOpenedEvent += OnBannerAdOpenedEvent;
        bannerAdView.OnAdFailedToOpenEvent += OnBannerAdFailedToOpenEvent;
        bannerAdView.OnAdClosedEvent += OnBannerAdClosedEvent;

        bannerAdView.LoadAd();
        // FunGamesMax.BannerIsLoaded("");
        // FunGamesMax.ShowBannerAd();



    }
    bool bannerShow;
    private void OnBannerAdLoadedEvent(Yodo1U3dBannerAdView adView)
    {
        // Banner ad is ready to be shown.
        print("[Yodo1 Mas] OnBannerAdLoadedEvent event received");
    }

    private void OnBannerAdFailedToLoadEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnBannerAdFailedToLoadEvent event received with error: " + adError.ToString());
    }

    private void OnBannerAdOpenedEvent(Yodo1U3dBannerAdView adView)
    {
        print("[Yodo1 Mas] OnBannerAdOpenedEvent event received");
    }

    private void OnBannerAdFailedToOpenEvent(Yodo1U3dBannerAdView adView, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnBannerAdFailedToOpenEvent event received with error: " + adError.ToString());
    }

    private void OnBannerAdClosedEvent(Yodo1U3dBannerAdView adView)
    {
        print("[Yodo1 Mas] OnBannerAdClosedEvent event received");
    }
    public void HideBanner()
    {
        bannerShow = false;
        bannerAdView.Destroy();
        bannerAdView = null;
        // FunGamesMax.HideBannerAd();

    }

    public bool HasBannerPlacement()
    {
        var bol =/*  Advertisements.Instance.IsBannerOnScreen() || */ bannerShow;
        print("HASBANNERPlacement : " + bol);
        return bol;
    }
    public int CalcBannerHeight(int height)
    {

        return 0;
    }
    public void setPanelBanner(Vector2 orgSize, RectTransform rectTransform)
    {

        var pivotOrg = rectTransform.pivot;
        float bannerHeight = 200;
        // var bannerHeight = MaxSdkUtils.GetAdaptiveBannerHeight();

        // if (FunGamesMax.IsBannerLoaded)
        //     bannerHeight = 200;
        // else
        //     bannerHeight = 0;

        // if (FunGamesMax._USE_BANNER_ADMOBOnly)
        // {
        //     bannerHeight = 200;
        // }
        if (Application.isEditor)
        {
            var banner = GameObject.FindGameObjectWithTag("banner");
            if (banner != null)
            {
                bannerHeight = banner.GetComponent<RectTransform>().sizeDelta.y;
            }
        }
        rectTransform.sizeDelta = orgSize;
        // if (FunGamesMax.bannerPosition == MaxSdkBase.BannerPosition.TopCenter)
        // {
        //     rectTransform.pivot = new Vector2(0, pivotOrg.y);
        // }
        // else
        {
            rectTransform.pivot = new Vector2(1, pivotOrg.y);
        }
        var s = rectTransform.sizeDelta;
        s.y -= bannerHeight;
        rectTransform.sizeDelta = s;
        rectTransform.pivot = pivotOrg;
    }




    public void DestroyBanner()
    {

        HideBanner();
    }

    public bool HasInterstitial()
    {
        var bol = Yodo1U3dInterstitialAd.GetInstance().IsLoaded();

        print("HAS INTERSTITIAL : " + bol);
        return bol;
    }

    System.Action _onClosed;
    void _showInterstitial(System.Action onClosed)
    {

        // FunGamesMax.ShowAd(false, (s) =>
        // {

        //     onClosed?.Invoke();
        // });
        bool isLoaded = Yodo1U3dInterstitialAd.GetInstance().IsLoaded();
        print("start show interstitial");
        delayAction(() =>
        {
            _onClosed = onClosed;

            if (isLoaded) Yodo1U3dInterstitialAd.GetInstance().ShowAd(); else waitingPanel.SetActive(false);
        });

    }
    void delayAction(Action callback)
    {
        if (waitingPanel.activeInHierarchy)
        {
            return;
        }
        waitingPanel.SetActive(true);
        // StopAllCoroutines();
        StartCoroutine(_delayAction(callback));
    }

    IEnumerator _delayAction(Action callback)
    {
        yield return new WaitForSecondsRealtime(3);
        callback?.Invoke();
    }

    public bool ShowInterstitial()
    {
        if (Time.time < timer)
        {
            print(Time.time + "// " + timer);
            return false;
        }
        if (HasInterstitial())
        {
            _showInterstitial(() =>
            {
                timer = Time.time + MaxTime;
            });
        }

        return false;
    }
    public const float MaxTime = 25;
    public static float timer = MaxTime / 2;
    public void ShowInterstitialTimer(Action action)
    {
        if (Time.time < timer)
        {
            print(Time.time + "// " + timer);
            action?.Invoke();
            return;
        }


        if (HasInterstitial())
        {
            _showInterstitial(action);
            timer = Time.time + MaxTime;

        }
        else
        {
            action?.Invoke();

        }
    }

    public bool HasRewardedVideo()
    {
        var bol = Yodo1U3dRewardAd.GetInstance().IsLoaded();

        print("HAS REWARD : " + bol);
        return bol;
        // return false;
    }

    private void InitializeRewardedAds()
    {
        // Instantiate
        Yodo1U3dRewardAd.GetInstance();

        // Ad Events
        Yodo1U3dRewardAd.GetInstance().OnAdLoadedEvent += OnRewardAdLoadedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdLoadFailedEvent += OnRewardAdLoadFailedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdOpenedEvent += OnRewardAdOpenedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdOpenFailedEvent += OnRewardAdOpenFailedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdClosedEvent += OnRewardAdClosedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdEarnedEvent += OnRewardAdEarnedEvent;
    }

    private void OnRewardAdLoadedEvent(Yodo1U3dRewardAd ad)
    {
        print("[Yodo1 Mas] OnRewardAdLoadedEvent event received");
    }

    private void OnRewardAdLoadFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnRewardAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnRewardAdOpenedEvent(Yodo1U3dRewardAd ad)
    {
        print("[Yodo1 Mas] OnRewardAdOpenedEvent event received");
    }

    private void OnRewardAdOpenFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
    {
        print("[Yodo1 Mas] OnRewardAdOpenFailedEvent event received with error: " + adError.ToString());
        // Load the next ad
        Yodo1U3dRewardAd.GetInstance().LoadAd();
        _ClosedAd?.Invoke(false);
        _ClosedAd = null;
    }

    private void OnRewardAdClosedEvent(Yodo1U3dRewardAd ad)
    {
        print("[Yodo1 Mas] OnRewardAdClosedEvent event received");
        // Load the next ad
        Yodo1U3dRewardAd.GetInstance().LoadAd();
    }

    private void OnRewardAdEarnedEvent(Yodo1U3dRewardAd ad)
    {
        print("[Yodo1 Mas] OnRewardAdEarnedEvent event received");
        // Add your reward code here
        _ClosedAd?.Invoke(true);
        _ClosedAd = null;
    }

    System.Action<bool> _ClosedAd;
    void _showRewardedVideo(System.Action<bool> ClosedAd)
    {

        // FunGamesMax.ShowAd(true, (s) =>
        // {
        //     ClosedAd?.Invoke(s != FunGamesMax.AdState.failed);
        // });
        _ClosedAd = ClosedAd;
        bool isLoaded = Yodo1U3dRewardAd.GetInstance().IsLoaded();

        if (isLoaded) Yodo1U3dRewardAd.GetInstance().ShowAd();


    }

    public bool ShowRewardedAd()
    {

        if (HasRewardedVideo())
        {
            _showRewardedVideo((b) =>
            {
                if (b)
                {
                    onUserEarnedReward?.Invoke();
                    print("HandleRewardedAdRewarded event received for ");
                }
                onRewardedAdClosed?.Invoke();

            });
            return true;
        }
        else
        {
            print("Rewarded ad is not ready yet");
            return false;
        }
    }
    public bool OnShowAd;
    public void ShowIntersitial(UnityAction CompleteMethod)
    {
        if (OnShowAd)
        {
            return;
        }
        OnShowAd = true;
        if (HasInterstitial())
        {
            _showInterstitial(() =>
            {
                OnShowAd = false;
                CompleteMethod?.Invoke();
            });
        }
        else
        {
            OnShowAd = false;
            CompleteMethod?.Invoke();
        }
    }
    public void ShowReward(UnityAction<bool> CompleteMethod)
    {
        if (HasRewardedVideo())
        {
            _showRewardedVideo((b) => { CompleteMethod?.Invoke(b); });
        }
        else
        {
            CompleteMethod?.Invoke(false);
        }
    }

    void OnApplicationPause(bool isPaused)
    {
        if (!isPaused && isAppOpenInitiaed)
        {
            OnAppStateChanged();
        }
    }

}
