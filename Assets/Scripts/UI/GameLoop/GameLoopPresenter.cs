using TMPro;
using UniRx;
using UnityEngine;

public class GameLoopPresenter : MonoBehaviour
{
    private const string TimerText = "Time left: {0}";
    private const string EnemiesLeftText = "Enemies left: {0}";

    [SerializeField]
    private TextMeshProUGUI _enemiesLeftText;

    [SerializeField]
    private TextMeshProUGUI _timerText;

    [SerializeField]
    private GameLoop _gameLoop;

    [SerializeField]
    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        SetupTimerText();
        SetupEnemiesLeftText();
    }


    private void SetupTimerText()
    {
        _gameLoop.CurrentTime
            .ObserveOnMainThread()
            .Subscribe(time => _timerText.text = string.Format(TimerText, time))
            .AddTo(this);
    }

    private void SetupEnemiesLeftText()
    {
        _enemySpawner.EnemiesLeft
            .ObserveOnMainThread()
            .Subscribe(count => _enemiesLeftText.text = string.Format(EnemiesLeftText, count))
            .AddTo(this);
    }
}
