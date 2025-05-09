using UnityEngine;

public class BombWeapon : MonoBehaviour, IWeapon
{
    [Header("폭탄 위치 및 프리팹")]
    public GameObject FirePosition;
    public GameObject BombPrefab;

    [Header("폭탄 던지는 힘")]
    public float MinThrowPower = 5f;
    public float MaxThrowPower = 20f;
    public float MaxHoldTime = 2f;
    private float _holdTime = 0f;

    [Header("폭탄 개수")]
    private int _maxBombCount = 3;
    private int _currentBombCount = 3;

    public int MaxBombCount => _maxBombCount;
    public int CurrentBombCount => _currentBombCount;

    private bool _isReloading = false;
    public bool IsReloading => _isReloading;

    private void Update()
    {
        if (Input.GetMouseButton(1))
        {
            _holdTime += Time.deltaTime;
            _holdTime = Mathf.Clamp(_holdTime, 0f, MaxHoldTime);

            float normalizedPower = _holdTime / MaxHoldTime;
            UIManager.Instance.UpdateBombThrowPower(normalizedPower);
            UIManager.Instance.ShowBombThrowSlider(true);
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (_isReloading) CancelReload();
            Attack();
            _holdTime = 0f;
            UIManager.Instance.ShowBombThrowSlider(false);
        }

        if (Input.GetKeyDown(KeyCode.R) && !_isReloading && _currentBombCount < _maxBombCount)
        {
            Reload();
        }
    }

    public void Initialize()
    {
        _currentBombCount = _maxBombCount;
        _holdTime = 0f;
        _isReloading = false;
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        UIManager.Instance.ShowBombThrowSlider(false);
    }

    public void Attack()
    {
        if (_currentBombCount <= 0) return;

        _currentBombCount--;

        Bomb bomb = BombPool.Instance.GetBomb();
        if (bomb == null)
        {
            return;
        }

        bomb.transform.position = FirePosition.transform.position;

        float throwPower = Mathf.Lerp(MinThrowPower, MaxThrowPower, _holdTime / MaxHoldTime);

        Rigidbody rb = bomb.GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        rb.AddTorque(Vector3.one, ForceMode.Impulse);

        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }

    public void Reload()
    {
        _isReloading = true;
        _currentBombCount = _maxBombCount;
        UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
    }

    public void CancelReload()
    {
        _isReloading = false;
    }

    public void RecoverBomb()
    {
        if (_currentBombCount < _maxBombCount)
        {
            _currentBombCount++;
            UIManager.Instance.RefreshBombText(_currentBombCount, MaxBombCount);
        }
    }
}
