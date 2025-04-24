using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target; // ������ Ÿ��
    public float YOffset = 10f; // Y�� ������

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position; // Ÿ���� ��ġ�� ������
        newPosition.y += YOffset; // Y�� �������� �߰�
        transform.position = newPosition; // ī�޶� ��ġ ������Ʈ

        // �÷��̾ y�� ȸ���Ѹ�ŭ �̴ϸ� ī�޶� ȸ��
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }

}
