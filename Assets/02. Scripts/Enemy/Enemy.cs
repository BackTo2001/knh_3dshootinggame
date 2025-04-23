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
    public int Health = 100;                            // ü��   
    public float DamagedTime = 0.5f;                    // ���� �ð�
    public float DieTime = 1f;                    // ��� �ð�

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
        }

        Debug.Log($"������ȯ : {CurrentState} -> Damaged");

        CurrentState = EnemyState.Damaged; // ���� ����

        StartCoroutine(Damaged_Coroutine()); // �ڷ�ƾ ����
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
