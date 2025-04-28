using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// �ΰ����� : ���ó�� �ȶ��ϰ� �ൿ�ϴ� �˰���
// - ������ / ��ȹ�� -> ��Ģ ��� �ΰ����� (�������� ���)


public class Enemy : MonoBehaviour, IDamageable
{
    // �� Ÿ��
    public enum EnemyType
    {
        Default = 0,
        Follow = 1
    }

    // 1. ���¸� ���������� ����
    public enum EnemyState
    {
        Idle,
        Trace,
        Patrol,
        Follow,
        Return,
        Attack,
        Damaged,
        Die
    }

    // 2. ���� ���¸� ����
    public EnemyType Type = EnemyType.Default;
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                         // �÷��̾� ����
    private CharacterController _characterController;   // ĳ���� ��Ʈ�ѷ� ����
    private NavMeshAgent _agent;                 // �׺�޽� ������Ʈ ����
    private Vector3 _startPosition;                     // ���� ��ġ

    //private Animator _animator;

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

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private HealthBar healthBar; // ü�¹� ����

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed; // �̵� �ӵ� ����

        _startPosition = transform.position; // ���� ��ġ ����
        _characterController = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");

        currentHealth = maxHealth;

        // �ʱ� ü�� ����
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
    }
    public void Initialize()
    {
        // ���� �ʱ� ���� ����
        Health = 100; // ü�� �ʱ�ȭ
        CurrentState = EnemyState.Idle; // �⺻ ���·� ����
        _idleTimer = 0f; // Ÿ�̸� �ʱ�ȭ
        _currentPatrolIndex = 0; // ���� �ε��� �ʱ�ȭ

        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        if (_agent.isOnNavMesh)
            _agent.ResetPath();

        gameObject.SetActive(true); // Ȱ��ȭ
    }

    private void Update()
    {
        if (Type == EnemyType.Follow)
        {
            if (CurrentState != EnemyState.Follow)
            {
                CurrentState = EnemyState.Follow;
            }
            Follow();
            return;
        }
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

            case EnemyState.Follow:
                {
                    Follow();
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
        if (CurrentState == EnemyState.Die)
        {
            return;
        }

        // ü�� ����
        currentHealth -= damage.Value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // ü�¹� ������Ʈ
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"������ȯ : {CurrentState} -> Die");
            CurrentState = EnemyState.Die; // ���� ����
                                           //_animator.SetTrigger("Die");
            StartCoroutine(Die_Coroutine()); // �ڷ�ƾ ����
            return;
        }

        Debug.Log($"������ȯ : {CurrentState} -> Damaged");

        // �˹� ����
        ApplyKnockback(damage);

        //_animator.SetTrigger("Hit");
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
            //_characterController.Move(_knockbackPower * Time.deltaTime);
            _agent.Move(_knockbackPower * Time.deltaTime); // NavMeshAgent�� ����Ͽ� �˹� ����
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
            //_animator.SetTrigger("IdleToTrace");
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
        //Vector3 dir = (targetPoint.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(targetPoint.position); // NavMeshAgent�� ����Ͽ� ���� �������� �̵�

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
        //Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position); // NavMeshAgent�� ����Ͽ� �÷��̾ ����
    }

    private void Follow()
    {
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        // ���� : ���� ��ġ�� ����� ���� -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= 0.1f)
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
        //Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition); // NavMeshAgent�� ����Ͽ� ���� ��ġ�� ���ư�
    }

    private void Attack()
    {

        // ���� : ���� ���� ���� �־����� -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("������ȯ : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            //_animator.SetTrigger("AttackDelayToMove");
            return;
        }

        // �ൿ : �÷��̾ �����Ѵ�.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            // _animator.SetTrigger("AttackDelayToAttack");
            Debug.Log("�÷��̾� ����!");
            Damage damage = new Damage(10, gameObject); // ������ �� 10, ������ ���� ����
            _player.GetComponent<IDamageable>()?.TakeDamage(damage); // �÷��̾ ������ ����

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
        _agent.isStopped = true; // �̵� ����
        _agent.ResetPath();

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
