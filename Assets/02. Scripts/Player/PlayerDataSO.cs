using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]

public class PlayerDataSO : ScriptableObject
{
    // ��ǥ : �÷��̾��� ���� ������ �����ϴ� ��ũ��Ʈ
    [Header("ü��")]
    [SerializeField] private float maxHealth = 100f;
    public float MaxHealth => maxHealth;


    [Header("���¹̳�")]
    [SerializeField] private float maxStamina = 100f;
    public float MaxStamina => maxStamina;

    [Header("�̵�")]
    [SerializeField] private float moveSpeed = 7f;
    public float MoveSpeed => moveSpeed;

    [Header("�뽬")]
    [SerializeField] private float sprintSpeed = 12f;
    [SerializeField] private float sprintStaminaCost = 2f;
    [SerializeField] private float sprintStaminaRecover = 1f;
    public float SprintSpeed => sprintSpeed;
    public float SprintStaminaCost => sprintStaminaCost;
    public float SprintStaminaRecover => sprintStaminaRecover;

    [Header("����")]
    [SerializeField] private int maxJumpCount = 2;
    [SerializeField] private float jumpPower = 5f;
    public int MaxJumpCount => maxJumpCount;
    public float JumpPower => jumpPower;

    [Header("������")]
    [SerializeField] private float rollSpeed = 15f;
    [SerializeField] private float rollStaminaCost = 10f;
    [SerializeField] private float rollDuration = 0.5f;
    public float RollSpeed => rollSpeed;
    public float RollStaminaCost => rollStaminaCost;
    public float RollDuration => rollDuration;

    [Header("��Ÿ��")]
    [SerializeField] private float climbSpeed = 5f;
    [SerializeField] private float climbStaminaCost = 5f;
    public float ClimbSpeed => climbSpeed;
    public float ClimbStaminaCost => climbStaminaCost;

    [Header("�߷�")]
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float groundedGravity = 2f;
    public float Gravity => gravity;
    public float GroundedGravity => groundedGravity;
}
