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

    [Header("# �˾� ����")]
    public List<UI_Popup> Popups;

    private List<UI_Popup> _openedPopups = new List<UI_Popup>(); // null�� �ƴ����� ����ִ� ����Ʈ
    // 1. �ٸ� �����ڿ��� �������� �� �κи� �ٷ�ٴ� �Ͱ� ���� �����̶�� ���� ��������� �˸���. -> ������ up
    // 2. �� ������ ���δ�. + �������� ���븸 ���� ��쿡�� ���ϴ� (�߻�ȭ�� �� ����.)
    // ����(������), ť(��), ��ũ(��,������) -> ����Ʈ(Array)�� �������� ����

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
                // �˾��� �� ������ ����Ʈ�� �߰�
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
                    // ������ �˾� �ݱ�
                    _openedPopups[_openedPopups.Count - 1].Close();
                    _openedPopups.RemoveAt(_openedPopups.Count - 1);

                    // �����ִ� �˾��� �ݾҰų� || ���̻� ���� �˾��� ������ Ż��
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
