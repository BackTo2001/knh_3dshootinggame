using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ : wasd�� ������ ĳ���͸� ī�޶� ���⿡ �°� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�
    // - �̵� �ӵ�
    public float MoveSpeed = 7f;
    // - ���� ���� Ƚ��
    public int _currentJumpCount = 0;
    // - �ִ� ���� Ƚ��
    public int MaxJumpCount = 2; // �ִ� ���� Ƚ��
    // - ���� �Ŀ�
    public float JumpPower = 1f;
    // - �߷�
    private const float GRAVITY = -9.8f;    // �߷�
    private float _yVelocity = 0f;          // �߷� ���ӵ�
    // - ���¹̳�
    public float Stamina { get; private set; }

    private bool _isJumping = false;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        // ���¹̳� �ʱ�ȭ
        Stamina = 100f;
    }

    // ���� ����
    // 1. Ű���� �Է��� �޴´�.
    // 2. �Է����κ��� ������ �����Ѵ�.
    // 3. ���⿡ ���� �÷��̾ �̵��Ѵ�.

    private void Update()
    {
        // 1. Ű���� �Է��� �޴´�.
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // 2. �Է����κ��� ������ �����Ѵ�.
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        // 2-1. ���� ī�޶� �������� ������ ���Ѵ�.
        dir = Camera.main.transform.TransformDirection(dir);

        // ĳ���Ͱ� �� ���� �ִٸ�
        if (_characterController.isGrounded)
        //if(_characterController.collisionFlags == CollisionFlags.Below | CollisionFlags.Sides)
        {
            _isJumping = false;
            _currentJumpCount = 0; // ���� Ƚ�� �ʱ�ȭ
        }

        // 3. ���� ����
        if (Input.GetButtonDown("Jump") && _currentJumpCount < MaxJumpCount)
        {
            _yVelocity = JumpPower;
            _currentJumpCount++;
        }

        // 4. �߷� ����
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        // 5. Shift ����
        if (Input.GetKey(KeyCode.LeftShift))
        {
            MoveSpeed = 12f;
            // ���¹̳� �Ҹ�
            if (Stamina > 0)
            {
                // ���¹̳��� 0���� Ŭ ���� �Ҹ�
                Stamina -= 2f * Time.deltaTime;
            }
            else
            {
                // ���¹̳��� 0�� �Ǹ� �� �̻� �Ҹ����� ����
                MoveSpeed = 7f;
            }
        }
        else
        {
            MoveSpeed = 7f;
            // ���¹̳� ȸ��
            Stamina += 1f * Time.deltaTime;
            if (Stamina > 100f)
            {
                Stamina = 100f;
            }
        }

        // 6. ���⿡ ���� �÷��̾ �̵��Ѵ�.
        // TransformDirection : ���� ������ ���͸� ���� ������ ���ͷ� �ٲ��ִ� �Լ�
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }
}
