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
    }
}
