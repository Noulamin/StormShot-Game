using System;
using FunGames.Sdk.Analytics;
using FunGamesSdk;
using FunGamesSdk.FunGames.Ads;
using GameAnalyticsSDK;
using GoogleMobileAds;
using GoogleMobileAds.Api;
// using OgurySdk;
using UnityEngine;

public class FunGamesMax
{
    public static event System.Action OnFunGamesInitialized;

    // public static bool _USE_APPOPEN_ADMOBOnly;
    // public static bool _USE_BANNER_ADMOBOnly;

    private static string _maxSdkKey;
    private static string _interstitialAdUnitId;
    private static string _rewardedAdUnitId;
    private static string _bannerAdUnitId;
    private static string _appopenAdUnitId;


    private static int _interstitialRetryAttempt;
    private static int _rewardedRetryAttempt;
    private static int _bannerRetryAttempt;

    private static Action<string, string, int> _rewardedCallback;
    private static string _rewardedCallbackArgString;
    private static int _rewardedCallbackArgInt;

    private static Action<string, string, int> _interstitialCallback;
    private static string _interstitialCallbackArgString;
    private static int _interstitialCallbackArgInt;

    private static bool _isBannerLoaded;
    private static bool _showBannerAsked;
    private static bool _isBannerShowing;

    // Awake is called on the awake of FunGamesAds
    internal static void Awake()
    {
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        _maxSdkKey = settings.maxSdkKey;

        // _USE_APPOPEN_ADMOBOnly = settings.Use_Appopen_AdmobOnly;
        // _USE_BANNER_ADMOBOnly = settings.Use_Banner_AdmobOnly;
        if (settings.useMax)
        {


#if UNITY_IOS
		_interstitialAdUnitId = settings.iOSInterstitialAdUnitId;
		_rewardedAdUnitId = settings.iOSRewardedAdUnitId;
		_bannerAdUnitId = settings.iOSBannerAdUnitId;
		_appopenAdUnitId = settings.IOSAppopenAdUnitId;

#endif

#if UNITY_ANDROID
            _interstitialAdUnitId = settings.androidInterstitialAdUnitId;
            _rewardedAdUnitId = settings.androidRewardedAdUnitId;
            _bannerAdUnitId = settings.androidBannerAdUnitId;
            _appopenAdUnitId = settings.androidAppopenAdUnitId;

#endif
        }
        else
        {
#if UNITY_IOS
		_interstitialAdUnitId = settings.AdmobiOSInterstitialAdUnitId;
		_rewardedAdUnitId = settings.AdmobiOSRewardedAdUnitId;
		_bannerAdUnitId = settings.AdmobiOSBannerAdUnitId;
		_appopenAdUnitId = settings.AdmobIOSAppopenAdUnitId;

#endif

#if UNITY_ANDROID
            _interstitialAdUnitId = settings.AdmobandroidInterstitialAdUnitId;
            _rewardedAdUnitId = settings.AdmobandroidRewardedAdUnitId;
            _bannerAdUnitId = settings.AdmobandroidBannerAdUnitId;
            _appopenAdUnitId = settings.AdmobandroidAppopenAdUnitId;

#endif

        }
    }

