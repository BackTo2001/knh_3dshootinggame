using UnityEngine;

public class GunFire : MonoBehaviour, IWeapon
{
    [Header("�Ѿ� ")]
    public GameObject FirePosition;
    public ParticleSystem BulletEffect;

    private int _maxBulletCount = 50;
    private int _currentBulletCount = 50;

    public int MaxBulletCount => _maxBulletCount;
    public int CurrentBulletCount => _currentBulletCount;

    [Header("�߻� ��Ÿ��")]
    public float FireInterval = 0.5f;
    private float _fireTimer = 0f;

    [Header("������")]
    public float ReloadTime = 2f;
    private float _reloadProgress = 0f;
    private bool _isReloading = false;
    public bool IsReloading => _isReloading;

    [Header("��")]
    public GameObject UI_SniperZoom;
    public GameObject UI_Crosshair;
    public float ZoomInSize = 15f;
    public float ZoomOutSize = 60f;
    private bool _zoomMode = false;

    private void Update()
    {
        HandleZoom();

        if (Input.GetMouseButton(0))
        {
            if (_isReloading)
            {
                CancelReload();
            }
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading && _currentBulletCount < MaxBulletCount)
        {
            Reload();
        }

        if (_isReloading)
        {
            ContinueReload();
        }

        if (_fireTimer > 0f)
        {
            _fireTimer -= Time.deltaTime;
        }
    }
    public void Initialize()
    {
        _currentBulletCount = MaxBulletCount;
        _fireTimer = FireInterval;
        _reloadProgress = 0f;
        _isReloading = false;
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
        UIManager.Instance.ShowReloadText(false);
    }

    public void Attack()
    {
        if (_fireTimer <= 0f && _currentBulletCount > 0)
        {
            FireBullet();
            _fireTimer = FireInterval;
        }
    }

    private void FireBullet()
    {
        _currentBulletCount--;
        // �Ѿ� �߻� ����
        // 1. ���̸� �����ϰ� �߻� ��ġ�� ���� ������ ����
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
        // 2. ���̿� �ε��� ��ü�� ������ ������ ���� ����
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // 3-1. ������ �����Ͱ� �ִٸ�(�ε����ٸ�) �ǰ� ����Ʈ ����(ǥ��)
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal; // ���� ���� : ������ ���Ͽ� ������ ����
            BulletEffect.Play();

            IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage damage = new Damage(40, gameObject);
                damageable.TakeDamage(damage); // ������ ���ظ� ����
            }
        }
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
    }


    public void Reload()
    {
        _isReloading = true;
        _reloadProgress = 0f;
        UIManager.Instance.ShowReloadText(true);
    }

    private void ContinueReload()
    {
        _reloadProgress += Time.deltaTime / ReloadTime;
        UIManager.Instance.UpdateReloadTextWithColor(_reloadProgress);

        if (_reloadProgress >= 1f)
        {
            CompleteReload();
        }
    }

    public void CancelReload()
    {
        _isReloading = false;
        _reloadProgress = 0f;
        UIManager.Instance.ShowReloadText(false);
    }

    private void CompleteReload()
    {
        _isReloading = false;
        _currentBulletCount = MaxBulletCount;
        UIManager.Instance.RefreshBulletText(_currentBulletCount, MaxBulletCount);
        UIManager.Instance.ShowReloadText(false);
    }

    private void HandleZoom()
    {
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            _zoomMode = !_zoomMode;
            if (_zoomMode)
            {
                UI_Crosshair.SetActive(false);
                UI_SniperZoom.SetActive(true);
                Camera.main.fieldOfView = ZoomInSize;
            }
            else
            {
                UI_Crosshair.SetActive(true);
                UI_SniperZoom.SetActive(false);
                Camera.main.fieldOfView = ZoomOutSize;
            }
        }
    }

}
