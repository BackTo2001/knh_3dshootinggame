using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    // �ʿ� �Ӽ�

    // - �߻� ��ġ
    public GameObject FirePosition;

    // - ��ź ������
    public GameObject BombPrefab;
    // - ��ź ������ ��
    public float MinThrowPower = 5f;
    public float MaxThrowPower = 20f;
    private float _holdTime = 0f;
    public float MaxHoldTime = 2f;
    // - ��ź �ִ� ����
    private int _maxBombCount = 3;
    private int _currentBombCount = 3;

    public int MaxBombCount => _maxBombCount;
    public int CurrentBombCount => _currentBombCount;


    // - �Ѿ� �ִ� ����
    private int _maxBulletCount = 50;
    private int _currentBulletCount = 50;

    public int MaxBulletCount => _maxBulletCount;
    public int CurrentBulletCount => _currentBombCount;

    // - �Ѿ� ��Ÿ��
    public float FireInterval = 0.5f;
    private float _fireTimer = 0f;

    // - �Ѿ� ������ Ÿ��
    public float ReloadTime = 2f;       // ������ �ð�
    private float _reloadProgress = 0f; // ������ ���� ����
    private bool _isReloading = false;  // ������ ����

    // ��ǥ : ���콺�� ���� ��ư�� ������ ī�޶� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
    public ParticleSystem BulletEffect;

    private void Start()
    {

        //BombPool.Instance.SetPoolSize(MaxBombCount);

        //_fireTimer = FireInterval;

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void Update()
    {
        // ��ź ���
        if (Input.GetMouseButton(1))
        {
            _holdTime += Time.deltaTime; // ������ �ð� 
            _holdTime = Mathf.Clamp(_holdTime, 0f, MaxHoldTime); // �ִ� �ð� ����

            // ������ ���� 0~1�� ����ȭ�Ͽ� UI ������Ʈ
            float normalizedPower = _holdTime / MaxHoldTime;
            UIManager.Instance.UpdateBombThrowPower(normalizedPower);
            UIManager.Instance.ShowBombThrowSlider(true); // Slider ǥ��

        }
        if (Input.GetMouseButtonUp(1))
        {
            ThrowBomb();
            _holdTime = 0f; // ��ź ������ �� �ʱ�ȭ
            UIManager.Instance.ShowBombThrowSlider(false); // Slider �����
        }

        // �Ѿ� �߻�(������ ���)
        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư�� ������ �ִ� ����
        {
            if (_isReloading)
            {
                CancleReload();
            }

            FireBulletContinuous();
        }

        // ������ ����
        if (Input.GetKeyDown(KeyCode.R) && !_isReloading && _currentBulletCount < MaxBulletCount)
        {
            StartReload();
        }

        // ������ ����
        if (_isReloading)
        {
            ContinueReload();
        }

        // Ÿ�̸� ������Ʈ
        if (_fireTimer > 0)
        {
            _fireTimer -= Time.deltaTime;
        }
        // Ray : ������(������ġ, ����)
        // Raycast : �������� �߻�
        // RaycastHit : �������� ��ü�� �ε����ٸ� �� ������ �����ϴ� ����ü
    }

    public void ThrowBomb()
    {
        if (_currentBombCount > 0)
        {
            _currentBombCount--;

            // ��ź ��������
            Bomb bomb = BombPool.Instance.GetBomb();
            if (bomb == null)
            {
                Debug.Log("��ź�� �����ϴ�.");
                return;
            }

            // ��ź ��ġ
            bomb.transform.position = FirePosition.transform.position;

            // ������ �� ���
            float throwPower = Mathf.Lerp(MinThrowPower, MaxThrowPower, _holdTime / MaxHoldTime);

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
            // �Ѿ� �߻� ����
            // 1. ���̸� �����ϰ� �߻� ��ġ�� ���� ������ ����
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            // 2. ���̿� �ε��� ��ü�� ������ ������ ������ ����
            RaycastHit hitInfo;
            // 3. ���̸� �߻��� ����
            if (Physics.Raycast(ray, out hitInfo))
            {
                // 3-1. ������ �����Ͱ� �ִٸ�(�ε����ٸ�) �ǰ� ����Ʈ ����(ǥ��)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal; // ���� ���� : ������ ���Ͽ� ������ ����
                BulletEffect.Play();

                IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
                //if(hitInfo.collider.TryGetComponent<IDamageable>(out damageable))
                //{

                //}
                if (damageable != null)
                {
                    Damage damage = new Damage(10, gameObject);
                    damageable.TakeDamage(damage); // ������ ���ظ� ����
                }
            }
        }
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }

    private void FireBulletContinuous()
    {
        if (_fireTimer <= 0 && _currentBulletCount > 0)
        {
            FireBullet(); // �Ѿ� �߻�
            _fireTimer = FireInterval; // ��Ÿ�� �ʱ�ȭ
        }
    }

    private void StartReload()
    {
        _isReloading = true;
        _reloadProgress = 0f;
        //UIManager.Instance.ShowReload(true);
        UIManager.Instance.ShowReloadText(true); // �ؽ�Ʈ ǥ��
    }
    private void ContinueReload()
    {
        _reloadProgress += Time.deltaTime / ReloadTime;
        //UIManager.Instance.UpdateReload(_reloadProgress);
        UIManager.Instance.UpdateReloadTextWithColor(_reloadProgress); // �ؽ�Ʈ�� ���� ������Ʈ

        if (_reloadProgress >= 1f)
        {
            CompleteReload();
        }
    }
    private void CompleteReload()
    {
        _isReloading = false;
        _currentBulletCount = MaxBulletCount;
        _currentBombCount = MaxBombCount; // ��ź ���� �ʱ�ȭ
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount); // UI ������Ʈ
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount); // UI ������Ʈ
        UIManager.Instance.ShowReloadText(false); // �ؽ�Ʈ �����
    }
    private void CancleReload()
    {
        _isReloading = false;
        _reloadProgress = 0f;
        //UIManager.Instance.ShowReload(false);
        UIManager.Instance.ShowReloadText(false); // �ؽ�Ʈ �����
    }


}
