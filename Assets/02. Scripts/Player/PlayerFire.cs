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
    public float ReloadTime = 2f;
    private float _reloadTimer = 0f;
    private bool _isReloading = false;

    // ��ǥ : ���콺�� ���� ��ư�� ������ ī�޶� �ٶ󺸴� �������� ���� �߻��ϰ� �ʹ�.
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
        if (Input.GetMouseButtonDown(0))
        {
            FireBullet();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
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

            // ������ �� ���
            float throwPower = Mathf.Lerp(MinThrowPower, MaxThrowPower, _holdTime / MaxHoldTime);

            // ��ź ���� �� �߻�
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
            // �Ѿ� �߻� ����
            // 1. ���̸� �����ϰ� �߻� ��ġ�� ���� ������ ����
            Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
            // 2. ���̿� �ε��� ��ü�� ������ ������ ������ ����
            RaycastHit hitInfo = new RaycastHit();
            // 3. ���̸� �߻��� ����
            bool isHit = Physics.Raycast(ray, out hitInfo);
            if (isHit)
            {
                // 3-1. ������ �����Ͱ� �ִٸ�(�ε����ٸ�) �ǰ� ����Ʈ ����(ǥ��)
                BulletEffect.transform.position = hitInfo.point;
                BulletEffect.transform.forward = hitInfo.normal; // ���� ���� : ������ ���Ͽ� ������ ����
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
