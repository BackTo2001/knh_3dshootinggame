using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // ī�޶� ȸ�� ��ũ��Ʈ
    // ��ǥ : ���콺�� �����ϸ� ī�޶� �� �������� ȸ����Ű�� �ʹ�.

    public float RotationSpeed = 120f; // ȸ�� �ӵ�

    // ī�޶� ������ 0���������� �����Ѵٰ� ������ �����.
    private float _rotationX = 0;
    private float _rotationY = 0;


    private void Update()
    {
        // 1. ���콺 �Է��� �޴´�.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log($"Mouse X: {mouseX}, Mouse Y: {mouseY}");

        // 2. ȸ���� �縸ŭ �������� ������.
        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        // 3. ȸ�� �������� ȸ����Ų��.
        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }
}
