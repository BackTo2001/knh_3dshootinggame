using UnityEngine;

public class GunFire : MonoBehaviour, IWeapon
{
    [Header("총알 ")]
    public GameObject FirePosition;
    public ParticleSystem BulletEffect;

    private int _maxBulletCount = 50;
    private int _currentBulletCount = 50;

    public int MaxBulletCount => _maxBulletCount;
    public int CurrentBulletCount => _currentBulletCount;

    [Header("발사 쿨타임")]
    public float FireInterval = 0.5f;
    private float _fireTimer = 0f;

    [Header("재장전")]
    public float ReloadTime = 2f;
    private float _reloadProgress = 0f;
    private bool _isReloading = false;
    public bool IsReloading => _isReloading;

    [Header("줌")]
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
        // 총알 발사 로직
        // 1. 레이를 생성하고 발사 위치와 진행 방향을 설정
        Ray ray = new Ray(FirePosition.transform.position, Camera.main.transform.forward);
        // 2. 레이와 부딪힌 물체의 정보를 저장할 변수 생성
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            // 3-1. 변수에 데이터가 있다면(부딪혔다면) 피격 이펙트 생성(표시)
            BulletEffect.transform.position = hitInfo.point;
            BulletEffect.transform.forward = hitInfo.normal; // 법선 벡터 : 직선에 대하여 수직인 벡터
            BulletEffect.Play();

            IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damage damage = new Damage(40, gameObject);
                damageable.TakeDamage(damage); // 적에게 피해를 입힘
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
