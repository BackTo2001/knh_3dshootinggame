using System.Collections;
using UnityEngine;

// 인공지능 : 사람처럼 똑똑하게 행동하는 알고리즘
// - 반응형 / 계획형 -> 규칙 기반 인공지능 (전통적인 방식)


public class Enemy : MonoBehaviour
{
    // 1. 상태를 열거형으로 정의
    public enum EnemyState
    {
        Idle = 0,
        Trace = 1,
        Return = 2,
        Attack = 3,
        Damaged = 4,
        Die = 5
    }

    // 2. 현재 상태를 지정
    public EnemyState CurrentState = EnemyState.Idle;

    private GameObject _player;                         // 플레이어 참조
    private CharacterController _characterController;   // 캐릭터 컨트롤러 참조

    private Vector3 _startPosition;                     // 원래 위치
    public float FindDistance = 5f;                     // 플레이어 발견 범위
    public float AttackDistance = 2.5f;                 // 플레이어 공격 범위
    public float MoveSpeed = 3.3f;                      // 이동 속도
    public float AttackCooltime = 2f;                   // 공격 쿨타임
    public float _attackTimer = 0f;                     // 공격 타이머
    public int Health = 100;                            // 체력   
    public float DamagedTime = 0.5f;                    // 경직 시간
    public float DieTime = 1f;                    // 사망 시간

    private void Start()
    {
        _startPosition = transform.position; // 원래 위치 저장
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
        }
    }

    public void TakeDamage(Damage damage)
    {
        if (CurrentState == EnemyState.Damaged || CurrentState == EnemyState.Die)
        {
            return;
        }

        Health -= damage.Value; // 체력 감소

        if (Health <= 0)
        {
            Debug.Log($"상태전환 : {CurrentState} -> Damaged");
            CurrentState = EnemyState.Die; // 상태 전이
            StartCoroutine(Die_Coroutine()); // 코루틴 시작
        }

        Debug.Log($"상태전환 : {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged; // 상태 전이

        StartCoroutine(Damaged_Coroutine()); // 코루틴 시작
    }


    // 3. 상태 함수 구현
    private void Idle()
    {
        // 행동 : 가만히 있는다.

        // 필요 속성
        // 1. 플레이어(위치)
        // 2. FindDistance(거리)

        if (Vector3.Distance(transform.position, _player.transform.position) < FindDistance)
        {
            Debug.Log("상태전환 : Idle -> Trace");
            CurrentState = EnemyState.Trace;
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
        Vector3 dir = (_player.transform.position - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);

    }

    private void Return()
    {
        // 전이 : 시작 위치와 가까워 지면 -> Idle
        if (Vector3.Distance(transform.position, _startPosition) <= _characterController.minMoveDistance)
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
        Vector3 dir = (_startPosition - transform.position).normalized;
        _characterController.Move(dir * MoveSpeed * Time.deltaTime);
    }

    private void Attack()
    {

        // 전이 : 공격 범위 보다 멀어지면 -> Trace
        if (Vector3.Distance(transform.position, _player.transform.position) >= AttackDistance)
        {
            Debug.Log("상태전환 : Attack -> Trace");
            CurrentState = EnemyState.Trace;
            _attackTimer = 0f;
            return;
        }

        // 행동 : 플레이어를 공격한다.
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= AttackCooltime)
        {
            Debug.Log("플레이어 공격!");
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
