using UniRx;
using UnityEngine;

public class PlayerClicker : MonoBehaviour
{
    private const int ParticlesCount = 30;

    [SerializeField]
    private ParticleSystem _hitParticles;

    [SerializeField]
    private int _damagePerClick;

    private Camera _mainCamera;

    public void Enable()
    {
        if (enabled == false)
            enabled = true;
    }

    public void Disable()
    {
        if (enabled == true)
            enabled = false;
    }

    private void Awake()
    {
        _mainCamera = Camera.main;

        HitOnTouch();
    }

    private void HitOnTouch()
    {
        Observable.EveryUpdate()
            .Where(_ => Input.GetMouseButtonDown(0) && enabled)
            .Select(_ => Input.mousePosition)
            .Subscribe(touchPosition => HandleTouch(touchPosition))
            .AddTo(this);
    }

    private void HandleTouch(Vector3 touchPosition)
    {
        Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(touchPosition);
        EmitParticles(worldPosition);

        RaycastHit2D target = Physics2D.Raycast(new Vector2(worldPosition.x, worldPosition.y), Vector2.zero);
        HitIfCollide(target, worldPosition);
    }

    private void HitIfCollide(RaycastHit2D target, Vector3 worldPosition)
    {
        if (target.collider == null)
            return;

        if (target.collider.TryGetComponent(out Enemy enemy))
        {   
            enemy.Hit(_damagePerClick);
        }
    }

    private void EmitParticles(Vector3 worldPosition)
    {
        _hitParticles.transform.position = worldPosition;
        _hitParticles.Play();
    }
}
