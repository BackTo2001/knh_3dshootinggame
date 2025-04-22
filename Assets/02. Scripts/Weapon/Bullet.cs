using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject ExplosionEffectPrefab;

    // Ãæµ¹ÇßÀ» ¶§
    private void OnCollisionEnter(Collision collision)
    {
        GameObject effectObject = Instantiate(ExplosionEffectPrefab);
        effectObject.transform.position = transform.position;

        Destroy(gameObject); // ¼ö·ùÅº ÆÄ±«
    }
}
