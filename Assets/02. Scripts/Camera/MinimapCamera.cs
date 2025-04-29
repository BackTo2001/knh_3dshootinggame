using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target; // ������ Ÿ��
    public float YOffset = 10f; // Y�� ������

    private void Update()
    {

        // altŰ�� ������ ���콺 visible
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
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
