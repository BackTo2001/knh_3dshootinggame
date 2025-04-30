using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool EnemyPool; // Enemy Pool ����
    public Enemy.EnemyType EnemyType;
    public Vector3 SpawnCenter;   // ���� �߽�
    public float SpawnRadius = 5f;   // ���� �ݰ�
    public float SpawnInterval = 3f; // ���� �ֱ�
    public Transform[] PatrolPoints; // ���� ���� �迭

    public void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {

            // ���� Ȱ��ȭ�� ���� �� Ȯ��
            int activeEnemyCount = EnemyPool.GetActiveEnemyCount(EnemyType);

            if (activeEnemyCount < EnemyPool.PoolSize)
            {
                // ���� ��ġ ���
                Vector3 randomPosition = GetRandomPosition();

                // EnenmyPool���� �� ��������
                Enemy enemy = EnemyPool.GetFromPool(EnemyType, randomPosition);
                if (enemy != null)
                {
                    enemy.Initialize(); // �� �ʱ�ȭ
                    enemy.PatrolPoints = PatrolPoints; // ���� ���� ����
                    enemy.gameObject.SetActive(true);
                }
            }

            yield return new WaitForSeconds(SpawnInterval);
        }
    }

    private Vector3 GetRandomPosition()
    {
        float x = Random.Range(-SpawnRadius, SpawnRadius);
        float z = Random.Range(-SpawnRadius, SpawnRadius);
        return SpawnCenter + new Vector3(x, 0, z);
    }
}
