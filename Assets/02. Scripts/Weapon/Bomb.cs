using UnityEngine;

public class Bomb : MonoBehaviour
{
    // ��ǥ : ���콺�� ������ ��ư�� ������ ī�޶� �ٶ󺸴� �������� ����ź�� ������ �ʹ�.
    // 1. ����ź ������Ʈ ����

    public GameObject ExplosionEffectPrefab;

    // �浹���� ��
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        Destroy(gameObject); // ����ź �ı�
    }
}
