using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class LoadLeaderboardsFromRemote
{
    private const string URL = "https://taptics.b-cdn.net/files/leaderboard.json";

    public IObservable<IEnumerable<LeaderboardItem>> Load()
    {
        return UnityWebRequest.Get(URL)
            .SendWebRequest()
            .AsAsyncOperationObservable()
            .Select(result => MapDataFromJson(result.webRequest.downloadHandler.text));
    }

    private IEnumerable<LeaderboardItem> MapDataFromJson(string rawJson)
    {
        string jsonObjectString = "{\"Items\":" + rawJson + "}";
        LeaderboardsModel wrappedItems = JsonUtility.FromJson<LeaderboardsModel>(jsonObjectString);
        return wrappedItems.Items;
    }
}
