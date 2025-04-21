using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    private void Update()
    {
        // 보간(interpolation), smoothing 기법
        transform.position = Target.position;
    }
}
