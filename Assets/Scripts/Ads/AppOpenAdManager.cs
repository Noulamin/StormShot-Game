// Copyright 2021 Google LLC
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//      https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
// using GoogleMobileAds.Api;
using UnityEngine;
using Yodo1.MAS;

public class AppOpenAdManager
{

    // public string AD_UNIT_ID => FunGamesMax._appopenAdmobAdUnitId;


    private static AppOpenAdManager instance;

    // COMPLETE: Add loadTime field
    private DateTime loadTime;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }


    private Yodo1U3dAppOpenAd appOpenAd;

    public void Start()
    {
        this.RequestAppOpen();
    }

    public void ShowAppOpen()
    {
        appOpenAd.ShowAd();
    }


    public void RequestAppOpen()
    {
        appOpenAd = Yodo1U3dAppOpenAd.GetInstance();

        // Ad Events
        appOpenAd.OnAdLoadedEvent += OnAppOpenAdLoadedEvent;
        appOpenAd.OnAdLoadFailedEvent += OnAppOpenAdLoadFailedEvent;
        appOpenAd.OnAdOpenedEvent += OnAppOpenAdOpenedEvent;
        appOpenAd.OnAdOpenFailedEvent += OnAppOpenAdOpenFailedEvent;
        appOpenAd.OnAdClosedEvent += OnAppOpenAdClosedEvent;
        appOpenAd.LoadAd();
        AdsManager.isAppOpenInitiaed = true;
    }

    private void OnAppOpenAdLoadedEvent(Yodo1U3dAppOpenAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnAppOpenAdLoadedEvent event received");
        ad.ShowAd();
    }

    private void OnAppOpenAdLoadFailedEvent(Yodo1U3dAppOpenAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnAppOpenAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnAppOpenAdOpenedEvent(Yodo1U3dAppOpenAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnAppOpenAdOpenedEvent event received");
    }

    private void OnAppOpenAdOpenFailedEvent(Yodo1U3dAppOpenAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnAppOpenAdOpenFailedEvent event received with error: " + adError.ToString());
    }

    private void OnAppOpenAdClosedEvent(Yodo1U3dAppOpenAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnAppOpenAdClosedEvent event received");
    }

}
