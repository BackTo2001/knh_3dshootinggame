using System;
using System.Collections.Generic;
using UnityEngine;


public enum PopupType
{
    UI_OptionPopup,
    UI_CreditPopup,
}

public class PopupManager : MonoBehaviour
{
    public static PopupManager Instance;

    [Header("# 팝업 참조")]
    public List<UI_Popup> Popups;

    private List<UI_Popup> _openedPopups = new List<UI_Popup>(); // null은 아니지만 비어있는 리스트
    // 1. 다른 개발자에게 데이터의 끝 부분만 다룬다는 것과 후입 선출이라는 것을 명시적으로 알린다. -> 안정성 up
    // 2. 그 구조가 보인다. + 제한적인 내용만 쓰는 경우에는 편하다 (추상화가 더 높다.)
    // 스택(마지막), 큐(앞), 데크(앞,마지막) -> 리스트(Array)의 제한적인 버전

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Open(PopupType type, Action closeCallback = null)
    {
        Open(type.ToString(), closeCallback);
    }

    private void Open(string popupName, Action closeCallback)
    {

        foreach (UI_Popup popup in Popups)
        {
            if (popup.gameObject.name == popupName)
            {
                popup.Open(closeCallback);
                // 팝업을 열 때마다 리스트에 추가
                _openedPopups.Add(popup);
                break;
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_openedPopups.Count > 0)
            {
                while (true)
                {
                    bool opened = _openedPopups[_openedPopups.Count - 1].isActiveAndEnabled;
                    // 마지막 팝업 닫기
                    _openedPopups[_openedPopups.Count - 1].Close();
                    _openedPopups.RemoveAt(_openedPopups.Count - 1);

                    // 열려있는 팝업을 닫았거나 || 더이상 닫을 팝업이 없으면 탈출
                    if (opened || _openedPopups.Count == 0)
                    {
                        break;
                    }
                }
            }
            else
            {
                GameManager.Instance.Pause();
            }
        }
    }
}
