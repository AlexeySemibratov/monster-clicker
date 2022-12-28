using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    private const string Hit = "Hit";
    private const string Hide = "Hide";
    private const string Reset = "Reset";

    [SerializeField]
    private Animator _animator;

    public void Restart()
    {
        _animator.SetTrigger(Reset);
    }

    public void PlayHit()
    {
        _animator.SetTrigger(Hit);
    }

    public void PlayDead()
    {
        _animator.SetTrigger(Hide);
    }
}
