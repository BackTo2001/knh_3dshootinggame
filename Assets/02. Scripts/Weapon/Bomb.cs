using UnityEngine;

public class Bomb : MonoBehaviour
{
    // 목표 : 마우스의 오른쪽 버튼을 누르면 카메라가 바라보는 방향으로 수류탄을 던지고 싶다.
    // 1. 수류탄 오브젝트 생성

    public GameObject ExplosionEffectPrefab;

    // 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        Destroy(gameObject); // 수류탄 파괴
    }
}
