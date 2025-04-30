using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // ī�޶�� ȸ�� �ӵ� �Ȱ��ƾ� �Ѵ�.

    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        //_rotationX = Mathf.Clamp(_rotationX, -90f, 90f); // ȸ�� ���� ����
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        //transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }
}
