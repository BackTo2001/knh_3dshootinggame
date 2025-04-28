using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat playerStat; // 플레이어 스탯
    [SerializeField] private PlayerDataSO playerData; // 플레이어 데이터
    [SerializeField] private BloodEffect bloodEffect; // 혈흔 효과

    private void Start()
    {
        playerStat.Initialize(playerData); // 플레이어 스탯 초기화
    }
    public void TakeDamage(Damage damage)
    {
        playerStat.DecreaseHealth(damage.Value); // 체력 감소

        // UIManager를 통해 HealthSlider 업데이트
        if (UIManager.Instance != null)
        {
            UIManager.Instance.RefreshHealthSlider(playerStat.CurrentHealth, playerData.MaxHealth);
        }

        if (bloodEffect != null)
        {
            bloodEffect.ShowBloodEffect();
        }

        if (playerStat.IsDead)
        {
            Die(); // 사망 처리
        }
    }

    private void Die()
    {
        // 사망 처리 로직
        Debug.Log("Player is dead!");
    }
}