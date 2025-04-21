using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // 목표 : wasd를 누르면 캐릭터를 카메라 방향에 맞게 이동시키고 싶다.
    // 필요 속성
    // - 이동 속도
    public float MoveSpeed = 7f;
    // - 현재 점프 횟수
    public int _currentJumpCount = 0;
    // - 최대 점프 횟수
    public int MaxJumpCount = 2; // 최대 점프 횟수
    // - 점프 파워
    public float JumpPower = 1f;
    // - 중력
    private const float GRAVITY = -9.8f;    // 중력
    private float _yVelocity = 0f;          // 중력 가속도
    // - 스태미나
    public float Stamina { get; private set; }

    private bool _isJumping = false;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // 스태미나 초기화
        Stamina = 100f;
    }

    // 구현 순서
    // 1. 키보드 입력을 받는다.
    // 2. 입력으로부터 방향을 설정한다.
    // 3. 방향에 따라 플레이어를 이동한다.

    private void Update()
    {
        // 1. 키보드 입력을 받는다.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. 입력으로부터 방향을 설정한다.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 2-1. 메인 카메라를 기준으로 방향을 정한다.
        dir = Camera.main.transform.TransformDirection(dir);

        // 캐릭터가 땅 위에 있다면
        if (_characterController.isGrounded)
        //if(_characterController.collisionFlags == CollisionFlags.Below | CollisionFlags.Sides)
        {
            _isJumping = false;
            _currentJumpCount = 0; // 점프 횟수 초기화
        }

        // 3. 점프 적용
        if (Input.GetButtonDown("Jump") && _currentJumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
            _currentJumpCount++;
        }

        // 4. 중력 적용
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        // 5. Shift 가속
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = 12f;
            // 스태미나 소모
            if (Stamina > 0)
            {
                // 스태미나가 0보다 클 때만 소모
                Stamina -= 2f * Time.deltaTime;
            }
            else
            {
                // 스태미나가 0이 되면 더 이상 소모하지 않음
                MoveSpeed = 7f;
            }
        }
        else
        {
            MoveSpeed = 7f;
            // 스태미나 회복
            Stamina += 1f * Time.deltaTime;
            if (Stamina > 100f)
            {
                Stamina = 100f;
            }
        }

        // 6. 방향에 따라 플레이어를 이동한다.
        // TransformDirection : 지역 공간의 벡터를 월드 공간의 벡터로 바꿔주는 함수
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }
}
