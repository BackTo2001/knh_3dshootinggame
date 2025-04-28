using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 회전 속도

    private float _rotationX = 0;
    private float _rotationY = 0;

    [Header("# Camera Manager")]
    [SerializeField] private CameraManager _cameraManager; // CameraManager 참조

    private void Update()
    {
        if (_cameraManager == null)
        {
            return;
        }

        // 현재 카메라 모드에 따라 회전 방식을 다르게 처리
        switch (_cameraManager.GetCurrentCameraMode())
        {
            case CameraManager.CameraMode.FPS:
                HandleFPSRotation();
                break;

            case CameraManager.CameraMode.TPS:
                HandleTPSRotation();
                break;

            case CameraManager.CameraMode.QuarterView:
                HandleQuarterViewRotation();
                break;
        }
    }

    private void HandleFPSRotation()
    {
        // FPS 모드: 카메라가 플레이어의 시점에서 자유롭게 회전
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void HandleTPSRotation()
    {
        // TPS 모드: 카메라가 플레이어를 중심으로 회전
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -45f, 45f); // TPS 모드에서 제한된 각도

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void HandleQuarterViewRotation()
    {
        // 쿼터뷰 모드: 카메라가 고정된 각도에서 제한적으로 회전
        float mouseX = Input.GetAxis("Mouse X");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -60f, 60f); // 제한된 좌우 회전

        transform.eulerAngles = new Vector3(30f, _rotationX, 0); // 고정된 높이 각도
    }
}
