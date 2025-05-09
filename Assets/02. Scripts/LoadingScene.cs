using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // 목표 : 다음 씬을 '비동기 방식'으로 로드하고 싶다.
    // 또한, 로딩 진행률을 시각적으로 표현하고 싶다.
    // % 프로그레스 바와 %별 텍스트

    // 속성
    // - 다음 씬 번호(인덱스)
    public int NextSceneIndex = 2;

    // - 프로그레스 슬라이더바
    public Slider ProgressSlider;

    // - 프로그레스 텍스트
    public TextMeshProUGUI ProgressText;

    private void Start()
    {
        StartCoroutine(LoadNextScene_Coroutine());
    }

    private IEnumerator LoadNextScene_Coroutine()
    {
        // 지정된 씬을 비동기로 로드
        AsyncOperation ao = SceneManager.LoadSceneAsync(NextSceneIndex);
        ao.allowSceneActivation = false; // 비동기로 로드되는 씬의 모습이 화면에 보이지 않게 한다.

        // 로딩이 되는 동안 계속해서 반복문
        while (ao.isDone == false)
        {
            // 비동기로 실행할 코드
            ProgressSlider.value = ao.progress;
            ProgressText.text = $"{ao.progress * 100f}%";

            if (ao.progress <= 0.2f)
            {
                ProgressText.text = "우주복 압력 정상. 생명유지 장치 온라인 중...";
            }
            else if (ao.progress <= 0.4f)
            {
                ProgressText.text = "좌표 확인. 목표 은하계로의 워프 준비 중입니다.";
            }
            else if (ao.progress <= 0.6f)
            {
                ProgressText.text = "플라즈마 라이플, 중력 수류탄, 모든 장비 체크 완료.";
            }
            else if (ao.progress <= 0.8f)
            {
                ProgressText.text = "레이더에 움직임 포착. 전투 준비 태세 돌입.";
            }

            if (ao.progress >= 0.9f)
            {
                ProgressText.text = "행운을 빕니다, 사령관. 우주가 당신을 지켜보고 있습니다.";
                // 스페이스바 누르면 씬 활성화
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    ao.allowSceneActivation = true; // 씬을 활성화
                }
            }

            // yield return new WaitForSeconds(1); // 1초 대기
            yield return null; // 1 프레임 대기

        }
    }
}
