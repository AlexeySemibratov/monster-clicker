using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UniRx;
using UnityEngine;

public class WinDialogPresenter : MonoBehaviour
{
    private const string LoadingText = "Loading Leaderboards...";

    private const string LeaderboardItemText = "{0}. {1}: {2}";
    private const string PlayerScoreText = "Your score is {0}";

    private const int LeaderboardItemCount = 10;

    [SerializeField]
    private TextMeshProUGUI _playerScoreText;

    [SerializeField]
    private TextMeshProUGUI _leaderboardsDataText;

    [SerializeField]
    private DialogPopup _dialogPopup;

    [SerializeField]
    private GameLoop _gameLoop;

    private CompositeDisposable _disposables = new();

    public void OnRestartClicked()
    {
        _gameLoop.Restart();
        _dialogPopup.HideDialog();
    }

    private void OnEnable()
    {
        ShowLeaderboardsData();
        SetPlayerScore();
    }

    private void OnDisable()
    {
        _disposables.Clear();
    }

    private void SetPlayerScore()
    {
        LeaderboardItem lastResultItem = LeaderboardsRepositoryProvider.Get().GetLastPlayerResult();

        if (lastResultItem == null)
            throw new ArgumentException("No last player game result was found.");

        _playerScoreText.text = string.Format(PlayerScoreText, lastResultItem.score);
    }

    private void ShowLeaderboardsData()
    {
        ShowLoadingText();

        LeaderboardsRepositoryProvider.Get()
            .LoadData()
            .ObserveOnMainThread()
            .Subscribe(data => ShowLeaderBoards(data))
            .AddTo(_disposables);
    }

    private void ShowLeaderBoards(IEnumerable<LeaderboardItem> data)
    {
        IEnumerable<LeaderboardItem> sortedData = data
            .OrderBy(item => item.score)
            .Take(LeaderboardItemCount);

        _leaderboardsDataText.text = FormatItems(sortedData);
    }

    private void ShowLoadingText()
    {
        _leaderboardsDataText.text = LoadingText;
    }

    private string FormatItems(IEnumerable<LeaderboardItem> items)
    {
        IEnumerable<string> strings = items.Select(item => string.Format(LeaderboardItemText, item.name, item.score));
        var result = new StringBuilder();

        for (int i = 0; i < items.Count(); i++)
        {
            LeaderboardItem item = items.ElementAt(i);
            result.AppendLine(string.Format(LeaderboardItemText, i + 1, item.name, item.score));
        }

        return result.ToString();   
    }
}
