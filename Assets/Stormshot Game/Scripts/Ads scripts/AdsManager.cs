using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;

public class AdsManager : MonoBehaviour
{

    public static Action onRewardedAdClosed, onUserEarnedReward;
    public static AdsManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void Start()
    {
        Advertisements.Instance.Initialize();
        Invoke("InitOpenApp", 0.5f);
        Invoke("ShowBanner", 1);
    }
    void InitOpenApp()
    {
        AppOpenAdManager.Instance.LoadAd();
        AppStateEventNotifier.AppStateChanged += OnAppStateChanged;

    }
    public void OnAppStateChanged(AppState state)
    {
        if (state == AppState.Foreground)
        {
            // COMPLETE: Show an app open ad if available.
            AppOpenAdManager.Instance.ShowAdIfAvailable();
        }
    }

    public void RequestBanner()
    {
        ShowBanner();
    }

    public static float timer = 30;
    public void ShowInterstitialTimer(Action action)
    {
        if (Time.time < timer)
        {
            print(Time.time);
            action?.Invoke();
            return;
        }
        timer = Time.time + 30;
        ShowIntersitial(() =>
        {
            action?.Invoke();
        });
    }

    public void ShowBanner()
    {
        Advertisements.Instance.ShowBanner(BannerPosition.BOTTOM);
    }

    public void DestroyBanner()
    {
        Advertisements.Instance.HideBanner();
    }

    public bool ShowInterstitial()
    {
        if (Advertisements.Instance.IsInterstitialAvailable())
        {
            Advertisements.Instance.ShowInterstitial(() =>
            {
                //CUtils.SetActionTime("show_ads");
            });
            return true;
        }
        return false;
    }

    public bool ShowRewardedAd()
    {
        if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo((b) =>
            {

                onUserEarnedReward?.Invoke();
                print("HandleRewardedAdRewarded event received for ");

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
    int showCountIntertitial;
    int showCountReward;
    public void ShowIntersitialTwoTimes(UnityAction CompleteMethod)
    {
        if (showCountIntertitial >= 2 && Advertisements.Instance.IsInterstitialAvailable())
        {
            if (showCountReward >= 4)
            {
                Advertisements.Instance.ShowRewardedVideo((b) =>
                {
                    CompleteMethod?.Invoke();
                });
                showCountReward = 0;
            }
            else
            {
                Advertisements.Instance.ShowInterstitial(CompleteMethod);
            }
            showCountIntertitial = 0;
        }
        else
        {
            CompleteMethod?.Invoke();
        }
        showCountIntertitial++;
        showCountReward++;
    }
    public void ShowIntersitial(UnityAction CompleteMethod)
    {
        if (Advertisements.Instance.IsInterstitialAvailable())
        {
            Advertisements.Instance.ShowInterstitial(CompleteMethod);
        }
        else
        {
            CompleteMethod?.Invoke();
        }
    }
    public void ShowReward(UnityAction<bool> CompleteMethod)
    {
        if (Advertisements.Instance.IsRewardVideoAvailable())
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        }
        else
        {
            CompleteMethod?.Invoke(false);
        }
    }
}
