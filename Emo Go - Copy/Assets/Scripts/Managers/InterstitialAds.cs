using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Analytics;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class InterstitialAds : MonoBehaviour
{



    private InterstitialAd interstitialAd;

    private int interAdsWatched;


    // test ad id: "ca-app-pub-3940256099942544/1033173712"
    // final ad id: "ca-app-pub-7418823270776132/7761894460"
    private string _adUnitId = "ca-app-pub-7418823270776132/6759372786";


    public static InterstitialAds instance;





    private void Awake()
    {
        MobileAds.Initialize(initStatus => { });

        instance = this;
    }





    private void Start()
    {
        interAdsWatched = PlayerPrefs.GetInt("inter_ads", 0);

        LoadInterstitialAd();

        DontDestroyOnLoad(gameObject);
    }






    private void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        // create our request used to load th9090e ad.
        var adRequest = new AdRequest.Builder().Build();


        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {

                    return;
                }

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }





    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interAdsWatched++;
            PlayerPrefs.SetInt("inter_ads", interAdsWatched);

            if (interAdsWatched == 1)
            {
                FirebaseManager.instance.LogEvent("inter_1");
            }
            else if (interAdsWatched == 5)
            {
                FirebaseManager.instance.LogEvent("inter_5");
            }
            else if (interAdsWatched == 25)
            {
                FirebaseManager.instance.LogEvent("inter_25");
            }

            interstitialAd.Show();
        }
        else
        {
            LoadInterstitialAd();
        }
    }





    private void RegisterEventHandlers(InterstitialAd ad)
    {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            LoadInterstitialAd();
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            LoadInterstitialAd();
            SceneManager.LoadScene(PlayerPrefs.GetInt("CurrentLevel"));
        };
    }
}