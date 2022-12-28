using System;
using UniRx;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public IReadOnlyReactiveProperty<int> CurrentHP => _currentHP.ToReactiveProperty();

    public IReadOnlyReactiveProperty<bool> IsDead { get; private set; }

    public IObservable<HealthEvent> HealthEvents => _healthEvents;

    public int MaxHP { get => _maxHP; }


    private const int BaseHPAmount = 50;
    private const int LevelHPMultiplier = 20;

    private int _maxHP;
    private IntReactiveProperty _currentHP;

    private Subject<HealthEvent> _healthEvents = new Subject<HealthEvent>();

    private void Awake()
    {
        SetupHealth();
    }

    public void ReduceHp(int amount)
    {
        if (IsDead.Value == true)
            return;

        int newHpAmount = CurrentHP.Value - amount;

        if (newHpAmount > 0)
        {
            _healthEvents.OnNext(HealthEvent.HpReduced);
        }
        else
        {
            _healthEvents.OnNext(HealthEvent.Dead);
        }

        _currentHP.Value = Math.Clamp(newHpAmount, 0, MaxHP);
    }

    public void UpdateMaxHpFromLevel(int level)
    {
        _maxHP = CalculateMaxHp(level);
        _currentHP.Value = MaxHP;
    }

    private void SetupHealth()
    {
        _currentHP = new IntReactiveProperty(MaxHP);

        IsDead = _currentHP
            .Select(hp => hp <= 0)
            .ToReactiveProperty();
    }

    private int CalculateMaxHp(int level)
    {
        return level * LevelHPMultiplier + BaseHPAmount;
    }
}
