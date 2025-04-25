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

        // ���� ���� �� ��� ��ü Ž��
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask);

        foreach (Collider collider in colliders)
        {
            // �� �Ǵ� �÷��̾�԰� ������ ����
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Damage damage = new Damage((int)DamageAmount, gameObject);
                    enemy.TakeDamage(damage);
                }
            }

            // �ٸ� �巳�� ���� ����
            else if (collider.CompareTag("Barrel") && collider.gameObject != this.gameObject)
            {
                Barrel otherBarrel = collider.GetComponent<Barrel>();
                if (otherBarrel != null)
                {
                    Damage damage = new Damage((int)DamageAmount, gameObject);
                    otherBarrel.TakeDamage(damage); // �ٸ� �巳�뿡 ������ ����
                }
            }

            // ���߷� ����
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                //rigidbody.AddForce(rigidbody.position - transform.position); // ���߷� ����
                //rigidbody.AddTorque(Vector3.one * 10f, ForceMode.Impulse); // ȸ���� ����
                //// ������ �������� �̵�
                //Vector3 randomDirection = new Vector3(
                //    Random.Range(-1f, 1f),
                //    Random.Range(0.5f, 1.5f), // �������� �� ���� Ƣ���� ����
                //    Random.Range(-1f, 1f)
                //).normalized;

                //float randomDistance = Random.Range(10f, 20f); // �̵� �Ÿ�
                //Vector3 targetPosition = transform.position + randomDirection * randomDistance;

                //// DOTween�� ����Ͽ� �ε巴�� �̵�
                //float duration = 0.5f; // �̵� �� ȸ�� ���� �ð�
                //transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
                //// ������ ������ ȸ��
                //Vector3 randomRotation = new Vector3(
                //    Random.Range(0f, 360f),
                //    Random.Range(0f, 360f),
                //    Random.Range(0f, 360f)
                //);
                //transform.DORotate(randomRotation, duration, RotateMode.WorldAxisAdd);
            }
        }
        // �巳�� ����
        Destroy(gameObject, DestoryDelay);
    }
}
