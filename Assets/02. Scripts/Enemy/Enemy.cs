using System.Collections;
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
        Patrol = 2,
        Return = 3,
        Attack = 4,
        Damaged = 5,
        Die = 6
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
    public int Health = 100;                            // ü��   
    public float DamagedTime = 0.5f;                    // ���� �ð�
    public float DieTime = 1f;                          // ��� �ð�
    public Transform[] PatrolPoints;                    // ���� ���� �迭
    private int _currentPatrolIndex = 0;                // ���� ���� ���� �ε���
    public float IdleToPatrolTime = 5f;                 // Idle ���¿��� Patrol ���·� ���̵Ǵ� �ð�
    private float _idleTimer = 0f;                      // Idle ���¿��� ����ϴ� Ÿ�̸�
    private float knockbackDuration = 0.5f;             // �˹� ���� �ð�
    private float elapsedTime = 0f;                     // ��� �ð�

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

            case EnemyState.Patrol:
                {
                    Patrol();
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
        }
    }

    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value; // ü�� ����

        if (Health <= 0)
        {
            Debug.Log($"������ȯ : {CurrentState} -> Damaged");
            CurrentState = EnemyState.Die; // ���� ����
            StartCoroutine(Die_Coroutine()); // �ڷ�ƾ ����
            return;
        }

        Debug.Log($"������ȯ : {CurrentState} -> Damaged");

        // �˹� ����
        ApplyKnockback(damage);

        CurrentState = EnemyState.Damaged; // ���� ����

        StartCoroutine(Damaged_Coroutine()); // �ڷ�ƾ ����
    }

    private void ApplyKnockback(Damage damage)
    {
        // �˹� ���� ��� (���� ������ �ݴ�)
        Vector3 knockbackDir = (transform.position - damage.From.transform.position).normalized;

        // �˹� �Ŀ� ����
        Vector3 knockbackPower = knockbackDir * damage.KnockBackPower;

        // Debug: �˹� ����� ũ�� Ȯ��
        Debug.Log($"�˹� ����: {knockbackDir}, �˹� �Ŀ�: {knockbackPower}");

        // �˹� ����
        StartCoroutine(ApplyKnockBackCoroutine(knockbackPower));
    }

    private IEnumerator ApplyKnockBackCoroutine(Vector3 _knockbackPower)
    {
        elapsedTime = 0f; // ��� �ð� �ʱ�ȭ
        while (elapsedTime < knockbackDuration)
        {
            // ĳ���� ��Ʈ�ѷ��� ����Ͽ� �˹� ����
            _characterController.Move(_knockbackPower * Time.deltaTime);
            elapsedTime += Time.deltaTime; // ��� �ð� ����
            yield return null; // ���� �����ӱ��� ���
        }
    }


    // 3. ���� �Լ� ����
    private void Idle()
    {
        // �ൿ : ������ �ִ´�.
        _idleTimer += Time.deltaTime; // Ÿ�̸� ����

        // ���� : Idle ���°� ���ӵǸ� -> Patrol
        if (_idleTimer >= IdleToPatrolTime)
        {
            Debug.Log("������ȯ : Idle -> Patrol");
            _idleTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            CurrentState = EnemyState.Patrol;
            return;
        }
        // ���� : �÷��̾�� ����� ���� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ : Idle -> Trace");
            _idleTimer = 0f; // Ÿ�̸� �ʱ�ȭ
            CurrentState = EnemyState.Trace;
        }
    }

    private void Patrol()
    {
        // ���� : ���� �� �÷��̾�� ��������� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("������ȯ : Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // �ൿ : ���� �������� �̵��Ѵ�.
        Transform targetPoint = PatrolPoints[_currentPatrolIndex];
        Vector3 dir = (targetPoint.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

        // �ൿ2 : ���� ������ �����ϸ� -> ���� �������� �̵�
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);
        if (distanceToTarget <= 0.1f) // ���� ���� ����
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % PatrolPoints.Length; // ���� �������� �̵�
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

    private IEnumerator Damaged_Coroutine()
    {
        // �ൿ : ���� �ð� ���� ����
        //_damagedTimer += Time.deltaTime;

        //if (_damagedTimer >= DamagedTime)
        //{
        //    _damagedTimer = 0f;
        //    Debug.Log($"������ȯ : Damaged -> Trace");
        //    CurrentState = EnemyState.Trace;
        //}

        // �ڷ�ƾ ������� ����
        yield return new WaitForSeconds(DamagedTime);
        Debug.Log($"������ȯ : Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        // �ൿ : ���
        yield return new WaitForSeconds(DieTime);
        gameObject.SetActive(false); // ��Ȱ��ȭ
    }
}
