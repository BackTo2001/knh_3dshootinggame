using System;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    // 콜백 함수 : 어떤 함수를 기억해놨다가 특정 시점 혹은 특정 작업이 완료된 후 호출하는 함수
    private Action _closeCallback;

    public void Open(Action closeCallback = null)
    {
        // 팝업을 열 때 콜백 함수를 저장
        // null일 경우 기본값으로 null을 사용
        // null이 아닐 경우 해당 함수를 저장

        _closeCallback = closeCallback;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        // 팝업을 닫을 때 저장된 콜백 함수를 호출

        _closeCallback?.Invoke();

        gameObject.SetActive(false);
    }
}
