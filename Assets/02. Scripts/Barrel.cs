using UnityEngine;

public class Barrel : MonoBehaviour
{
    /*
     * ���� 2. �巳��(Barrel) ����
     * ������ ��� �����Ѵ�. (���� ����Ʈ, ü���� �ִ�.)
     * �����ϸ� ���� ��&�÷��̾�� �������� ���Ѵ�.
     * �� Physics.OverlapSphere �̿�
     * �����ϸ� �巳���� ��򰡷� ���󰡸� n�� �� �������.
     */

    public GameObject ExplosionEffectPrefab;

    private float Health = 100f;

    // TakeDamage
    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0)
        {
            Explode(); // ü���� 0 ���ϰ� �Ǹ� ����
        }
    }

    // Death
    // GameObject effectObject = Instantiate(ExplosionEffectPrefab);
    // effectObject.transform.position = transform.position;
    public void Explode()
    {
        // ���� ����Ʈ ����
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        // �巳�� ����
        Destroy(gameObject, 2f);
    }
}
