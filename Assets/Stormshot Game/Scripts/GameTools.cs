using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class GameTools : MonoBehaviour
{
    public Transform Aim_target;
    private RewardedAd rewardedAd;
    private BannerView bannerView;

    void Start()
    {
        string adUnitId = "ca-app-pub-7024820506501610/4203727138";

        this.rewardedAd = new RewardedAd(adUnitId);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded ad with the request.
        this.rewardedAd.LoadAd(request);



        MobileAds.Initialize(initStatus => { });

        this.RequestBanner();
    }

    private void RequestBanner()
    {
        #if UNITY_ANDROID
            string adUnitId = "ca-app-pub-3940256099942544/6300978111";
        #elif UNITY_IPHONE
            string adUnitId = "ca-app-pub-3940256099942544/2934735716";
        #else
            string adUnitId = "unexpected_platform";
        #endif

        // Create a 320x50 banner at the top of the screen.
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this.bannerView.LoadAd(request);
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.LookAt(Aim_target);
    }

    public void reload_scene()
    {
        if (this.rewardedAd.IsLoaded()) {
            this.rewardedAd.Show();
        }

        InvokeRepeating("reload",0.3f,0.3f);

    }

    void reload()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<LevelManager>().game_over = false;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);

    }
}
