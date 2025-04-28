using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public enum CameraMode
    {
        FPS,
        TPS,
        QuarterView
    }

    [SerializeField] private Transform _fps;
    [SerializeField] private Transform _tps;
    [SerializeField] private Transform _quarterView;

    [Header("# Target")]
    [SerializeField] private Transform _targetTransform;

    private CameraMode _cameramode = CameraMode.FPS;

    private void Update()
    {
        HandleCamera();
        UpdateCameraPosition();
    }

    private void HandleCamera()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            _cameramode = CameraMode.FPS;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            _cameramode = CameraMode.TPS;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            _cameramode = CameraMode.QuarterView;
        }
    }

    private void UpdateCameraPosition()
    {
        switch (_cameramode)
        {
            case CameraMode.FPS:
                transform.position = _fps.position;
                transform.rotation = _fps.rotation;
                break;

            case CameraMode.TPS:
                transform.position = _tps.position;
                transform.LookAt(_targetTransform);
                break;

            case CameraMode.QuarterView:
                transform.position = _quarterView.position;
                transform.LookAt(_targetTransform);
                break;
        }
    }
    public CameraMode GetCurrentCameraMode()
    {
        return _cameramode;
    }
}
