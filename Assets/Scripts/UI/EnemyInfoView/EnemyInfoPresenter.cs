using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInfoPresenter : MonoBehaviour
{
    private const string LevelText = "Level {0}";

    private const int FullAlpha = 1;
    private const int MinAlpha = 0;

    [SerializeField]
    private EnemySpawner _spawner;

    [SerializeField]
    private Slider _hpSlider;

    [SerializeField]
    private TextMeshProUGUI _enemyLevelText;

    [SerializeField]
    private CanvasGroup _canvasGroup;

    private CompositeDisposable _enemyInfoDisposable = new();

    private void Awake()
    {
        HideView();
        ObserveSpawnerEvents();
    }

    private void OnDestroy()
    {
        _enemyInfoDisposable.Dispose();
    }

    private void HideView()
    {
        _canvasGroup.alpha = MinAlpha;
    }

    private void ShowView()
    {
        _canvasGroup.alpha = FullAlpha;
    }

    private void ObserveSpawnerEvents()
    {
        _spawner.EnemySpawned
            .ObserveOnMainThread()
            .Subscribe(enemy => BindEnemyToView(enemy))
            .AddTo(this);
    }

    private void BindEnemyToView(Enemy enemy)
    {
        _enemyLevelText.text = string.Format(LevelText, enemy.Level);
        _hpSlider.maxValue = enemy.Health.MaxHP;

        ShowView();

        ObserveEnemyHealthEvents(enemy.Health);
    }

    private void ObserveEnemyHealthEvents(HealthComponent health)
    {
        _enemyInfoDisposable.Clear();

        health.CurrentHP
            .ObserveOnMainThread()
            .Subscribe(hp => _hpSlider.value = hp)
            .AddTo(_enemyInfoDisposable);

        health.IsDead
            .Where(isDead => isDead)
            .Subscribe(hp => OnEnemyDead())
            .AddTo(_enemyInfoDisposable);
    }

    private void OnEnemyDead()
    {
        HideView();
        _spawner.RequestNextSpawn();
    }
}
