using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

public class LeaderboardsRepository
{
    private LeaderboardsRemoteDataSource _loadFromRemote = new();

    private List<LeaderboardItem> _localItems = new List<LeaderboardItem>();

    private LeaderboardItem _lastPlayerResult;

    public IObservable<IEnumerable<LeaderboardItem>> LoadData()
    {
        IObservable<IEnumerable<LeaderboardItem>> localSource = Observable.Return(_localItems);
        IObservable<IEnumerable<LeaderboardItem>> remoteSource = _loadFromRemote.Load();

        return Observable.CombineLatest(
                localSource,
                remoteSource,
                (local, remote) => local.Concat(remote));
    }

    public void SaveResult(LeaderboardItem item)
    {
        _lastPlayerResult = item;
        _localItems.Add(item);
    }

    public LeaderboardItem GetLastPlayerResult()
    {
        return _lastPlayerResult;
    }
}
