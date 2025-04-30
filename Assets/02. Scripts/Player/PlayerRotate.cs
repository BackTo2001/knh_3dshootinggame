using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전 속도 똑같아야 한다.

    private float _rotationX = 0;
    private float _rotationY = 0;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        //_rotationX = Mathf.Clamp(_rotationX, -90f, 90f); // 회전 각도 제한
        _rotationY += mouseY * RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(-_rotationY, _rotationX, 0);
        //transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);
    }
}
