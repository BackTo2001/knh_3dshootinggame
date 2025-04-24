using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    public Transform Target; // 추적할 타겟
    public float YOffset = 10f; // Y축 오프셋

    private void LateUpdate()
    {
        Vector3 newPosition = Target.position; // 타겟의 위치를 가져옴
        newPosition.y += YOffset; // Y축 오프셋을 추가
        transform.position = newPosition; // 카메라 위치 업데이트

        // 플레이어가 y축 회전한만큼 미니맵 카메라도 회전
        Vector3 newEulerAngles = Target.eulerAngles;
        newEulerAngles.x = 90;
        newEulerAngles.z = 0;
        transform.eulerAngles = newEulerAngles;
    }

}
