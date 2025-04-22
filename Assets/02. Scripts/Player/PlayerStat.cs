using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    // ��ǥ : �÷��̾��� ���� ������ �����ϴ� ��ũ��Ʈ
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
