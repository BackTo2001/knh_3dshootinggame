using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �ʿ� �Ӽ�
    // - �߻� ��ġ
    public GameObject FirePosition;
    // - ��ź ������
    public GameObject BombPrefab;
    // - ������ ��
    public float ThrowPower = 15f;

    // ��ǥ : ���콺�� ���� ��ư�� ������ ī�޶� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    public ParticleSystem BulletEffect;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        // 2. ������ ��ư �Է� �ޱ�
        // - 0 : ����, 1: ������, 2: ��
        if (Input.GetMouseButtonDown(1))
        {
            // 3. �߻� ��ġ�� ����ź �����ϱ�
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.transform.position;

            // 4. ������ ����ź�� ī�޶� �������� �������� �� ���ϱ�       
            Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce(Camera.main.transform.forward * ThrowPower, ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);
        }

        // �Ѿ� �߻�(������ ���)
        // 1. ���� ��ư �Է� �ޱ�
        if (Input.GetMouseButtonDown(0))
        {
            // 2. ���̸� �����ϰ� �߻� ��ġ�� ���� ������ ����
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);

            // 3. ���̿� �ε��� ��ü�� ������ ������ ������ ����
            RaycastHit hitInfo = new RaycastHit();

            // 4. ���̸� �߻��� ����
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                // 4-1. ������ �����Ͱ� �ִٸ�(�ε����ٸ�) �ǰ� ����Ʈ ����(ǥ��)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal; // ���� ���� : ������ ���Ͽ� ������ ����
                BulletEffect.Play();

                // ���� ���� : ���������(��Į��, ����, ���), ������(�ﰢ�Լ�)
            }
        }
        // Ray : ������(������ġ, ����)
        // Raycast : �������� �߻�
        // RaycastHit : �������� ��ü�� �ε����ٸ� �� ������ �����ϴ� ����ü
    }
}
