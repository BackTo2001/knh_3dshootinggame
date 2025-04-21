using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    // 카메라 회전 스크립트
    // 목표 : 마우스를 조작하면 카메라를 그 방향으로 회전시키고 싶다.

    public float RotationSpeed = 120f; // 회전 속도

    private void Update()
    {
        // 1. 마우스 입력을 받는다.
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Debug.Log($"Mouse X: {mouseX}, Mouse Y: {mouseY}");

        // 2. 마우스 입력으로부터 회전시킬 방향을 만든다.
        // Todo : 마우스 좌표계와 화면 좌표계의 차이점을 알고, 잘 작동 하도록 아래 한줄의 코드를 잘 ~ 수정해보세요.
        Vector3 dir = new Vector3(-mouseY, mouseX, 0);

        // 3. 카메라를 그 방향으로 회전한다.
        // 새로운 위치 = 현재 위치 + 속도(속력 * 시간)
        // 새로운 각도 = 현재 각도 + 회전 속도 * 시간
        transform.eulerAngles = transform.eulerAngles + dir * RotationSpeed * Time.deltaTime;

        // 회전의 상하 제한이 필요하다! (-90도 ~ +90도)
        Vector3 rotation = transform.eulerAngles;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        transform.eulerAngles = rotation;
    }
}
