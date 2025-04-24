using UnityEngine;

public class Damage : MonoBehaviour
{
    public int Value;                   // ���ط�
    public GameObject From;             // ���ظ� �� ��ü
    public float KnockBackPower = 5f;        // �˹� ��

    public Damage(int value, GameObject from, float knockBackPower = 5f)
    {
        Value = value;
        From = from;
        KnockBackPower = knockBackPower;
    }
}