using UnityEngine;

// �ΰ����� : ���ó�� �ȶ��ϰ� �ൿ�ϴ� �˰���
// - ������ / ��ȹ�� -> ��Ģ ��� �ΰ����� (�������� ���)


public class Enemy : MonoBehaviour
{
    // 1. ���¸� ���������� ����
    public enum EnemyState
    {
        Idle = 0,
        Trace = 1,
        Return = 2,
        Attack = 3,
        Damaged = 4,
        Die = 5
    }

    // 2. ���� ���¸� ����
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                         // �÷��̾� ����
    private CharacterController _characterController;   // ĳ���� ��Ʈ�ѷ� ����

    private Vector3 _startPosition;                     // ���� ��ġ
    public float FindDistance = 5f;                     // �÷��̾� �߰� ����
    public float AttackDistance = 2.5f;                 // �÷��̾� ���� ����
    public float MoveSpeed = 3.3f;                      // �̵� �ӵ�
    public float AttackCooltime = 2f;                   // ���� ��Ÿ��
    public float _attackTimer = 0f;                     // ���� Ÿ�̸�

    private void Start()
    {
        _startPosition = transform.position; // ���� ��ġ ����
        _characterController = GetComponent<CharacterController>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Idle:
                {
                    Idle();
                    break;
                }

            case EnemyState.Trace:
                {
                    Trace();
                    break;
                }

            case EnemyState.Return:
                {
                    Return();
                    break;
                }

            case EnemyState.Attack:
                {
                    Attack();
                    break;
                }

            case EnemyState.Damaged:
                {
                    Damaged();
                    break;
                }

            case EnemyState.Die:
                {
                    Die();
                    break;
                }
        }
    }

    // 3. ���� �Լ� ����
    private void Idle()
    {
        // �ൿ : ������ �ִ´�.

        // �ʿ� �Ӽ�
        // 1. �÷��̾�(��ġ)
        // 2. FindDistance(�Ÿ�)

        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ : Idle -> Trace");
            CurrentState = EnemyState.Trace;
        }
    }

    private void Trace()
    {
        // ���� : �÷��̾�� �־����� -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) >= FindDistance)
        {
            Debug.Log("������ȯ : Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }
        // ���� : ���� ���� ��ŭ ����� ���� -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("������ȯ : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // �ൿ : �÷��̾ �����Ѵ�.
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }

    private void Return()
    {
        // ���� : ���� ��ġ�� ����� ���� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
        {
            Debug.Log("������ȯ : Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // ���� : �÷��̾�� ����� ���� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("������ȯ : Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // �ൿ : ���� ��ġ�� ���ư���.
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {

        // ���� : ���� ���� ���� �־����� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("������ȯ : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        // �ൿ : �÷��̾ �����Ѵ�.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("�÷��̾� ����!");
            _attackTimer = 0f;
        }
    }

    private void Damaged()
    {
        // �ൿ : �ǰ�
    }

    private void Die()
    {
        // �ൿ : ���
    }
}
