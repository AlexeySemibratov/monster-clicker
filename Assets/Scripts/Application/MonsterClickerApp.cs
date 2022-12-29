using GoogleMobileAds.Api;
using UnityEngine;

public class MonsterClickerApp : MonoBehaviour
{
    public static MonsterClickerApp Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            InitializeAds();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAds()
    {
        MobileAds.Initialize(_ => { });
    }
}
