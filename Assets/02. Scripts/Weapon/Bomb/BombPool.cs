using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public List<Bomb> BombPrefabs; // 수류탄 프리팹

    public int PoolSize = 3; // 수류탄 풀의 크기

    public List<Bomb> _bombs; // 수류탄 풀 리스트

    public static BombPool Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("BombPool이 이미 존재합니다.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;

        int bombPrefabCount = BombPrefabs.Count;
        _bombs = new List<Bomb>(PoolSize * bombPrefabCount);

        foreach (Bomb bombPrefab in BombPrefabs)
        {
            for (int i = 0; i < PoolSize; i++)
            {
                Bomb bomb = Instantiate(bombPrefab);
                _bombs.Add(bomb);
                bomb.transform.SetParent(this.transform);
                bomb.gameObject.SetActive(false); // 비활성화
            }
        }
    }

    public Bomb GetBomb()
    {
        foreach (Bomb bomb in _bombs)
        {
            if (bomb == null)
            {
                continue;
            }
            if (bomb.gameObject.activeInHierarchy == false)
            {
                bomb.gameObject.SetActive(true);
                return bomb;
            }
        }
        return null;
    }

    public void ReturnBomb(Bomb bomb)
    {
        bomb.gameObject.SetActive(false); // 비활성화
        bomb.transform.SetParent(this.transform); // 풀로 반환
    }
}
