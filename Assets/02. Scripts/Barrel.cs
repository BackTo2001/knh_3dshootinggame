using UnityEngine;
public class Barrel : MonoBehaviour, IDamageable
{
    /*
     * 과제 2. 드럼통(Barrel) 구현
     * 총으로 쏘면 폭발한다. (폭발 이펙트, 체력이 있다.)
     * 폭발하면 주위 적&플레이어에게 데미지를 가한다.
     * ㄴ Physics.OverlapSphere 이용
     * 폭발하면 드럼통은 어딘가로 날라가며 n초 후 사라진다.
     */

    public GameObject ExplosionEffectPrefab;
    public float ExplosionRadius = 5f; // 폭발 반경
    public float ExplosionForce = 500f; // 폭발 힘
    public float DamageAmount = 50f; // 폭발 피해량
    private float DestoryDelay = 3f; // 폭발 후 삭제 시간
    public LayerMask LayerMask;

    public float Health = 20f;

    private bool _hasExploded = false; // 폭발 여부 플래그

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
        if (_hasExploded) return; // 이미 폭발한 경우 무시
        _hasExploded = true; // 폭발 상태로 설정

        // 폭발 이펙트 생성
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        // LayerMask 설정
        // 유니티는 레이어를 넘버링하는게 아니라 비트로 관리
        // 2진수 -> 0000 0000
        // 9번 레이어 선택 0000 0001 0000 0000
        // 9번 레이어 제외 선택 1111 1110 1111 1111
        // 비트 단위로 on/off를 관리할 수 있다.
        // int (32비트)
        // bool(8비트)


        // 폭발 범위 내 모든 객체 탐지
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask);

        foreach (Collider collider in colliders)
        {
            // 적 또는 플레이어게게 데미지 적용
            if (collider.TryGetComponent(out IDamageable damageable))
            {
                Damage damage = new Damage((int)DamageAmount, gameObject);
                damageable.TakeDamage(damage); // 적에게 피해를 입힘
            }

            // 다른 드럼통 연쇄 폭발
            //else if (collider.CompareTag("Barrel") && collider.gameObject != this.gameObject)
            //{
            //    Barrel otherBarrel = collider.GetComponent<Barrel>();
            //    if (otherBarrel != null)
            //    {
            //        Damage damage = new Damage((int)DamageAmount, gameObject);
            //        otherBarrel.TakeDamage(damage); // 다른 드럼통에 데미지 적용
            //    }
            //}

            // 폭발력 적용
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }
        }
        // 드럼통 삭제
        Destroy(gameObject, DestoryDelay);
    }
}
