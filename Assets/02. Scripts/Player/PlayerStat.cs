using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 참조 데이터 ( 고정 수치)
    private PlayerDataSO _data;

    // 상태 값
    public float MoveSpeed { get; set; }
    public float CurrentHealth { get; private set; }
    public float CurrentStamina { get; private set; }
    public float YVelocity { get; set; }
    public Vector3 MoveDirection { get; set; }


    public int CurrentJumpCount { get; private set; }
    public bool IsJumping { get; set; }
    public bool IsSprinting { get; set; }
    public bool IsRolling { get; set; }
    public float RollTimer { get; set; }
    public bool IsClimbing { get; set; }
    public bool IsGrounded { get; set; }

    // 스탯 초기화
    public void Initialize(PlayerDataSO data)
    {
        _data = data;
        MoveSpeed = _data.MoveSpeed;
        CurrentHealth = _data.MaxHealth;
        CurrentStamina = _data.MaxStamina;
        CurrentJumpCount = 0;
        YVelocity = 0f;
        MoveDirection = Vector3.zero;
        IsJumping = false;
        IsSprinting = false;
        IsRolling = false;
        RollTimer = 0f;
        IsClimbing = false;
        IsGrounded = false;
    }

    // 체력 조작
    public void DecreaseHealth(float amount)
    {
        CurrentHealth = Mathf.Max(CurrentHealth - amount, 0f);
    }

    public void RecoverHealth(float amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, _data.MaxHealth);
    }

    public bool IsDead => CurrentHealth <= 0f;

    // 스태미나 조작
    public void DecreaseStamina(float amount)
    {
        CurrentStamina = Mathf.Max(CurrentStamina - amount, 0f);
    }

    public void RecoverStamina(float amount)
    {
        CurrentStamina = Mathf.Min(CurrentStamina + amount, _data.MaxStamina);
    }

    // 점프 조작
    public void IncreaseJumpCount() => CurrentJumpCount++;
    public void ResetJumpCount() => CurrentJumpCount = 0;
    public bool CanJump() => CurrentJumpCount < _data.MaxJumpCount;

    public void SetGrounded(bool value)
    {
        IsGrounded = value;
        if (value)
        {
            ResetJumpCount();
        }
    }

    // 구르기 조작
    public void StartRoll(float duration)
    {
        IsRolling = true;
        RollTimer = duration;
    }
}
