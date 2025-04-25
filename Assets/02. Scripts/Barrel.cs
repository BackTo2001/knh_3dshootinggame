using UnityEngine;
public class Barrel : MonoBehaviour, IDamageable
{
    /*
     * ���� 2. �巳��(Barrel) ����
     * ������ ��� �����Ѵ�. (���� ����Ʈ, ü���� �ִ�.)
     * �����ϸ� ���� ��&�÷��̾�� �������� ���Ѵ�.
     * �� Physics.OverlapSphere �̿�
     * �����ϸ� �巳���� ��򰡷� ���󰡸� n�� �� �������.
     */

    public GameObject ExplosionEffectPrefab;
    public float ExplosionRadius = 5f; // ���� �ݰ�
    public float ExplosionForce = 500f; // ���� ��
    public float DamageAmount = 50f; // ���� ���ط�
    private float DestoryDelay = 3f; // ���� �� ���� �ð�
    public LayerMask LayerMask;

    public float Health = 20f;

    private bool _hasExploded = false; // ���� ���� �÷���

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
        if (_hasExploded) return; // �̹� ������ ��� ����
        _hasExploded = true; // ���� ���·� ����

        // ���� ����Ʈ ����
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        // LayerMask ����
        // ����Ƽ�� ���̾ �ѹ����ϴ°� �ƴ϶� ��Ʈ�� ����
        // 2���� -> 0000 0000
        // 9�� ���̾� ���� 0000 0001 0000 0000
        // 9�� ���̾� ���� ���� 1111 1110 1111 1111
        // ��Ʈ ������ on/off�� ������ �� �ִ�.
        // int (32��Ʈ)
        // bool(8��Ʈ)


        // ���� ���� �� ��� ��ü Ž��
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask);

        foreach (Collider collider in colliders)
        {
            // �� �Ǵ� �÷��̾�԰� ������ ����
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage((int)DamageAmount, gameObject);
                damageable.TakeDamage(damage); // ������ ���ظ� ����
            }

            // �ٸ� �巳�� ���� ����
            //else if (collider.CompareTag("Barrel") && collider.gameObject != this.gameObject)
            //{
            //    Barrel otherBarrel = collider.GetComponent<Barrel>();
            //    if (otherBarrel != null)
            //    {
            //        Damage damage = new Damage((int)DamageAmount, gameObject);
            //        otherBarrel.TakeDamage(damage); // �ٸ� �巳�뿡 ������ ����
            //    }
            //}

            // ���߷� ����
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
        }
        // �巳�� ����
        Destroy(gameObject, DestoryDelay);
    }
}
