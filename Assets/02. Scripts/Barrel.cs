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

        // 폭발 범위 내 모든 객체 탐지
        Collider[] colliders = Physics.OverlapSphere(transform.position, ExplosionRadius, LayerMask);

        foreach (Collider collider in colliders)
        {
            // 적 또는 플레이어게게 데미지 적용
            if (collider.CompareTag("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                    Damage damage = new Damage((int)DamageAmount, gameObject);
                    enemy.TakeDamage(damage);
                }
            }

            // 다른 드럼통 연쇄 폭발
            else if (collider.CompareTag("Barrel") && collider.gameObject != this.gameObject)
            {
                Barrel otherBarrel = collider.GetComponent<Barrel>();
                if (otherBarrel != null)
                {
                    Damage damage = new Damage((int)DamageAmount, gameObject);
                    otherBarrel.TakeDamage(damage); // 다른 드럼통에 데미지 적용
                }
            }

            // 폭발력 적용
            Rigidbody rigidbody = collider.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
                //rigidbody.AddForce(rigidbody.position - transform.position); // 폭발력 적용
                //rigidbody.AddTorque(Vector3.one * 10f, ForceMode.Impulse); // 회전력 적용
                //// 무작위 방향으로 이동
                //Vector3 randomDirection = new Vector3(
                //    Random.Range(-1f, 1f),
                //    Random.Range(0.5f, 1.5f), // 위쪽으로 더 많이 튀도록 설정
                //    Random.Range(-1f, 1f)
                //).normalized;

                //float randomDistance = Random.Range(10f, 20f); // 이동 거리
                //Vector3 targetPosition = transform.position + randomDirection * randomDistance;

                //// DOTween을 사용하여 부드럽게 이동
                //float duration = 0.5f; // 이동 및 회전 지속 시간
                //transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
                //// 무작위 축으로 회전
                //Vector3 randomRotation = new Vector3(
                //    Random.Range(0f, 360f),
                //    Random.Range(0f, 360f),
                //    Random.Range(0f, 360f)
                //);
                //transform.DORotate(randomRotation, duration, RotateMode.WorldAxisAdd);
            }
        }
        // 드럼통 삭제
        Destroy(gameObject, DestoryDelay);
    }
}
