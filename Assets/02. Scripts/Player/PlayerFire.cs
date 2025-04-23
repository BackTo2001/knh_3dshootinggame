using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // 필요 속성

    // - 발사 위치
    public GameObject FirePosition;

    // - 폭탄 프리팹
    public GameObject BombPrefab;
    // - 폭탄 던지는 힘
    public float MinThrowPower = 5f;
    public float MaxThrowPower = 20f;
    private float _holdTime = 0f;
    public float MaxHoldTime = 2f;
    // - 폭탄 최대 개수
    private int _maxBombCount = 3;
    private int _currentBombCount = 3;

    public int MaxBombCount => _maxBombCount;
    public int CurrentBombCount => _currentBombCount;


    // - 총알 최대 개수
    private int _maxBulletCount = 50;
    private int _currentBulletCount = 50;

    public int MaxBulletCount => _maxBulletCount;
    public int CurrentBulletCount => _currentBombCount;

    // - 총알 쿨타임
    public float FireInterval = 0.5f;
    private float _fireTimer = 0f;

    // - 총알 재장전 타임
    public float ReloadTime = 2f;
    private float _reloadTimer = 0f;
    private bool _isReloading = false;

    // 목표 : 마우스의 왼쪽 버튼을 누르면 카메라가 바라보는 방향으로 총을 발사하고 싶다.
    public ParticleSystem BulletEffect;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //BombPool.Instance.SetPoolSize(MaxBombCount);

        //_fireTimer = FireInterval;

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void Update()
    {
        // 폭탄 사용
        if (Input.GetMouseButton(1))
        {
            _holdTime += Time.deltaTime; // 누르는 시간 
            _holdTime = Mathf.Clamp(_holdTime, 0f, MaxHoldTime); // 최대 시간 제한

            // 던지는 힘을 0~1로 정규화하여 UI 업데이트
            float normalizedPower = _holdTime / MaxHoldTime;
            UIManager.Instance.UpdateBombThrowPower(normalizedPower);
            UIManager.Instance.ShowBombThrowSlider(true); // Slider 표시

        }
        if (Input.GetMouseButtonUp(1))
        {
            ThrowBomb();
            _holdTime = 0f; // 폭탄 던지기 후 초기화
            UIManager.Instance.ShowBombThrowSlider(false); // Slider 숨기기
        }

        // 총알 발사(레이저 방식)
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
        // Ray : 레이저(시작위치, 방향)
        // Raycast : 레이저를 발사
        // RaycastHit : 레이저가 물체와 부딪혔다면 그 정보를 저장하는 구조체
    }

    public void ThrowBomb()
    {
        if (_currentBombCount > 0)
        {
            _currentBombCount--;

            // 던지는 힘 계산
            float throwPower = Mathf.Lerp(MinThrowPower, MaxThrowPower, _holdTime / MaxHoldTime);

            // 폭탄 생성 및 발사
            GameObject bomb = Instantiate(BombPrefab);
            bomb.transform.position = FirePosition.transform.position;

            Rigidbody bombRigidbody = bomb.GetComponent<Rigidbody>();
            bombRigidbody.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
            bombRigidbody.AddTorque(Vector3.one);
        }
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }

    public void RecoverBomb()
    {
        if (_currentBombCount < _maxBombCount)
        {
            _currentBombCount++;
        }
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }

    public void FireBullet()
    {
        if (_currentBulletCount > 0)
        {
            _currentBulletCount--;
            // 총알 발사 로직
            // 1. 레이를 생성하고 발사 위치와 진행 방향을 설정
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            // 2. 레이와 부딪힌 물체의 정보를 저장할 변수를 생성
            RaycastHit hitInfo = new RaycastHit();
            // 3. 레이를 발사한 다음
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                // 3-1. 변수에 데이터가 있다면(부딪혔다면) 피격 이펙트 생성(표시)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal; // 법선 벡터 : 직선에 대하여 수직인 벡터
                BulletEffect.Play();
            }
        }
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void Reload()
    {
        _currentBulletCount = MaxBulletCount;
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void ResetReload()
    {
        _reloadTimer = 0f;
        _isReloading = false;
    }
}
