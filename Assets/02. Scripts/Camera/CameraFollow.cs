using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // ����(interpolation), smoothing ���
        transform.position = Target.position;
    }
}
