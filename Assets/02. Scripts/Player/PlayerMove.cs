using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _playerDataSO; // PlayerDataSO ����
    private PlayerStat _playerStat; // PlayerStat ����

    private Vector3 _moveDirection = Vector3.zero; // �̵� ����
    private float _yVelocity = 0f;        // �߷� ���ӵ�

    private CharacterController _characterController; // ĳ���� ��Ʈ�ѷ�

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerStat = GetComponent<PlayerStat>();

        if (_playerStat != null && _playerDataSO != null)
        {
            _playerStat.Initialize(_playerDataSO);
        }
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

        _moveDirection = worldDir * _playerStat.MoveSpeed;
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
            _playerStat.ResetJumpCount(); // ���� ī��Ʈ �ʱ�ȭ
        }
        if (Input.GetKeyDown(KeyCode.Space) && _playerStat.CanJump())
        {
            _yVelocity = _playerDataSO.JumpPower;
            _playerStat.IncreaseJumpCount(); // ���� ī��Ʈ ����
        }
    }

    // 4. �߷� �Լ�
    public void Gravity()
    {
        if (_characterController.isGrounded && _yVelocity < 0)
        {
            _yVelocity = -_playerDataSO.GroundedGravity;
        }
        else
        {
            _yVelocity += _playerDataSO.Gravity * Time.deltaTime;
        }
    }

    // 5. ������Ʈ �Լ�
    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_playerStat.CurrentStamina > 0)
            {
                // ���¹̳��� 0���� Ŭ ���� �Ҹ�
                _playerStat.DecreaseStamina(_playerDataSO.SprintStaminaCost * Time.deltaTime);
                _playerStat.MoveSpeed = _playerDataSO.SprintSpeed; // ������Ʈ �ӵ�
            }
        }
        else
        {
            _playerStat.RecoverStamina(_playerDataSO.SprintStaminaRecover * Time.deltaTime); // ���¹̳� ȸ��
            _playerStat.MoveSpeed = _playerDataSO.MoveSpeed; // �⺻ �ӵ�
        }
    }

    // 6. ������ �Լ�
    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerStat.CurrentStamina >= _playerDataSO.RollStaminaCost)
        {
            _playerStat.DecreaseStamina(_playerDataSO.RollStaminaCost);
            _playerStat.StartRoll(_playerDataSO.RollDuration);
        }
        if (_playerStat.IsRolling)
        {
            // ������ ���� : ���� �̵� ���� (dir) �Ǵ� ī�޶� �ٶ󺸴� ����
            Vector3 rollDir = transform.forward; // �̵� ����
            rollDir.y = 0; // y�� ������ ����

            // ������ �̵�
            _characterController.Move(rollDir * _playerDataSO.RollSpeed * Time.deltaTime);

            // ������ Ÿ�̸� ����
            _playerStat.RollTimer -= Time.deltaTime;
            if (_playerStat.RollTimer <= 0f)
            {
                _playerStat.IsRolling = false; // ������ ����
            }
        }
    }

    // 7. ��Ÿ�� �Լ�
    public void Climb()
    {
        // ���� ��� �ְ� W ������ �ְ� ���¹̳� ���� �� -> ��� ����
        if ((_characterController.collisionFlags & CollisionFlags.Sides) != 0
            && Input.GetKey(KeyCode.W)
            && _playerStat.CurrentStamina > 0)
        {
            _playerStat.IsClimbing = true;
        }

        if (_playerStat.IsClimbing)
        {
            _yVelocity = 0; // �߷� �ʱ�ȭ

            // W�� ������ ���� ���� ���� �ö�
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 climbDir = Vector3.up;
                _characterController.Move(climbDir * _playerDataSO.ClimbSpeed * Time.deltaTime);

                // ���¹̳� �Ҹ�
                _playerStat.DecreaseStamina(_playerDataSO.ClimbStaminaCost * Time.deltaTime);

                // ���¹̳��� �����ϸ� �� Ÿ�� ����
                if (_playerStat.CurrentStamina <= 0)
                {
                    _playerStat.IsClimbing = false;
                }
            }

            // ������ �������ų� ���¹̳� ������ climbing ����
            if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0 || _playerStat.CurrentStamina <= 0)
            {
                _playerStat.IsClimbing = false;
            }
        }
    }
}
