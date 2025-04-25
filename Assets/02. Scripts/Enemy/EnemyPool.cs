using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyPool : MonoBehaviour
{
    public List<Enemy> EnemyPrefabs; // 적 프리팹

    public int PoolSize = 10;

    public List<Enemy> _enemies; // 적 풀 리스트

    public static EnemyPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("EnemyPool이 이미 존재합니다.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        int enemyPrefabCount = EnemyPrefabs.Count;
        _enemies = new List<Enemy>(PoolSize * enemyPrefabCount);

        foreach (Enemy enemyPrefab in EnemyPrefabs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                Enemy enemy = Instantiate(enemyPrefab);
                _enemies.Add(enemy);
                enemy.transform.SetParent(this.transform);
                enemy.gameObject.SetActive(false); // 비활성화
            }
        }
    }

    public Enemy GetFromPool(EnemyType enemyType, Vector3 position)
    {
        foreach (Enemy enemy in _enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            if (enemy.Type == enemyType && enemy.gameObject.activeInHierarchy == false)
            {
                enemy.transform.position = position;
                enemy.Initialize();

                enemy.gameObject.SetActive(true);

                return enemy;
            }
        }
        return null;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        enemy.gameObject.SetActive(false); // 비활성화
        enemy.transform.SetParent(this.transform); // 풀로 반환
    }

    public int GetActiveEnemyCount()
    {
        int activeCount = 0;
        foreach (Enemy enemy in _enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                activeCount++;
            }
        }
        return activeCount;
    }
}
