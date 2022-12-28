using System;
using UniRx;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public IReadOnlyReactiveProperty<int> CurrentTime { get => _currentGameTime; }

    public IObservable<GameResult> GameResult { get => _gameResult; }


    private const int TimerPeriodSecond = 1;

    [SerializeField]
    private int _gameTimeSeconds = 10;

    [SerializeField]
    private int _continueTimeSeconds = 5;

    [SerializeField]
    private EnemySpawner _enemySpawner;

    [SerializeField]
    private PlayerClicker _playerClicker;

    private Subject<GameResult> _gameResult = new Subject<GameResult>();

    private IReactiveProperty<int> _currentGameTime = new ReactiveProperty<int>();

    private IDisposable _gameTimer;

    private int _totalGameTime;

    public void Restart()
    {
        RestartTimer(_gameTimeSeconds);
        _totalGameTime = 0;
        _enemySpawner.Restart();
        _playerClicker.Enable();
    }

    public void Continue()
    {
        RestartTimer(_continueTimeSeconds);
        _playerClicker.Enable();
    }

    private void Awake()
    {
        _currentGameTime.Value = _gameTimeSeconds;

        _enemySpawner.Compelted
            .Subscribe(_ => CompleteGame())
            .AddTo(this);

        Restart();
    }

    private void RestartTimer(int fromSeconds)
    {
        StopTimer();

        _gameTimer = Timer
            .From(fromSeconds, TimerPeriodSecond)
            .Subscribe(
                onNext: time => UpdateTime((int)time),
                onCompleted: () => LooseGame())
            .AddTo(this);
    }

    private void UpdateTime(int value)
    {
        _totalGameTime++;
        _currentGameTime.Value = value;
    }

    private void CompleteGame()
    {
        var result = new GameResult
        {
            IsWin = true,
            Time = _totalGameTime
        };

        _gameResult.OnNext(result);

        OnGameEnded();
    }

    private void LooseGame()
    {
        var result = new GameResult
        {
            IsWin = false,
            Time = _totalGameTime
        };

        _gameResult.OnNext(result);

        OnGameEnded();
    }

    private void OnGameEnded()
    {
        _playerClicker.Disable();
        StopTimer();
    }

    private void StopTimer()
    {
        if (_gameTimer != null)
            _gameTimer.Dispose();
    }
}
