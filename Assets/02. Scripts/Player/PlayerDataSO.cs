using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]

public class PlayerDataSO : ScriptableObject
{
    // 목표 : 플레이어의 고정 스탯을 관리하는 스크립트
    [Header("체력")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;


    [Header("스태미나")]
    [SerializeField] private float maxStamina = 100f;
    public float MaxStamina => maxStamina;

    [Header("이동")]
    [SerializeField] private float moveSpeed = 7f;
    public float MoveSpeed => moveSpeed;

    [Header("대쉬")]
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float sprintStaminaCost = 2f;
    [SerializeField] private float sprintStaminaRecover = 1f;
    public float SprintSpeed => sprintSpeed;
    public float SprintStaminaCost => sprintStaminaCost;
    public float SprintStaminaRecover => sprintStaminaRecover;

    [Header("점프")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float jumpPower = 5f;
    public int MaxJumpCount => maxJumpCount;
    public float JumpPower => jumpPower;

    [Header("구르기")]
    [SerializeField] private float rollSpeed = 15f;
    [SerializeField] private float rollStaminaCost = 10f;
    [SerializeField] private float rollDuration = 0.5f;
    public float RollSpeed => rollSpeed;
    public float RollStaminaCost => rollStaminaCost;
    public float RollDuration => rollDuration;

    [Header("벽타기")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float climbStaminaCost = 5f;
    public float ClimbSpeed => climbSpeed;
    public float ClimbStaminaCost => climbStaminaCost;

    [Header("중력")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = 2f;
    public float Gravity => gravity;
    public float GroundedGravity => groundedGravity;
}
