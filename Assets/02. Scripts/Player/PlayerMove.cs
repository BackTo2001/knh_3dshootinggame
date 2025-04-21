using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // ��ǥ : wasd�� ������ ĳ���͸� ī�޶� ���⿡ �°� �̵���Ű�� �ʹ�.
    // �ʿ� �Ӽ�
    // - �̵� �ӵ�
    public float MoveSpeed = 7f;
    public float JumpPower = 5f;

    private const float GRAVITY = -9.8f;    // �߷�
    private float _yVelocity = 0f;          // �߷� ���ӵ�

    private bool _isJumping = false;

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
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
        }

        // 3. ���� ����
        if (Input.GetButtonDown("Jump") && _isJumping == false)
        {
            _yVelocity = JumpPower;

            _isJumping = true;
        }

        // 4. �߷� ����
        _yVelocity += GRAVITY * Time.deltaTime;
        dir.y = _yVelocity;

        // 5. ���⿡ ���� �÷��̾ �̵��Ѵ�.
        // TransformDirection : ���� ������ ���͸� ���� ������ ���ͷ� �ٲ��ִ� �Լ�
        //transform.position += dir * MoveSpeed * Time.deltaTime;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }
}
