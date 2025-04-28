using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // ȸ�� �ӵ�

    private float _rotationX = 0;
    private float _rotationY = 0;

    [Header("# Camera Manager")]
    [SerializeField] private CameraManager _cameraManager; // CameraManager ����

    private void Update()
    {
        if (_cameraManager == null)
        {
            return;
        }

        // ���� ī�޶� ��忡 ���� ȸ�� ����� �ٸ��� ó��
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
        // FPS ���: ī�޶� �÷��̾��� �������� �����Ӱ� ȸ��
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -90f, 90f);

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void HandleTPSRotation()
    {
        // TPS ���: ī�޶� �÷��̾ �߽����� ȸ��
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationY += -mouseY * RotationSpeed * Time.deltaTime;
        _rotationY = Mathf.Clamp(_rotationY, -45f, 45f); // TPS ��忡�� ���ѵ� ����

        transform.eulerAngles = new Vector3(_rotationY, _rotationX, 0);
    }

    private void HandleQuarterViewRotation()
    {
        // ���ͺ� ���: ī�޶� ������ �������� ���������� ȸ��
        float mouseX = Input.GetAxis("Mouse X");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;
        _rotationX = Mathf.Clamp(_rotationX, -60f, 60f); // ���ѵ� �¿� ȸ��

        transform.eulerAngles = new Vector3(30f, _rotationX, 0); // ������ ���� ����
    }
}
