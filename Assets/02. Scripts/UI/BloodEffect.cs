using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BloodEffect : MonoBehaviour
{
    [SerializeField] private Image bloodImage; // ���� �̹���
    [SerializeField] private float fadeDuration = 1.5f; // ���İ��� ������ ������� �ð�

    private Coroutine fadeCoroutine;

    private void Start()
    {
        if (bloodImage != null)
        {
            bloodImage.color = new Color(1, 1, 1, 0); // �ʱ� ���İ� 0
        }
    }

    public void ShowBloodEffect()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine); // ���� �ڷ�ƾ ����
        }

        fadeCoroutine = StartCoroutine(FadeOutBlood());
    }

    private IEnumerator FadeOutBlood()
    {
        if (bloodImage == null) yield break;

        bloodImage.color = new Color(1, 1, 1, 1); // ���İ� 1�� ����
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration); // ���İ� ������ ����
            bloodImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }

        bloodImage.color = new Color(1, 1, 1, 0); // ������ �����ϰ�
    }
}
