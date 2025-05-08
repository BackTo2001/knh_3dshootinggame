using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private Image bloodImage; // 혈흔 이미지
    [SerializeField] private float fadeDuration = 1.5f; // 알파값이 서서히 사라지는 시간

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (bloodImage != null)
        {
            bloodImage.color = new Color(1, 1, 1, 0); // 초기 알파값 0
        }
    }

    public void ShowBloodEffect()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // 기존 코루틴 중지
        }

        fadeCoroutine = StartCoroutine(FadeOutBlood());
    }

    private IEnumerator FadeOutBlood()
    {
        if (bloodImage == null) yield break;

        bloodImage.color = new Color(1, 1, 1, 1); // 알파값 1로 설정
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // 알파값 서서히 감소
            bloodImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        bloodImage.color = new Color(1, 1, 1, 0); // 완전히 투명하게
    }
}
