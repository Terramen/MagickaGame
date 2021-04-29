using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.Advertisements;
public class BannerAd : MonoBehaviour, IUnityAdsListener { 

    string gameId = "3916283";
    public string placementId = "bannerPlacement";
    public string placementVideoId = "video";
    public string placementRewardId = "rewardedVideo";
    public bool testMode = true;

    void Start () {
        Advertisement.Initialize(gameId, testMode);
        /*Advertisement.Banner.SetPosition (BannerPosition.BOTTOM_CENTER);
        StartCoroutine(ShowBannerWhenInitialized());*/
    }

    /*IEnumerator ShowBannerWhenInitialized () {
        while (!Advertisement.isInitialized) {
            yield return new WaitForSeconds(0.5f);
        }
        Advertisement.Banner.Show (placementId);
    }*/

    public void ShowVideo()
    {
        if (Advertisement.IsReady(placementVideoId)) {
            Advertisement.Show(placementVideoId);
        } 
        else {
            Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        if (placementId == placementVideoId) {
            // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished) {
            
        } else if (showResult == ShowResult.Skipped) {
            // Do not reward the user for skipping the ad.
        } else if (showResult == ShowResult.Failed) {
            Debug.LogWarning ("The ad did not finish due to an error.");
        }
    }
}
