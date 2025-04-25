using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class EnemyPool : MonoBehaviour
{
    public List<Enemy> EnemyPrefabs; // �� ������

    public int PoolSize = 10;

    public List<Enemy> _enemies; // �� Ǯ ����Ʈ

    public static EnemyPool Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("EnemyPool�� �̹� �����մϴ�.");
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
                enemy.gameObject.SetActive(false); // ��Ȱ��ȭ
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
        enemy.gameObject.SetActive(false); // ��Ȱ��ȭ
        enemy.transform.SetParent(this.transform); // Ǯ�� ��ȯ
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
