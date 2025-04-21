using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // ī�޶� ȸ�� ��ũ��Ʈ
    // ��ǥ : ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.

    public float RotationSpeed = 120f; // ȸ�� �ӵ�

    private void Update()
    {
        // 1. ���콺 �Է��� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log($"Mouse X: {mouseX}, Mouse Y: {mouseY}");

        // 2. ���콺 �Է����κ��� ȸ����ų ������ �����.
        // Todo : ���콺 ��ǥ��� ȭ�� ��ǥ���� �������� �˰�, �� �۵� �ϵ��� �Ʒ� ������ �ڵ带 �� ~ �����غ�����.
        Vector3 dir = new Vector3(-mouseY, mouseX, 0);

        // 3. ī�޶� �� �������� ȸ���Ѵ�.
        // ���ο� ��ġ = ���� ��ġ + �ӵ�(�ӷ� * �ð�)
        // ���ο� ���� = ���� ���� + ȸ�� �ӵ� * �ð�
        transform.eulerAngles = transform.eulerAngles + dir * RotationSpeed * Time.deltaTime;

        // ȸ���� ���� ������ �ʿ��ϴ�! (-90�� ~ +90��)
        Vector3 rotation = transform.eulerAngles;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        transform.eulerAngles = rotation;
    }
}
