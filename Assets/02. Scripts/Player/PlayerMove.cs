using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private PlayerDataSO _playerDataSO; // PlayerDataSO 참조
    private PlayerStat _playerStat; // PlayerStat 참조

    private Vector3 _moveDirection = Vector3.zero; // 이동 방향
    private float _yVelocity = 0f;        // 중력 가속도

    private CharacterController _characterController; // 캐릭터 컨트롤러

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


    // 1. 이동 함수
    public void Move()
    {
        // 1. 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. 입력으로부터 방향을 설정한다.
        Vector3 inputDir = new Vector3(h, 0, v).normalized;

        // 2-1. 메인 카메라를 기준으로 방향을 정한다.
        Vector3 worldDir = Camera.main.transform.TransformDirection(inputDir);
        worldDir.y = 0; // y축 방향은 무시

        _moveDirection = worldDir * _playerStat.MoveSpeed;
        _moveDirection.y = _yVelocity; // 중력 적용
    }

    // 2. 방향에 따라 플레이어를 이동한다.
    public void ApplyMovement()
    {
        // TransformDirection : 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(_moveDirection * Time.deltaTime);
    }

    // 3. 점프 함수
    public void Jump()
    {
        // 캐릭터가 땅 위에 있다면
        if (_characterController.isGrounded)
        //if(_characterController.collisionFlags == CollisionFlags.Below | CollisionFlags.Sides)
        {
            _playerStat.ResetJumpCount(); // 점프 카운트 초기화
        }
        if (Input.GetKeyDown(KeyCode.Space) && _playerStat.CanJump())
        {
            _yVelocity = _playerDataSO.JumpPower;
            _playerStat.IncreaseJumpCount(); // 점프 카운트 증가
        }
    }

    // 4. 중력 함수
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

    // 5. 스프린트 함수
    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_playerStat.CurrentStamina > 0)
            {
                // 스태미나가 0보다 클 때만 소모
                _playerStat.DecreaseStamina(_playerDataSO.SprintStaminaCost * Time.deltaTime);
                _playerStat.MoveSpeed = _playerDataSO.SprintSpeed; // 스프린트 속도
            }
        }
        else
        {
            _playerStat.RecoverStamina(_playerDataSO.SprintStaminaRecover * Time.deltaTime); // 스태미나 회복
            _playerStat.MoveSpeed = _playerDataSO.MoveSpeed; // 기본 속도
        }
    }

    // 6. 구르기 함수
    public void Roll()
    {
        if (Input.GetKeyDown(KeyCode.E) && _playerStat.CurrentStamina >= _playerDataSO.RollStaminaCost)
        {
            _playerStat.DecreaseStamina(_playerDataSO.RollStaminaCost);
            _playerStat.StartRoll(_playerDataSO.RollDuration);
        }
        if (_playerStat.IsRolling)
        {
            // 구르기 방향 : 현재 이동 방향 (dir) 또는 카메라가 바라보는 방향
            Vector3 rollDir = transform.forward; // 이동 방향
            rollDir.y = 0; // y축 방향은 무시

            // 구르기 이동
            _characterController.Move(rollDir * _playerDataSO.RollSpeed * Time.deltaTime);

            // 구르기 타이머 감소
            _playerStat.RollTimer -= Time.deltaTime;
            if (_playerStat.RollTimer <= 0f)
            {
                _playerStat.IsRolling = false; // 구르기 종료
            }
        }
    }

    // 7. 벽타기 함수
    public void Climb()
    {
        // 벽에 닿고 있고 W 누르고 있고 스태미나 있을 때 -> 등반 시작
        if ((_characterController.collisionFlags & CollisionFlags.Sides) != 0
            && Input.GetKey(KeyCode.W)
            && _playerStat.CurrentStamina > 0)
        {
            _playerStat.IsClimbing = true;
        }

        if (_playerStat.IsClimbing)
        {
            _yVelocity = 0; // 중력 초기화

            // W를 누르고 있을 때만 위로 올라감
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 climbDir = Vector3.up;
                _characterController.Move(climbDir * _playerDataSO.ClimbSpeed * Time.deltaTime);

                // 스태미나 소모
                _playerStat.DecreaseStamina(_playerDataSO.ClimbStaminaCost * Time.deltaTime);

                // 스태미나가 부족하면 벽 타기 종료
                if (_playerStat.CurrentStamina <= 0)
                {
                    _playerStat.IsClimbing = false;
                }
            }

            // 벽에서 떨어지거나 스태미나 없으면 climbing 종료
            if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0 || _playerStat.CurrentStamina <= 0)
            {
                _playerStat.IsClimbing = false;
            }
        }
    }
}
