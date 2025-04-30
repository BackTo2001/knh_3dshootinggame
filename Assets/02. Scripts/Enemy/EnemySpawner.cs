using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyPool EnemyPool; // Enemy Pool 참조
    public Enemy.EnemyType EnemyType;
    public Vector3 SpawnCenter;   // 스폰 중심
    public float SpawnRadius = 5f;   // 스폰 반경
    public float SpawnInterval = 3f; // 스폰 주기
    public Transform[] PatrolPoints; // 순찰 지점 배열

    public void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {

            // 현재 활성화된 적의 수 확인
            int activeEnemyCount = EnemyPool.GetActiveEnemyCount(EnemyType);

            if (activeEnemyCount < EnemyPool.PoolSize)
            {
                // 랜덤 위치 계산
                Vector3 randomPosition = GetRandomPosition();

                // EnenmyPool에서 적 가져오기
                Enemy enemy = EnemyPool.GetFromPool(EnemyType, randomPosition);
                if (enemy != null)
                {
                    enemy.Initialize(); // 적 초기화
                    enemy.PatrolPoints = PatrolPoints; // 순찰 지점 설정
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
