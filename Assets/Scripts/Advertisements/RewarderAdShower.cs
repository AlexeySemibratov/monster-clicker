using GoogleMobileAds.Api;
using System;
using UniRx;
using UnityEngine;

public class RewarderAdShower : MonoBehaviour
{
    private const string RewardedAdId = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd _rewardedAd;
    private Subject<RewardedAdResult> _resultSubject;

    public IObservable<RewardedAdResult> RequestAdShowing()
    {
        CreateRewardedAd();

        AdRequest request = new AdRequest.Builder().Build();
       
        _rewardedAd.LoadAd(request);

        return _resultSubject;
    }

    private void OnDisable()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        }
    }

    private void CreateRewardedAd()
    {
        _rewardedAd = new RewardedAd(RewardedAdId);
        _resultSubject = new Subject<RewardedAdResult>();

        _rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        _rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        _rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Show();
        }
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        _resultSubject.OnNext(RewardedAdResult.Error);
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        _resultSubject.OnNext(RewardedAdResult.Error);
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        _resultSubject.OnNext(RewardedAdResult.UserRewarded);
    }
}