    // Start is called on the start of FunGamesAds
    internal static void Start()
    {
        Debug.Log("internal static void Start()");
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
        if (settings.useMax)
        {


            // Set user consent status for AppLovin SDK
            MaxSdk.SetHasUserConsent(true);
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) =>
            {

                InitializeAds();
                OnFunGamesInitialized?.Invoke();
            };


            MaxSdk.SetSdkKey(_maxSdkKey);

            MaxSdk.InitializeSdk();
            Debug.Log("Initializing FunGamesAds");
        }
        else
        {
            // Initialize the Google Mobile Ads SDK.
            MobileAds.Initialize((InitializationStatus initStatus) =>
            {
                // This callback is called once the MobileAds SDK is initialized.
                isAdmobInitialized = true;
                InitializeAds();
                OnFunGamesInitialized?.Invoke();
            });

            Debug.Log("Initializing FunGamesAds");
        }
    }

    public static void InitializeAds()
    {
        InitializeInterstitialAds();
        InitializeRewardedAds();
        InitializeBannerAds();
        InitializeAppOpenAds();
    }

    private static void InitializeInterstitialAds()
    {
        if (!isAdmobInitialized)
        {
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        }
        try
        {
            LoadInterstitial();
        }
        catch
        {
            Debug.Log("Failed Load Interstitials : Please Check Ad Unit");
        }
    }
    static InterstitialAd interstitialAd;

    private static void LoadInterstitial()
    {
        if (isAdmobInitialized)
        {
            // Clean up the old ad before loading a new one.
            if (interstitialAd != null)
            {
                interstitialAd.Destroy();
                interstitialAd = null;
            }

            Debug.Log("Loading the interstitial ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();


            // send the request to load the ad.
            InterstitialAd.Load(_interstitialAdUnitId, adRequest, (InterstitialAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("interstitial ad failed to load an ad " +
                                        "with error : " + error);

                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadInterstitial();
                        });
                        return;
                    }

                    Debug.Log("Interstitial ad loaded with response : "
                                + ad.GetResponseInfo());

                    interstitialAd = ad;

                    // Raised when the ad is estimated to have earned money.
                    interstitialAd.OnAdPaid += (AdValue adValue) =>
                    {
                        Debug.Log(String.Format("Interstitial ad paid {0} {1}.",
                            adValue.Value,
                            adValue.CurrencyCode));
                    };
                    // Raised when an impression is recorded for an ad.
                    interstitialAd.OnAdImpressionRecorded += () =>
                    {
                        Debug.Log("Interstitial ad recorded an impression.");
                    };
                    // Raised when a click is recorded for an ad.
                    interstitialAd.OnAdClicked += () =>
                    {
                        Debug.Log("Interstitial ad was clicked.");
                    };
                    // Raised when an ad opened full screen content.
                    interstitialAd.OnAdFullScreenContentOpened += () =>
                    {
                        Debug.Log("Interstitial ad full screen content opened.");
                    };
                    // Raised when the ad closed full screen content.
                    interstitialAd.OnAdFullScreenContentClosed += () =>
                    {
                        Debug.Log("Interstitial ad full screen content closed.");
                        _interstitialCallback?.Invoke("success", _interstitialCallbackArgString, _interstitialCallbackArgInt);
                        _interstitialCallback = null;

                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadInterstitial();
                        });
                    };
                    // Raised when the ad failed to open full screen content.
                    interstitialAd.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        Debug.LogError("Interstitial ad failed to open full screen content " +
                                    "with error : " + error);
                        _interstitialCallback?.Invoke("fail", _interstitialCallbackArgString, _interstitialCallbackArgInt);
                        _interstitialCallback = null;
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadInterstitial();
                        });
                    };
                });
        }
        else
        {
            MaxSdk.LoadInterstitial(_interstitialAdUnitId);
        }
    }

    private static void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        _interstitialRetryAttempt = 0;
        printApplovinAdInfo("Intertitial OnInterstitialLoadedEvent", adInfo);
    }

    private static void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        _interstitialRetryAttempt++;
        var retryDelay = Math.Pow(2, _interstitialRetryAttempt);

        FunGamesAds._instance.Invoke(nameof(LoadInterstitial), (float)retryDelay);
        FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial);
        _interstitialCallback?.Invoke("fail", _interstitialCallbackArgString, _interstitialCallbackArgInt);
        _interstitialCallback = null;
        printApplovinAdInfo("Intertitial OnInterstitialFailedEvent", null, errorInfo);
    }

    private static void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial);
        // _interstitialCallback?.Invoke("success", _interstitialCallbackArgString, _interstitialCallbackArgInt);
        // _interstitialCallback = null;
        printApplovinAdInfo("Intertitial OnInterstitialDisplayedEvent", adInfo);
    }

    private static void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadInterstitial();
        FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial);
        _interstitialCallback?.Invoke("fail", _interstitialCallbackArgString, _interstitialCallbackArgInt);
        _interstitialCallback = null;
        printApplovinAdInfo("Intertitial InterstitialFailedToDisplayEvent", adInfo, errorInfo);
    }

    private static void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        _interstitialCallback?.Invoke("success", _interstitialCallbackArgString, _interstitialCallbackArgInt);
        _interstitialCallback = null;
        LoadInterstitial();
        FunGamesAnalytics.NewDesignEvent("Interstitial", "Dismissed");
        printApplovinAdInfo("Intertitial OnInterstitialDismissedEvent", adInfo);
    }

    private static void InitializeRewardedAds()
    {
        if (!isAdmobInitialized)
        {
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        }

        try
        {
            LoadRewardedAd();
        }
        catch
        {
            Debug.Log("Failed Load Rewarded : Please Check Ad Unit");
        }
    }

    static RewardedAd rewardedAd;

    private static void LoadRewardedAd()
    {
        if (isAdmobInitialized)
        {
            // Clean up the old ad before loading a new one.
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }

            Debug.Log("Loading the rewarded ad.");

            // create our request used to load the ad.
            var adRequest = new AdRequest();


            // send the request to load the ad.
            RewardedAd.Load(_rewardedAdUnitId, adRequest, (RewardedAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("Rewarded ad failed to load an ad " +
                                        "with error : " + error);

                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadRewardedAd();
                        });
                        return;
                    }

                    Debug.Log("Rewarded ad loaded with response : "
                                + ad.GetResponseInfo());

                    rewardedAd = ad;

                    // Raised when the ad is estimated to have earned money.
                    rewardedAd.OnAdPaid += (AdValue adValue) =>
                    {
                        Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                            adValue.Value,
                            adValue.CurrencyCode));
                        // collected = true;
                    };
                    // Raised when an impression is recorded for an ad.
                    rewardedAd.OnAdImpressionRecorded += () =>
                    {
                        Debug.Log("Rewarded ad recorded an impression.");
                    };
                    // Raised when a click is recorded for an ad.
                    rewardedAd.OnAdClicked += () =>
                    {
                        Debug.Log("Rewarded ad was clicked.");
                    };
                    // Raised when an ad opened full screen content.
                    rewardedAd.OnAdFullScreenContentOpened += () =>
                    {
                        Debug.Log("Rewarded ad full screen content opened.");
                    };
                    // Raised when the ad closed full screen content.
                    rewardedAd.OnAdFullScreenContentClosed += () =>
                    {
                        Debug.Log("Rewarded ad full screen content closed.");
                        if (collected)
                        {
                            _rewardedCallback?.Invoke("reward", _rewardedCallbackArgString, _rewardedCallbackArgInt);
                            _rewardedCallback = null;
                        }
                        else
                        {
                            _rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
                            _rewardedCallback = null;
                        }
                        collected = false;
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadRewardedAd();
                        });
                    };
                    // Raised when the ad failed to open full screen content.
                    rewardedAd.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        Debug.LogError("Rewarded ad failed to open full screen content " +
                                    "with error : " + error);
                        _rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
                        _rewardedCallback = null;
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadRewardedAd();
                        });
                    };

                });
        }
        else
        {
            MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
        }
    }

    private static void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        _rewardedRetryAttempt = 0;
        FunGamesAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.RewardedVideo);
        printApplovinAdInfo("Reward OnRewardedAdLoadedEvent", adInfo);

    }

    private static void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        _rewardedRetryAttempt++;
        var retryDelay = Math.Pow(2, _rewardedRetryAttempt);

        FunGamesAds._instance.Invoke("LoadRewardedAd", (float)retryDelay);
        FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo);
        _rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
        _rewardedCallback = null;
        printApplovinAdInfo("Reward OnRewardedAdFailedEvent", null, errorInfo);
    }

    private static void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo);
        printApplovinAdInfo("Reward OnRewardedAdDisplayedEvent", adInfo);
    }

    private static void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo);
        _rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
        _rewardedCallback = null;
        printApplovinAdInfo("Reward OnRewardedAdFailedToDisplayEvent", adInfo, errorInfo);
    }

    private static void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FunGamesAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo);
        printApplovinAdInfo("Reward OnRewardedAdClickedEvent", adInfo);
    }

    private static void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        LoadRewardedAd();
        // _rewardedCallback?.Invoke("success", _rewardedCallbackArgString, _rewardedCallbackArgInt);
        // _rewardedCallback = null;
        if (collected)
        {
            _rewardedCallback?.Invoke("reward", _rewardedCallbackArgString, _rewardedCallbackArgInt);
            _rewardedCallback = null;
        }
        else
        {
            _rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
            _rewardedCallback = null;
        }
        collected = false;
        printApplovinAdInfo("Reward OnRewardedAdDismissedEvent", adInfo);
    }
    static bool collected;

    private static void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdkBase.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        FunGamesAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo);
        collected = true;
        printApplovinAdInfo("Reward OnRewardedAdReceivedRewardEvent", adInfo);
    }
    public static BannerPosition bannerPosition = BannerPosition.BOTTOM;

    public static bool isAdmobInitialized;
    static BannerView _bannerView;
    public static void InitializeBannerAds()
    {

        if (isAdmobInitialized)
        {
            if (_bannerView != null)
            {
                _bannerView.Destroy();
            }
            _bannerView = new BannerView(_bannerAdUnitId, AdSize.Banner, bannerPosition == BannerPosition.BOTTOM ? AdPosition.Bottom : AdPosition.Top);
            // Raised when an ad is loaded into the banner view.
            _bannerView.OnBannerAdLoaded += () =>
            {
                // Debug.Log("Banner view loaded an ad with response : "
                //     + _bannerView.GetResponseInfo());
                printApplovinAdInfo<ResponseInfo, string>("Banner view loaded", _bannerView.GetResponseInfo(), "");
            };
            _bannerView.OnAdFullScreenContentClosed += () =>
            {
                // Debug.Log("Banner view loaded an ad with response : "
                //     + _bannerView.GetResponseInfo());
                printApplovinAdInfo<ResponseInfo, string>("Banner view closed", _bannerView.GetResponseInfo(), "");
            };
            _bannerView.OnAdFullScreenContentOpened += () =>
            {
                // Debug.Log("Banner view loaded an ad with response : "
                //     + _bannerView.GetResponseInfo());
                printApplovinAdInfo<ResponseInfo, string>("Banner view Opening", _bannerView.GetResponseInfo(), "");
            };
            // Raised when an ad fails to load into the banner view.
            _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
            {
                Debug.LogError("Banner view failed to load an ad with error : "
                    + error);
                printApplovinAdInfo<ResponseInfo, string>("Banner view faild", error.GetResponseInfo(), error.ToString());
            };
            // Raised when the ad is estimated to have earned money.
            _bannerView.OnAdPaid += (AdValue adValue) =>
            {
                printApplovinAdInfo<string, string>("Banner view paid", adValue.ToString(), "");
                // Debug.Log(String.Format("Banner view paid {0} {1}.",
                //     adValue.Value,
                //     adValue.CurrencyCode));
            };

            Debug.Log("Banner admobOnly Created");
            ShowBannerAd();
        }
        else
        {

            MaxSdkCallbacks.OnBannerAdLoadedEvent += BannerIsLoaded;


            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdLoadFailedEvent;
            MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
            MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;
            MaxSdkCallbacks.Banner.OnAdExpandedEvent += OnBannerAdExpandedEvent;
            MaxSdkCallbacks.Banner.OnAdCollapsedEvent += OnBannerAdCollapsedEvent;

            try
            {
                MaxSdk.CreateBanner(_bannerAdUnitId, bannerPosition == BannerPosition.TOP ? MaxSdkBase.BannerPosition.TopCenter : MaxSdkBase.BannerPosition.BottomCenter);
                MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.clear);
                _isBannerShowing = false;
                Debug.Log("Banner Created");
            }
            catch
            {
                Debug.Log("Failed Create Banner : Please Check Ad Unit");
            }
        }
    }
    public static bool IsBannerLoaded;
    static AppOpenAd appOpenAd;

    private static void InitializeAppOpenAds()
    {
        if (isAdmobInitialized)
        {
            loadAppOpen();
            // ShowAdIfReady(_appopenAdUnitId);
            Debug.Log("App Open Initiated");
        }
        else
        {
            MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;

            try
            {

                ShowAdIfReady(_appopenAdUnitId);
                Debug.Log("App Open Initiated");
            }
            catch
            {
                Debug.Log("Failed Initiate AppOpen : Please Check Ad Unit");
            }
        }
    }
    public static void LoadAppOpen()
    {
        if (isAdmobInitialized)
        {


            if (appOpenAd != null)
            {
                appOpenAd.Destroy();
                appOpenAd = null;
            }

            Debug.Log("Loading the app open ad.");

            // Create our request used to load the ad.
            var adRequest = new AdRequest();

            // send the request to load the ad.
            AppOpenAd.Load(_appopenAdUnitId, ScreenOrientation.Portrait, adRequest, (AppOpenAd ad, LoadAdError error) =>
                {
                    // if error is not null, the load request failed.
                    if (error != null || ad == null)
                    {
                        Debug.LogError("app open ad failed to load an ad " +
                                        "with error : " + error);
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {

                            LoadAppOpen();
                        });
                        return;
                    }

                    Debug.Log("App open ad loaded with response : "
                                + ad.GetResponseInfo());

                    appOpenAd = ad;
                    AdsManager.isAppOpenInitiaed = true;
                    // Raised when the ad is estimated to have earned money.
                    appOpenAd.OnAdPaid += (AdValue adValue) =>
                    {
                        Debug.Log(String.Format("App open ad paid {0} {1}.",
                            adValue.Value,
                            adValue.CurrencyCode));
                    };
                    // Raised when an impression is recorded for an ad.
                    appOpenAd.OnAdImpressionRecorded += () =>
                    {
                        Debug.Log("App open ad recorded an impression.");
                    };
                    // Raised when a click is recorded for an ad.
                    appOpenAd.OnAdClicked += () =>
                    {
                        Debug.Log("App open ad was clicked.");
                    };
                    // Raised when an ad opened full screen content.
                    appOpenAd.OnAdFullScreenContentOpened += () =>
                    {
                        Debug.Log("App open ad full screen content opened.");
                    };
                    // Raised when the ad closed full screen content.
                    appOpenAd.OnAdFullScreenContentClosed += () =>
                    {
                        Debug.Log("App open ad full screen content closed.");
                    };
                    // Raised when the ad failed to open full screen content.
                    appOpenAd.OnAdFullScreenContentFailed += (AdError error) =>
                    {
                        Debug.LogError("App open ad failed to open full screen content " +
                                    "with error : " + error);
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            LoadAppOpen();
                        });
                    };
                    if (firstTimeAppopen == false)
                    {
                        firstTimeAppopen = true;
                        FunGamesManager._instance.DelayInvoke(2, () =>
                        {
                            showAppOpen();
                        });
                    }
                });
        }
    }
    public static void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        MaxSdk.LoadAppOpenAd(_appopenAdUnitId);
        printApplovinAdInfo("AppOpen OnAppOpenDismissedEvent", adInfo);
    }
    public static void _OnApplicationPause(bool pauseStatus)
    {
        if (!pauseStatus)
        {
            ShowAdIfReady(_appopenAdUnitId);
        }
    }
    public static void showAppOpen()
    {
        ShowAdIfReady(_appopenAdUnitId);
    }
    public static void loadAppOpen()
    {
        if (isAdmobInitialized)
        {
            LoadAppOpen();
        }
        else
        {
            MaxSdk.LoadAppOpenAd(_appopenAdUnitId);
        }
    }

    static bool firstTimeAppopen;
    public static void ShowAdIfReady(string AppOpenAdUnitId)
    {
        // if (_USE_APPOPEN_ADMOBOnly)
        // {
        //     return;
        // }
        if (Time.unscaledTime < AdsManager.timer && firstTimeAppopen)
        {
            Debug.Log(Time.unscaledTime + "// " + AdsManager.timer);
            // action?.Invoke();
            return;
        }
        AdsManager.timer = Time.unscaledTime + AdsManager.MaxTime;

        if (isAdmobInitialized)
        {
            if (appOpenAd != null && appOpenAd.CanShowAd())
            {
                firstTimeAppopen = true;
                Debug.Log("Showing app open ad.");
                appOpenAd.Show();
            }
            else
            {
                Debug.LogError("App open ad is not ready yet.");
                loadAppOpen();
            }
        }
        else
        {
            if (MaxSdk.IsAppOpenAdReady(AppOpenAdUnitId))
            {
                firstTimeAppopen = true;
                Debug.Log("Show App Open");
                MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
            }
            else
            {
                Debug.Log("Load App Open");
                AdsManager.isAppOpenInitiaed = true;
                MaxSdk.LoadAppOpenAd(AppOpenAdUnitId);
                Debug.Log("Show App Open");
                MaxSdk.ShowAppOpenAd(AppOpenAdUnitId);
            }
        }
    }

    public static void printApplovinAdInfo(string tag, MaxSdkBase.AdInfo adInfo, MaxSdkBase.ErrorInfo errorInfo = null)
    {
        printApplovinAdInfo<MaxSdkBase.AdInfo, MaxSdkBase.ErrorInfo>(tag, adInfo, errorInfo);
    }
    public static void printApplovinAdInfo<T, T1>(string tag, T adInfo, T1 errorInfo)
    {
        string _tag = "[[" + tag + "]]";
        string _adInfo = "";
        string _ironSourceError = "";
        if (adInfo != null)
        {
            _adInfo = " | " + adInfo.ToString();
        }
        if (errorInfo != null)
        {
            _ironSourceError = " | " + errorInfo.ToString();

        }
        Debug.Log(_tag + _adInfo + _ironSourceError);
    }

    private static void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        printApplovinAdInfo("Banner Ad Loaded Event", adInfo);
    }

    private static void OnBannerAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        printApplovinAdInfo("Banner Ad Load Failed Event", null, errorInfo);
        IsBannerLoaded = false;

    }

    private static void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        printApplovinAdInfo("Banner Ad Clicked Event", adInfo);

    }

    private static void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        printApplovinAdInfo("Banner Ad Revenue Paid Event", adInfo);

    }

    private static void OnBannerAdExpandedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        printApplovinAdInfo("Banner Ad Expanded Event", adInfo);
        IsBannerLoaded = true;

    }

    private static void OnBannerAdCollapsedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        printApplovinAdInfo("Banner Ad Collapsed Event", adInfo);
        IsBannerLoaded = false;

    }

    internal static void BannerIsLoaded(string adUnitId)
    {
        _isBannerLoaded = true;

        if (_showBannerAsked)
        {
            ShowBannerAd();
        }
    }

    internal static void ShowBannerAd()
    {
        // if (_USE_BANNER_ADMOBOnly)
        // {
        //     if (_bannerView == null)
        //     {
        //         return;
        //     }
        //     var requiest = new AdRequest.Builder();
        //     _bannerView.LoadAd(requiest.Build());
        // }
        // else
        // {
        if (isAdmobInitialized)
        {
            if (_bannerView == null)
            {
                Debug.Log("Banner not initialized yet");

                return;
            }

            Debug.Log("Showiiing Banner");

            // MaxSdk.ShowBanner(_bannerAdUnitId);
            var adRequest = new AdRequest();

            // send the request to load the ad.
            Debug.Log("Loading banner ad.");
            if (_bannerView.IsDestroyed)
            {
                InitializeBannerAds();
            }
            _bannerView.LoadAd(adRequest);
            _isBannerShowing = true;
            // }
            FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Banner);
        }
        else
        {

            if (_isBannerShowing)
            {
                return;
            }

            if (_showBannerAsked == false)
            {
                _showBannerAsked = true;
            }

            if (_isBannerLoaded == false)
            {
                return;
            }

            Debug.Log("Showiiing Banner");

            MaxSdk.ShowBanner(_bannerAdUnitId);
            _isBannerShowing = true;
            // }
            FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Banner);
        }
    }

    internal static void HideBannerAd()
    {
        if (_isBannerLoaded == false)
        {
            return;
        }
        if (isAdmobInitialized)
        {
            Debug.Log("Destroying banner ad.");
            _bannerView.Destroy();
            _bannerView = null;

            _isBannerShowing = false;
            _showBannerAsked = false;
        }
        else
        {


            MaxSdk.HideBanner(_bannerAdUnitId);
            _isBannerShowing = false;
            _showBannerAsked = false;
        }
    }

    internal static bool IsRewardedAdReady()
    {
        if (isAdmobInitialized)
        {
            return rewardedAd != null && rewardedAd.CanShowAd();
        }
        else
        {
            return MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);
        }
    }

    internal static void LoadAds()
    {
        ShowBannerAd();
        LoadRewardedAd();
        LoadInterstitial();
    }

    public static bool HasInterstitial()
    {
        if (isAdmobInitialized)
        {
            return interstitialAd != null && interstitialAd.CanShowAd();
        }
        else
        {
            return MaxSdk.IsInterstitialReady(_interstitialAdUnitId);
        }
    }
    public static bool HasRewardedVideo()
    {
        if (isAdmobInitialized)
        {
            return rewardedAd != null && rewardedAd.CanShowAd();
        }
        else
        {
            return MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);
        }
    }

    internal static void ShowRewarded(Action<string, string, int> callback, string callbackArgsString = "", int callbackArgsInt = 0)
    {
        _rewardedCallback = callback;
        _rewardedCallbackArgString = callbackArgsString;
        _rewardedCallbackArgInt = callbackArgsInt;

        if (isAdmobInitialized)
        {
            if (rewardedAd != null && rewardedAd.CanShowAd())
            {
                try
                {
                    rewardedAd.Show((Reward reward) =>
                    {
                        // TODO: Reward the user.
                        collected = true;
                    });
                    FunGamesAnalytics.NewDesignEvent("Rewarded" + callbackArgsString, "succeeded");
                    callback?.Invoke("succeeded", callbackArgsString, callbackArgsInt);
                }
                catch (Exception e)
                {
                    callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                    FunGamesAnalytics.NewDesignEvent("RewardedError" + callbackArgsString, "UserQuitBeforeEndingAd");
                    Debug.Log(e);
                    throw;
                }
            }
            else
            {
                callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                FunGamesAnalytics.NewDesignEvent("RewardedNoAd" + callbackArgsString, "NoAdReady");
                _rewardedCallback = null;
            }
        }
        else
        {
            if (MaxSdk.IsRewardedAdReady(_rewardedAdUnitId))
            {
                try
                {

                    MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
                    FunGamesAnalytics.NewDesignEvent("Rewarded" + callbackArgsString, "succeeded");
                    callback?.Invoke("succeeded", callbackArgsString, callbackArgsInt);
                }
                catch (Exception e)
                {
                    callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                    FunGamesAnalytics.NewDesignEvent("RewardedError" + callbackArgsString, "UserQuitBeforeEndingAd");
                    Debug.Log(e);
                    throw;
                }
            }
            else
            {
                callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                FunGamesAnalytics.NewDesignEvent("RewardedNoAd" + callbackArgsString, "NoAdReady");
                _rewardedCallback = null;
            }
        }
    }
    public enum AdState
    {
        succeeded,
        rewarded,
        failed
    }

    internal static void ShowAd(bool isReward, Action<AdState> callback)
    {
        if (isReward)
        {
            ShowRewarded((state, argStr, argInt) =>
            {
                if (state == "reward")
                {
                    callback?.Invoke(AdState.rewarded);
                }
                else if (state == "success")
                {
                    callback?.Invoke(AdState.succeeded);

                }
                else if (state == "fail")
                {
                    callback?.Invoke(AdState.failed);

                }
                // else
                // {
                //     callback?.Invoke(AdState.failed);

                // }
            });
        }
        else
        {

            ShowInterstitial((state, argStr, argInt) =>
            {
                if (state == "success")
                {
                    callback?.Invoke(AdState.succeeded);

                }
                else if (state == "fail")
                {
                    callback?.Invoke(AdState.failed);

                }
                else
                {
                    callback?.Invoke(AdState.failed);

                }
            });
        }
    }
    internal static void ShowInterstitial(Action<string, string, int> callback, string callbackArgsString = "", int callbackArgsInt = 0)
    {
        _interstitialCallback = callback;
        _interstitialCallbackArgString = callbackArgsString;
        _interstitialCallbackArgInt = callbackArgsInt;

        if (isAdmobInitialized)
        {
            if (interstitialAd != null && interstitialAd.CanShowAd())
            {
                try
                {
                    Debug.Log("Showing interstitial ad.");
                    interstitialAd.Show();

                }
                catch (Exception e)
                {
                    callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                    FunGamesAnalytics.NewDesignEvent("Error", "UserQuitBeforeEndingAd");
                    Debug.Log(e);
                    throw;
                }
            }
            else
            {
                callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                _interstitialCallback = null;
            }
        }
        else
        {
            if (MaxSdk.IsInterstitialReady(_interstitialAdUnitId))
            {
                try
                {
                    MaxSdk.ShowInterstitial(_interstitialAdUnitId);
                }
                catch (Exception e)
                {
                    callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                    FunGamesAnalytics.NewDesignEvent("Error", "UserQuitBeforeEndingAd");
                    Debug.Log(e);
                    throw;
                }
            }
            else
            {
                callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
                _interstitialCallback = null;
            }
        }
    }

    void InterstitialCallbackFunc(string status, string argString, int argInt)
    {
        FunGamesAnalytics.NewDesignEvent("InterstitialComplete");
    }
}
