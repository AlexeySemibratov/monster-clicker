using UniRx;
using UnityEngine;

[RequireComponent(typeof(EnemyAnimator), typeof(HealthComponent))]
public class Enemy : MonoBehaviour
{
    public int Level { get => _level; }

    public HealthComponent Health { get => _health; }


    [SerializeField]
    private int _level = 1;

    private HealthComponent _health;

    private EnemyAnimator _animator;

    private CompositeDisposable _disposables = new();

    public void SetLevel(int level)
    {
        _level = level;
        _health.UpdateMaxHpFromLevel(level);
    }

    public void Hit(int damage)
    {
        _health.ReduceHp(damage);
    }

    public void Enable()
    {
        enabled = true;
        _animator.Restart();
    }

    private void Awake()
    {
        _animator = GetComponent<EnemyAnimator>();
        _health = GetComponent<HealthComponent>();

        enabled = false;
    }

    private void OnEnable()
    {
        ObserveHealthEvents();
    }

    private void OnDisable()
    {
        _disposables.Clear();
    }

    private void ObserveHealthEvents()
    {
        _health.HealthEvents
            .ObserveOnMainThread()
            .Subscribe(e => HandleHealthEvent(e))
            .AddTo(_disposables);
    }

    private void HandleHealthEvent(HealthEvent e)
    {
        switch (e)
        {
            case HealthEvent.HpReduced:
                _animator.PlayHit();
                break;
            case HealthEvent.Dead:
                _animator.PlayDead();
                OnDead();
                break;
            default:
                break;
        }
    }

    private void OnDead()
    {
        enabled = false;
    }
}
