using UnityEngine;

public class Barrel : MonoBehaviour
{
    /*
     * 과제 2. 드럼통(Barrel) 구현
     * 총으로 쏘면 폭발한다. (폭발 이펙트, 체력이 있다.)
     * 폭발하면 주위 적&플레이어에게 데미지를 가한다.
     * ㄴ Physics.OverlapSphere 이용
     * 폭발하면 드럼통은 어딘가로 날라가며 n초 후 사라진다.
     */

    public GameObject ExplosionEffectPrefab;

    private float Health = 100f;

    // TakeDamage
    public void TakeDamage(Damage damage)
    {
        Health -= damage.Value;

        if (Health <= 0)
        {
            Explode(); // 체력이 0 이하가 되면 폭발
        }
    }

    // Death
    // GameObject effectObject = Instantiate(ExplosionEffectPrefab);
    // effectObject.transform.position = transform.position;
    public void Explode()
    {
        // 폭발 이펙트 생성
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        // 드럼통 삭제
        Destroy(gameObject, 2f);
    }
}
