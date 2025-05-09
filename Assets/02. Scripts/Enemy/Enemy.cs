using System.Collections;
using UnityEngine;
using UnityEngine.AI;

// 인공지능 : 사람처럼 똑똑하게 행동하는 알고리즘
// - 반응형 / 계획형 -> 규칙 기반 인공지능 (전통적인 방식)


public class Enemy : MonoBehaviour, IDamageable
{
    // 적 타입
    public enum EnemyType
    {
        Default = 0,
        Follow = 1
    }

    // 1. 상태를 열거형으로 정의
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

    // 2. 현재 상태를 지정
    public EnemyType Type = EnemyType.Default;
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                         // 플레이어 참조
    private CharacterController _characterController;   // 캐릭터 컨트롤러 참조
    private NavMeshAgent _agent;                 // 네비메시 에이전트 참조
    private Vector3 _startPosition;                     // 원래 위치

    //private Animator _animator;

    public float FindDistance = 5f;                     // 플레이어 발견 범위
    public float AttackDistance = 2.5f;                 // 플레이어 공격 범위
    public float MoveSpeed = 3.3f;                      // 이동 속도
    public float AttackCooltime = 2f;                   // 공격 쿨타임
    public float _attackTimer = 0f;                     // 공격 타이머
    public int Health = 100;                            // 체력   
    public float DamagedTime = 0.5f;                    // 경직 시간
    public float DieTime = 1f;                          // 사망 시간
    public Transform[] PatrolPoints;                    // 순찰 지점 배열
    private int _currentPatrolIndex = 0;                // 현재 순찰 지점 인덱스
    public float IdleToPatrolTime = 5f;                 // Idle 상태에서 Patrol 상태로 전이되는 시간
    private float _idleTimer = 0f;                      // Idle 상태에서 대기하는 타이머
    private float knockbackDuration = 0.5f;             // 넉백 지속 시간
    private float elapsedTime = 0f;                     // 경과 시간

    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;

    [SerializeField] private HealthBar healthBar; // 체력바 참조

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.speed = MoveSpeed; // 이동 속도 설정

        _startPosition = transform.position; // 원래 위치 저장
        _characterController = GetComponent<CharacterController>();
        //_animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Initialize()
    {
        // 적의 초기 상태 설정
        Health = 100; // 체력 초기화
        currentHealth = maxHealth;
        // 초기 체력 설정
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }
        CurrentState = EnemyState.Idle; // 기본 상태로 설정
        _idleTimer = 0f; // 타이머 초기화
        _currentPatrolIndex = 0; // 순찰 인덱스 초기화

        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        if (_agent.isOnNavMesh)
            _agent.ResetPath();

        gameObject.SetActive(true); // 활성화
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

