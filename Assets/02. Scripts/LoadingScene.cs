using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // ��ǥ : ���� ���� '�񵿱� ���'���� �ε��ϰ� �ʹ�.
    // ����, �ε� ������� �ð������� ǥ���ϰ� �ʹ�.
    // % ���α׷��� �ٿ� %�� �ؽ�Ʈ

    // �Ӽ�
    // - ���� �� ��ȣ(�ε���)
    public int NextSceneIndex = 2;

    // - ���α׷��� �����̴���
    public Slider ProgressSlider;

    // - ���α׷��� �ؽ�Ʈ
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // ������ ���� �񵿱�� �ε�
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // �񵿱�� �ε�Ǵ� ���� ����� ȭ�鿡 ������ �ʰ� �Ѵ�.

        // �ε��� �Ǵ� ���� ����ؼ� �ݺ���
        while (ao.isDone == false)
        {
            // �񵿱�� ������ �ڵ�
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";

            if (ao.progress <= 0.2f)
            {
                ProgressText.text = "���ֺ� �з� ����. �������� ��ġ �¶��� ��...";
            }
            else if (ao.progress <= 0.4f)
            {
                ProgressText.text = "��ǥ Ȯ��. ��ǥ ���ϰ���� ���� �غ� ���Դϴ�.";
            }
            else if (ao.progress <= 0.6f)
            {
                ProgressText.text = "�ö�� ������, �߷� ����ź, ��� ��� üũ �Ϸ�.";
            }
            else if (ao.progress <= 0.8f)
            {
                ProgressText.text = "���̴��� ������ ����. ���� �غ� �¼� ����.";
            }

            if (ao.progress >= 0.9f)
            {
                ProgressText.text = "����� ���ϴ�, ��ɰ�. ���ְ� ����� ���Ѻ��� �ֽ��ϴ�.";
                // �����̽��� ������ �� Ȱ��ȭ
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ao.allowSceneActivation = true; // ���� Ȱ��ȭ
                }
            }

            // yield return new WaitForSeconds(1); // 1�� ���
            yield return null; // 1 ������ ���

        }
    }
}
