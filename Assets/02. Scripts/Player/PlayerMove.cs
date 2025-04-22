using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //[SerializeField] private PlayerDataSO _playerDataSO; // �÷��̾� ������ ��ũ���ͺ� ������Ʈ
    //private PlayerStat _playerStat;
    //private CharacterController _characterController;   // ĳ���� ��Ʈ�ѷ�
    //private Vector3 _moveDirection = Vector3.zero;      // �̵� ����

    //private void Awake()
    //{
    //    _characterController = GetComponent<CharacterController>();
    //    _playerStat = new PlayerStat(_playerDataSO); // �÷��̾� ���� �ʱ�ȭ
    //}

    //private void Update()
    //{

    //}

    // ��ǥ : wasd�� ������ ĳ���͸� ī�޶� ���⿡ �°� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�
    // �̵�
    public float MoveSpeed = 7f;                        // �̵� �ӵ�
    private Vector3 _moveDirection = Vector3.zero;      // �̵� ����

    // ���¹̳�
    public float Stamina { get; private set; }
    public float MaxStamina = 100f;                     // �ִ� ���¹̳�

    // ���� ����
    public int _currentJumpCount = 0;     // ���� ���� Ƚ��
    public int MaxJumpCount = 2;          // �ִ� ���� Ƚ��
    public float JumpPower = 1f;          // ���� �Ŀ�
    private bool _isJumping = false;      // ���� ����

    // �߷� ����
    private const float GRAVITY = -9.8f;  // �߷�
    private const float GroundedGravity = 2f; // ���� ����� ���� �߷�
    private float _yVelocity = 0f;        // �߷� ���ӵ�

    // ������Ʈ ����
    public float SprintSpeed = 12f;       // ������Ʈ �ӵ�
    public float SprintStaminaCost = 20f; // ������Ʈ �Ҹ� ���¹̳�
    public float SprintStaminaRecover = 10f; // ������Ʈ ȸ�� ���¹̳�

    // ������ ����
    public float RollSpeed = 15f;         // ������ �ӵ�
    public float RollStaminaCost = 30f;   // ������ �Ҹ� ���¹̳�
    public float RollDuration = 0.5f;     // ������ ���� �ð�
    private bool _isRolling = false;      // ������ ����
    private float _rollTimer = 0f;        // ������ Ÿ�̸�

    // ��Ÿ�� ����
    public float ClimbSpeed = 5f;         // ��Ÿ�� �ӵ�
    public float ClimbStaminaCost = 10f;   // ��Ÿ�� �Ҹ� ���¹̳�
    private bool _isClimbing = false;     // ��Ÿ�� ����

    // ĳ���� ��Ʈ�ѷ�
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // ���¹̳� �ʱ�ȭ
        Stamina = MaxStamina;
    }

    private void Update()
    {
        Gravity();
        Sprint();
        Roll();
        Climb();
        Jump();
        Move();
        ApplyMovement();
    }


    // 1. �̵� �Լ�
    public void Move()
    {
        // 1. Ű���� �Է��� �޴´�.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. �Է����κ��� ������ �����Ѵ�.
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // 2-1. ���� ī�޶� �������� ������ ���Ѵ�.
        Vector3 worldDir = Camera.main.transform.TransformDirection(inputDir);
        worldDir.y = 0; // y�� ������ ����

        _moveDirection = worldDir * MoveSpeed;
        _moveDirection.y = _yVelocity; // �߷� ����
    }

    // 2. ���⿡ ���� �÷��̾ �̵��Ѵ�.
    public void ApplyMovement()
    {
        // TransformDirection : ���� ������ ���͸� ���� ������ ���ͷ� �ٲ��ִ� �Լ�
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    // 3. ���� �Լ�
    public void Jump()
    {
        // ĳ���Ͱ� �� ���� �ִٸ�
        if (_characterController.isGrounded)
        //if(_characterController.collisionFlags == CollisionFlags.Below | CollisionFlags.Sides)
        {
            _isJumping = false;
            _currentJumpCount = 0; // ���� Ƚ�� �ʱ�ȭ
        }
        if (Input.GetKeyDown(KeyCode.Space) && _currentJumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
            _currentJumpCount++;
            _isJumping = true;
        }
    }

    // 4. �߷� �Լ�
    public void Gravity()
    {
        if (_characterController.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -GroundedGravity;
        }
        else
        {
            _yVelocity += GRAVITY * Time.deltaTime;
        }
    }

    // 5. ������Ʈ �Լ�
    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Stamina > 0)
            {
                // ���¹̳��� 0���� Ŭ ���� �Ҹ�
                MoveSpeed = SprintSpeed;
                Stamina -= SprintStaminaCost * Time.deltaTime;
            }
            else
            {
                MoveSpeed = 7f;
            }
        }
        else
        {
            MoveSpeed = 7f;
            // ���¹̳� ȸ��
            Stamina += SprintStaminaRecover * Time.deltaTime;
            if (Stamina > 100f) Stamina = 100f;
        }
    }

    // 6. ������ �Լ�
    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.E) && !_isRolling && Stamina >= RollStaminaCost)
        {
            _isRolling = true;
            _rollTimer = RollDuration;
            Stamina -= RollStaminaCost;
        }
        if (_isRolling)
        {
            // ������ ���� : ���� �̵� ���� (dir) �Ǵ� ī�޶� �ٶ󺸴� ����
            Vector3 rollDir = Camera.main.transform.forward;
            _characterController.Move(rollDir * RollSpeed * Time.deltaTime);

            // ������ Ÿ�̸� ����
            _rollTimer -= Time.deltaTime;
            if (_rollTimer <= 0f)
            {
                _isRolling = false; // ������ ����
            }
        }
    }

    // 7. ��Ÿ�� �Լ�
    public void Climb()
    {
        if ((_characterController.collisionFlags & CollisionFlags.Sides) != 0
            && Input.GetKey(KeyCode.W)
            && Stamina > 0)
        {
            _isClimbing = true;
        }

        if (_isClimbing)
        {
            _yVelocity = 0; // �߷� �ʱ�ȭ

            // W�� ������ ���� ���� ���� �ö�
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 climbDir = Vector3.up;
                _characterController.Move(climbDir * ClimbSpeed * Time.deltaTime);

                // ���¹̳� �Ҹ�
                Stamina -= ClimbStaminaCost * Time.deltaTime;
            }

            // ������ �������ų� ���¹̳� ������ climbing ����
            if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0 || Stamina <= 0)
            {
                _isClimbing = false;
            }
        }
    }
}