        // 체력 감소
        currentHealth -= damage.Value;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // 체력바 업데이트
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log($"상태전환 : {CurrentState} -> Die");
            CurrentState = EnemyState.Die; // 상태 전이
                                           //_animator.SetTrigger("Die");
            StartCoroutine(Die_Coroutine()); // 코루틴 시작
            return;
        }

        Debug.Log($"상태전환 : {CurrentState} -> Damaged");

        // 넉백 적용
        ApplyKnockback(damage);

        //_animator.SetTrigger("Hit");
        CurrentState = EnemyState.Damaged; // 상태 전이

        StartCoroutine(Damaged_Coroutine()); // 코루틴 시작
    }

    private void ApplyKnockback(Damage damage)
    {
        // 넉백 방향 계산 (공격 방향의 반대)
        Vector3 knockbackDir = (transform.position - damage.From.transform.position).normalized;

        // 넉백 파워 적용
        Vector3 knockbackPower = knockbackDir * damage.KnockBackPower;

        // Debug: 넉백 방향과 크기 확인
        Debug.Log($"넉백 방향: {knockbackDir}, 넉백 파워: {knockbackPower}");

        // 넉백 적용
        StartCoroutine(ApplyKnockBackCoroutine(knockbackPower));
    }

    private IEnumerator ApplyKnockBackCoroutine(Vector3 _knockbackPower)
    {
        elapsedTime = 0f; // 경과 시간 초기화
        while (elapsedTime < knockbackDuration)
        {
            // 캐릭터 컨트롤러를 사용하여 넉백 적용
            //_characterController.Move(_knockbackPower * Time.deltaTime);
            _agent.Move(_knockbackPower * Time.deltaTime); // NavMeshAgent를 사용하여 넉백 적용
            elapsedTime += Time.deltaTime; // 경과 시간 증가
            yield return null; // 다음 프레임까지 대기
        }
    }


    // 3. 상태 함수 구현
    private void Idle()
    {

        // 행동 : 가만히 있는다.
        _idleTimer += Time.deltaTime; // 타이머 증가

        // 전이 : Idle 상태가 지속되면 -> Patrol
        if (_idleTimer >= IdleToPatrolTime)
        {
            Debug.Log("상태전환 : Idle -> Patrol");
            _idleTimer = 0f; // 타이머 초기화
            CurrentState = EnemyState.Patrol;
            return;
        }
        // 전이 : 플레이어와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            _idleTimer = 0f; // 타이머 초기화
            CurrentState = EnemyState.Trace;
            //_animator.SetTrigger("IdleToTrace");
        }
    }

    private void Patrol()
    {
        // 전이 : 순찰 중 플레이어와 가까워지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Patrol -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // 행동 : 순찰 지점으로 이동한다.
        Transform targetPoint = PatrolPoints[_currentPatrolIndex];
        //Vector3 dir = (targetPoint.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(targetPoint.position); // NavMeshAgent를 사용하여 순찰 지점으로 이동

        // 행동2 : 순찰 지점에 도착하면 -> 다음 지점으로 이동
        float distanceToTarget = Vector3.Distance(transform.position, targetPoint.position);
        if (distanceToTarget <= 0.1f) // 도달 조건 수정
        {
            _currentPatrolIndex = (_currentPatrolIndex + 1) % PatrolPoints.Length; // 다음 지점으로 이동
        }
    }

    private void Trace()
    {
        // 전이 : 플레이어와 멀어지면 -> Return
        if (Vector3.Distance(transform.position, _player.transform.position) >= FindDistance)
        {
            Debug.Log("상태전환 : Trace -> Return");
            CurrentState = EnemyState.Return;
            return;
        }
        // 전이 : 공격 범위 만큼 가까워 지면 -> Attack
        if (Vector3.Distance(transform.position, _player.transform.position) < AttackDistance)
        {
            Debug.Log("상태전환 : Trace -> Attack");
            CurrentState = EnemyState.Attack;
            return;
        }

        // 행동 : 플레이어를 추적한다.
        //Vector3 dir = (_player.transform.position - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_player.transform.position); // NavMeshAgent를 사용하여 플레이어를 추적
    }

    private void Follow()
    {
        _agent.SetDestination(_player.transform.position);
    }

    private void Return()
    {
        // 전이 : 시작 위치와 가까워 지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= 0.1f)
        {
            Debug.Log("상태전환 : Return -> Idle");
            transform.position = _startPosition;
            CurrentState = EnemyState.Idle;
            return;
        }

        // 전이 : 플레이어와 가까워 지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) <= FindDistance)
        {
            Debug.Log("상태전환 : Return -> Trace");
            CurrentState = EnemyState.Trace;
            return;
        }

        // 행동 : 원래 위치로 돌아간다.
        //Vector3 dir = (_startPosition - transform.position).normalized;
        //_characterController.Move(dir * MoveSpeed * Time.deltaTime);
        _agent.SetDestination(_startPosition); // NavMeshAgent를 사용하여 원래 위치로 돌아감
    }

    private void Attack()
    {

        // 전이 : 공격 범위 보다 멀어지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            //_animator.SetTrigger("AttackDelayToMove");
            return;
        }

        // 행동 : 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            // _animator.SetTrigger("AttackDelayToAttack");
            Debug.Log("플레이어 공격!");
            Damage damage = new Damage(10, gameObject); // 데미지 값 10, 공격자 정보 전달
            _player.GetComponent<IDamageable>()?.TakeDamage(damage); // 플레이어에 데미지 전달

            _attackTimer = 0f;
        }
    }

    private IEnumerator Damaged_Coroutine()
    {
        // 행동 : 일정 시간 동안 경직
        //_damagedTimer += Time.deltaTime;

        //if (_damagedTimer >= DamagedTime)
        //{
        //    _damagedTimer = 0f;
        //    Debug.Log($"상태전환 : Damaged -> Trace");
        //    CurrentState = EnemyState.Trace;
        //}

        // 코루틴 방식으로 변경
        _agent.isStopped = true; // 이동 정지
        _agent.ResetPath();

        yield return new WaitForSeconds(DamagedTime);

        Debug.Log($"상태전환 : Damaged -> Trace");
        CurrentState = EnemyState.Trace;
    }

    private IEnumerator Die_Coroutine()
    {
        // 행동 : 사망
        yield return new WaitForSeconds(DieTime);
        gameObject.SetActive(false); // 비활성화
    }
}
