using System;
using UniRx;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public IObservable<Enemy> EnemySpawned { get => _enemySpawned; }

    public IObservable<int> EnemiesLeft { get => _enemiesLeft; }

    public IObservable<Unit> Compelted { get => _compelted; }


    private const float SpawnDelay = 1.0f;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    [SerializeField]
    private int _startFromLevel;

    [SerializeField]
    private int _maxLevel;

    private IReactiveProperty<int> _enemiesLeft = new ReactiveProperty<int>();
    private IReactiveProperty<int> _spawnedEnemyLevel = new ReactiveProperty<int>();

    private Subject<Unit> _compelted = new Subject<Unit>();

    private ReplaySubject<int> _spawnEnemyWithLevel = new ReplaySubject<int>();
    private ReplaySubject<Enemy> _enemySpawned = new ReplaySubject<Enemy>();

    void Awake()
    {
        SpawnEnemies();
    }

    public void Restart()
    {
        _enemyFactory.Restart();
        _spawnEnemyWithLevel.OnNext(_startFromLevel);
        _enemiesLeft.Value = _maxLevel;
    }

    public void RequestNextSpawn()
    {
        SpawnNextEnemyOrComplete();
    }

    private void SpawnEnemies()
    {
        _spawnEnemyWithLevel
            .Delay(TimeSpan.FromSeconds(SpawnDelay))
            .Subscribe(level => SpawnEnemyWithLevel(level))
            .AddTo(this);
    }

    private void SpawnEnemyWithLevel(int level)
    {
        Enemy enemy = _enemyFactory.CreateEnemy(level);

        _enemySpawned.OnNext(enemy);
        _spawnedEnemyLevel.Value = level;
    }

    private void SpawnNextEnemyOrComplete()
    {
        int nextLevel = _spawnedEnemyLevel.Value + 1;
        _enemiesLeft.Value = _enemiesLeft.Value - 1;

        if (nextLevel > _maxLevel)
        {
            _compelted.OnNext(Unit.Default);
        }
        else
        {
            _spawnEnemyWithLevel.OnNext(nextLevel);
        }
    }
}
