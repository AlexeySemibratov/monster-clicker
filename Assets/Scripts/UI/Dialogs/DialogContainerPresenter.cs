using UniRx;
using UnityEngine;

public class DialogContainerPresenter : MonoBehaviour
{
    private const string LocalPlayerName = "You";

    [SerializeField]
    private GameLoop _gameLoop;

    [SerializeField]
    private DialogPopup _winDialog;

    [SerializeField]
    private DialogPopup _looseDialog;

    private void Awake()
    {
        _gameLoop.GameResult
            .Subscribe(result => HandleGameResult(result))
            .AddTo(this);
    }

    private void HandleGameResult(GameResult result)
    {
        if (result.IsWin)
        {
            SaveResult(result);
            _winDialog.ShowDialog();
        }
        else
        {
            _looseDialog.ShowDialog();
        }
    }

    private void SaveResult(GameResult result)
    {
        var resultItem = new LeaderboardItem
        {
            name = LocalPlayerName,
            score = result.Time
        };

        LeaderboardsRepositoryProvider.Get()
            .SaveResult(resultItem);
    }
}
