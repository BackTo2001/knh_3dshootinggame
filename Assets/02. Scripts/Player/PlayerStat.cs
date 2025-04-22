using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // 목표 : 플레이어의 가변 스탯을 관리하는 스크립트
    public float MaxHealth;
    public float CurrentStamina;

    public Vector3 MoveDirection;

    public bool IsSprinting;

    public int CurrentJumpCount;
    public bool IsJumping;

    public bool IsRolling;
    public float RollTimer;

    public bool IsClimbing;



    public float YVelocity;
    public bool IsGrounded;
}
