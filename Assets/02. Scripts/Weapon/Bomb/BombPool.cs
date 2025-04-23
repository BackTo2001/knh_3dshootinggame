using System.Collections.Generic;
using UnityEngine;

public class BombPool : MonoBehaviour
{
    public List<Bomb> BombPrefabs; // ����ź ������

    public int PoolSize = 3; // ����ź Ǯ�� ũ��

    public List<Bomb> _bombs; // ����ź Ǯ ����Ʈ

    public static BombPool Instance;
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("BombPool�� �̹� �����մϴ�.");
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
                bomb.gameObject.SetActive(false); // ��Ȱ��ȭ
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
        bomb.gameObject.SetActive(false); // ��Ȱ��ȭ
        bomb.transform.SetParent(this.transform); // Ǯ�� ��ȯ
    }
}
