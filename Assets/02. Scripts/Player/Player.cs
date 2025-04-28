using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] private PlayerStat playerStat; // �÷��̾� ����
    [SerializeField] private PlayerDataSO playerData; // �÷��̾� ������
    [SerializeField] private BloodEffect bloodEffect; // ���� ȿ��

    private void Start()
    {
        playerStat.Initialize(playerData); // �÷��̾� ���� �ʱ�ȭ
    }
    public void TakeDamage(Damage damage)
    {
        playerStat.DecreaseHealth(damage.Value); // ü�� ����

        // UIManager�� ���� HealthSlider ������Ʈ
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
            Die(); // ��� ó��
        }
    }

    private void Die()
    {
        // ��� ó�� ����
        Debug.Log("Player is dead!");
    }
}