using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    //[SerializeField] private PlayerDataSO _playerDataSO; // 플레이어 데이터 스크립터블 오브젝트
    //private PlayerStat _playerStat;
    //private CharacterController _characterController;   // 캐릭터 컨트롤러
    //private Vector3 _moveDirection = Vector3.zero;      // 이동 방향

    //private void Awake()
    //{
    //    _characterController = GetComponent<CharacterController>();
    //    _playerStat = new PlayerStat(_playerDataSO); // 플레이어 스탯 초기화
    //}

    //private void Update()
    //{

    //}

    // 목표 : wasd를 누르면 캐릭터를 카메라 방향에 맞게 이동시키고 싶다.
    // 필요 속성
    // 이동
    public float MoveSpeed = 7f;                        // 이동 속도
    private Vector3 _moveDirection = Vector3.zero;      // 이동 방향

    // 스태미나
    public float Stamina { get; private set; }
    public float MaxStamina = 100f;                     // 최대 스태미나

    // 점프 변수
    public int _currentJumpCount = 0;     // 현재 점프 횟수
    public int MaxJumpCount = 2;          // 최대 점프 횟수
    public float JumpPower = 1f;          // 점프 파워
    private bool _isJumping = false;      // 점프 여부

    // 중력 변수
    private const float GRAVITY = -9.8f;  // 중력
    private const float GroundedGravity = 2f; // 땅에 닿았을 때의 중력
    private float _yVelocity = 0f;        // 중력 가속도

    // 스프린트 변수
    public float SprintSpeed = 12f;       // 스프린트 속도
    public float SprintStaminaCost = 20f; // 스프린트 소모 스태미나
    public float SprintStaminaRecover = 10f; // 스프린트 회복 스태미나

    // 구르기 변수
    public float RollSpeed = 15f;         // 구르기 속도
    public float RollStaminaCost = 30f;   // 구르기 소모 스태미나
    public float RollDuration = 0.5f;     // 구르기 지속 시간
    private bool _isRolling = false;      // 구르기 여부
    private float _rollTimer = 0f;        // 구르기 타이머

    // 벽타기 변수
    public float ClimbSpeed = 5f;         // 벽타기 속도
    public float ClimbStaminaCost = 10f;   // 벽타기 소모 스태미나
    private bool _isClimbing = false;     // 벽타기 여부

    // 캐릭터 컨트롤러
    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // 스태미나 초기화
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

        _moveDirection = worldDir * MoveSpeed;
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
            _isJumping = false;
            _currentJumpCount = 0; // 점프 횟수 초기화
        }
        if (Input.GetKeyDown(KeyCode.Space) && _currentJumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
            _currentJumpCount++;
            _isJumping = true;
        }
    }

    // 4. 중력 함수
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

    // 5. 스프린트 함수
    public void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Stamina > 0)
            {
                // 스태미나가 0보다 클 때만 소모
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
            // 스태미나 회복
            Stamina += SprintStaminaRecover * Time.deltaTime;
            if (Stamina > 100f) Stamina = 100f;
        }
    }

    // 6. 구르기 함수
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
            // 구르기 방향 : 현재 이동 방향 (dir) 또는 카메라가 바라보는 방향
            Vector3 rollDir = Camera.main.transform.forward;
            _characterController.Move(rollDir * RollSpeed * Time.deltaTime);

            // 구르기 타이머 감소
            _rollTimer -= Time.deltaTime;
            if (_rollTimer <= 0f)
            {
                _isRolling = false; // 구르기 종료
            }
        }
    }

    // 7. 벽타기 함수
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
            _yVelocity = 0; // 중력 초기화

            // W를 누르고 있을 때만 위로 올라감
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 climbDir = Vector3.up;
                _characterController.Move(climbDir * ClimbSpeed * Time.deltaTime);

                // 스태미나 소모
                Stamina -= ClimbStaminaCost * Time.deltaTime;
            }

            // 벽에서 떨어지거나 스태미나 없으면 climbing 종료
            if ((_characterController.collisionFlags & CollisionFlags.Sides) == 0 || Stamina <= 0)
            {
                _isClimbing = false;
            }
        }
    }
}
