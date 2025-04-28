using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyPool : MonoBehaviour
{
    public List<Enemy> EnemyPrefabs; // 적 프리팹

    public int PoolSize = 10;

    private Dictionary<EnemyType, List<Enemy>> _enemyPools; // 타입별 적 풀

    public static EnemyPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log("EnemyPool이 이미 존재합니다.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        _enemyPools = new Dictionary<EnemyType, List<Enemy>>();

        foreach (Enemy enemyPrefab in EnemyPrefabs)
        {
            if (_enemyPools.ContainsKey(enemyPrefab.Type) == false)
            {
                _enemyPools[enemyPrefab.Type] = new List<Enemy>();
            }

            for (int i = 0; i < PoolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                enemy.gameObject.SetActive(false); // 비활성화
                enemy.transform.SetParent(this.transform);
                _enemyPools[enemyPrefab.Type].Add(enemy);
            }
        }
    }

    public Enemy GetFromPool(EnemyType enemyType, Vector3 position)
    {
        if (_enemyPools.ContainsKey(enemyType))
        {
            foreach (Enemy enemy in _enemyPools[enemyType])
            {
                if (enemy.gameObject.activeInHierarchy == false)
                {
                    enemy.transform.position = position;
                    enemy.Initialize();
                    enemy.gameObject.SetActive(true);
                    return enemy;
                }
            }
        }
        return null;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        if (_enemyPools.ContainsKey(enemy.Type))
        {
            enemy.gameObject.SetActive(false); // 비활성화
            enemy.transform.SetParent(this.transform); // 풀로 반환
        }
    }

    public int GetActiveEnemyCount(EnemyType enemyType)
    {
        if (_enemyPools.ContainsKey(enemyType))
        {
            int activeCount = 0;
            foreach (Enemy enemy in _enemyPools[enemyType])
            {
                if (enemy.gameObject.activeInHierarchy)
                {
                    activeCount++;
                }
            }
            return activeCount;
        }
        return 0;
    }
}
