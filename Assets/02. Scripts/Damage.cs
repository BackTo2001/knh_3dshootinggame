using UnityEngine;

public class Damage : MonoBehaviour
{
    public int Value;                   // ÇÇÇØ·®
    public GameObject From;             // ÇÇÇØ¸¦ ÁØ °´Ã¼
    public float KnockBackPower = 5f;        // ³Ë¹é Èû

    public Damage(int value, GameObject from, float knockBackPower = 5f)
    {
        Value = value;
        From = from;
        KnockBackPower = knockBackPower;
    }
}