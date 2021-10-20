using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class Ads : MonoBehaviour
{
    private int mixAdCount = 0;
    private int initAdCount = 0;
    public InterstitialAd interstitial;
    private string adUnitID = "ca-app-pub-3344717684064061/9465249642";
    public void LoadAd()
    {
        if (interstitial != null)
        {
            interstitial.Destroy();
        }
        interstitial=new InterstitialAd(adUnitID);
        AdRequest request = new AdRequest.Builder().Build();
        interstitial.LoadAd(request);
    }

    public void ShowAdMix()
    {
        if (mixAdCount >= 2)
        {
            if (interstitial.IsLoaded())
            {
                interstitial.Show();
                mixAdCount = 0;
            }
            if (!interstitial.IsLoaded())
            {
                LoadAd();
            }
        }
        mixAdCount++;
    }
    
    public void ShowAdInit()
    {
        if (initAdCount >= 2)
        {
            if (GetComponent<Ads>().interstitial.IsLoaded())
            {
                GetComponent<Ads>().interstitial.Show();
            }
            initAdCount = 0; 
            if (!interstitial.IsLoaded())
            {
                LoadAd();
            }
        }
        initAdCount++;
    }
}
