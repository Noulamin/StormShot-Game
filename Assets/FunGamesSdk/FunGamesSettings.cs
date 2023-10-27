using Unity.Collections;
using UnityEngine;

namespace FunGamesSdk
{
    [CreateAssetMenu(fileName = "Assets/Resources/FunGamesSettings", menuName = "FunGamesSdk/Settings", order = 1000)]
    public class FunGamesSettings : ScriptableObject
    {
        static FunGamesSettings _funGamesSettings;
        public static FunGamesSettings LoadedSettings
        {
            get
            {
                if (_funGamesSettings == null)
                {
                    _funGamesSettings = Resources.Load<FunGamesSettings>("FunGamesSettings");
                }
                return _funGamesSettings;
            }
        }
        // [Header("Editor Testings")]
        // public bool TurnOnIsSubscribed;
        // public bool TurnOnIsSDKInitialized;
        public bool UnLockAllLevel = true;
        [Header("Sdk Version")]

        [Tooltip("Sdk Version")]
        [ReadOnly] public string version = "2.5";


        [Header("AppTrackingTransparency (iOS 14)")]

        [Tooltip("UseATTPopup")]
        public bool useATTPopup;


        [Header("PushNotifications")]

        [Tooltip("Use PushNotifications")]
        public bool usePushNotifications;


        [Header("Firebase")]
        public bool SignInAnonymously;

        [Header("AppsFlyer")]

        [Tooltip("Use AppsFlyer")]
        public bool useAppsFlyer;
        public string devkey;
        public string appID;
        public bool isDebug;

        // [Header("In App Purchase")]
        // public string PrivacyPolicyLink = "";
        // public string TermsAndConditionLink = "";
        // [Header("RevenueCat")]
        // public string RevenueCat_API_KEY = "sk_LeNBhbYJmpYvyiNzVXAAJMQkqXEuC";

        // [Header("In App Purchase")]
        // public string NoAdsSubscriptionProductId = "productId";

        // [Header("Facebook")]

        // [Tooltip("Use Facebook")]
        // public bool useFacebook;

        // [Tooltip("Facebook Game ID")]
        // public string facebookGameID;


        [Header("GameAnalytics")]

        [Tooltip("Use GameAnalytics")]
        public bool useGameAnalytics;

        // [Tooltip("GameAnalytics Ios Game Key")]
        // public string gameAnalyticsIosGameKey;

        // [Tooltip("GameAnalytics Ios Secret Key")]
        // public string gameAnalyticsIosSecretKey;

        // [Tooltip("GameAnalytics Android Game Key")]
        // public string gameAnalyticsAndroidGameKey;

        // [Tooltip("GameAnalytics Android Secret Key")]
        // public string gameAnalyticsAndroidSecretKey;


        [Header("Tenjin")]

        [Tooltip("Use Tenjin")]
        public bool useTenjin;

        [Tooltip("Tenjin Api Key")]
        public string tenjinApiKeyAndroid = "";
        public string tenjinApiKeyIOS = "";

        public string tenjinApiKey =>
#if UNITY_EDITOR || UNITY_ANDROID
        tenjinApiKeyAndroid
#else
        tenjinApiKeyIOS
#endif
        ;


        [Header("CrossPromo")]

        [Tooltip("Play Local Videos")]
        public bool playLocalVideos;

        [Tooltip("Play Remote Videos")]
        public bool playRemoteVideos;


        [Header("Ogury")]
        [Tooltip("Use Ogury")]
        public bool useOgury;


        [Header("Ogury iOS")]

        [Tooltip("ogury iOS AssetKey")]
        public string oguryIOSAssetKey;

        [Tooltip("iOS thumbnail AdUnitId")]
        public string iOSThumbnailAdUnitId;


        [Header("Ogury Android")]

        [Tooltip("ogury Android AssetKey")]
        public string oguryAndroidAssetKey;

        [Tooltip("Android thumbnail AdUnitId")]
        public string androidThumbnailAdUnitId;

        [Header("GDPR")]
        [Tooltip("Use GDPR")]
        public bool useGdpr;


        // [Header("Google Admob")]
        // [Tooltip("Use Appopen Admob Only it will Disable Applovin AppOpen From Showing")]
        // public bool Use_Appopen_AdmobOnly;
        // public string IOSAppopenAdmobAdUnitId = "ca-app-pub-3940256099942544/3419835294";
        // public string androidAppopenAdmobAdUnitId = "ca-app-pub-3940256099942544/3419835294";
        // public bool Use_Banner_AdmobOnly;
        // public string IOSBannerAdmobAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        // public string androidBannerAdmobAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        // [Header("Admob iOS")]
        // public string AdmobiOSInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        // public string AdmobiOSRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        // public string AdmobiOSBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        // public string AdmobIOSAppopenAdUnitId = "ca-app-pub-3940256099942544/3419835294";

        // [Header("Admob Android")]
        // public string AdmobandroidInterstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
        // public string AdmobandroidRewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
        // public string AdmobandroidBannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
        // public string AdmobandroidAppopenAdUnitId = "ca-app-pub-3940256099942544/3419835294";
        [Header("Ads Settings")]
        public BannerPosition bannerPosition = BannerPosition.BOTTOM;
        [Header("Applovin Max")]

        [Tooltip("Use Max")]
        public bool useMax;

        [Tooltip("Max Sdk Key")]
        public string maxSdkKey = "-x3h7mcZ5EdJJCd0iDab_rNf-6t9bsentb_ilJcaZ_ORIGB0P4reTeRrMeRe39-EAu-F6Bqcgah9fv-gSdoO1U";

        [Header("Max iOS")]
        public string iOSInterstitialAdUnitId;
        public string iOSRewardedAdUnitId;
        public string iOSBannerAdUnitId;
        public string IOSAppopenAdUnitId;

        [Header("Max Android")]
        public string androidInterstitialAdUnitId;
        public string androidRewardedAdUnitId;
        public string androidBannerAdUnitId;
        public string androidAppopenAdUnitId;
    }
}