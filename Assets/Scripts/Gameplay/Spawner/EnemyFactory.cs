using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    [SerializeField]
    private Enemy _enemyPrefab;

    private Enemy _enemyInstanse;

    public void Restart()
    {
        if (_enemyInstanse != null)
        {
            Destroy(_enemyInstanse.gameObject);
            _enemyInstanse = null;
        }
    }

    public Enemy CreateEnemy(int level)
    {
        if (_enemyInstanse == null)
        {
            InstantiateEnemy();
        }

        _enemyInstanse.SetLevel(level);
        _enemyInstanse.Enable();

        return _enemyInstanse;
    }

    private void InstantiateEnemy()
    {
        _enemyInstanse = Enemy.Instantiate(_enemyPrefab, transform);
    }
}
