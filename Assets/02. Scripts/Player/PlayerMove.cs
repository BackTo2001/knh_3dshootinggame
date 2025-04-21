using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ : wasd�� ������ ĳ���͸� ī�޶� ���⿡ �°� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�
    // - �̵� �ӵ�
    public float MoveSpeed = 7f;

    // ���� ����
    // 1. Ű���� �Է��� �޴´�.
    // 2. �Է����κ��� ������ �����Ѵ�.
    // 3. ���⿡ ���� �÷��̾ �̵��Ѵ�.

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // ���� ī�޶� �������� ������ ���Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);


        transform.position += dir * MoveSpeed * Time.deltaTime;
    }
}
