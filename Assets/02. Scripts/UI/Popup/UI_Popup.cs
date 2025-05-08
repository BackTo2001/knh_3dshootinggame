using System;
using UnityEngine;

public class UI_Popup : MonoBehaviour
{
    // �ݹ� �Լ� : � �Լ��� ����س��ٰ� Ư�� ���� Ȥ�� Ư�� �۾��� �Ϸ�� �� ȣ���ϴ� �Լ�
    private Action _closeCallback;

    public void Open(Action closeCallback = null)
    {
        // �˾��� �� �� �ݹ� �Լ��� ����
        // null�� ��� �⺻������ null�� ���
        // null�� �ƴ� ��� �ش� �Լ��� ����

        _closeCallback = closeCallback;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        // �˾��� ���� �� ����� �ݹ� �Լ��� ȣ��

        _closeCallback?.Invoke();

        gameObject.SetActive(false);
    }
}
