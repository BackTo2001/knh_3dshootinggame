using DG.Tweening;
using System;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public class UI_InputFields
{
    public TextMeshProUGUI ResultText;
    public TMP_InputField IDInputField;
    public TMP_InputField PasswordInputField;
    public TMP_InputField PasswordConfirmInputField;
    public Button ConfirmButton;
}

public class UI_LoginScene : MonoBehaviour
{

    [Header("�г�")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    [Header("�α���")]
    public UI_InputFields LoginInputFields;


    [Header("ȸ������")]
    public UI_InputFields RegisterInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "1028038";

    // ���� �����ϸ� �α��� �г�
    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        LoginCheck();
    }

    // ȸ������ ��ư Ŭ��
    public void OnClickGotoRegisterButton()
    {
        LoginPanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }

    public void OnClickGotoLoginButton()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }

    // ȸ������
    public void Register()
    {
        // 1. ���̵� �Է��� Ȯ���Ѵ�.
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            RegisterInputFields.ResultText.text = "���̵� �Է��ϼ���.";
            return;
        }

        // 2. ��й�ȣ �Է��� Ȯ���Ѵ�.
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            RegisterInputFields.ResultText.text = "��й�ȣ�� �Է��ϼ���.";
            return;
        }

        // 3. 2�� ��й�ȣ �Է��� Ȯ���ϰ�, 1�� ��й�ȣ�� �Է°� ������ Ȯ���Ѵ�.
        string passwordConfirm = RegisterInputFields.PasswordConfirmInputField.text;
        if (string.IsNullOrEmpty(passwordConfirm))
        {
            RegisterInputFields.ResultText.text = "��й�ȣ�� �Է��ϼ���.";
            return;
        }
        if (password != passwordConfirm)
        {
            RegisterInputFields.ResultText.text = "��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
            return;
        }

        // 4. PlayerPrefs�� �̿��ؼ� ���̵�� ��й�ȣ�� �����Ѵ�.
        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));

        // 5. �α��� â���� ���ư���. (�̶�, ���̵�� �ڵ� �ԷµǾ� �ִ�.)
        OnClickGotoLoginButton();
    }

    public string Encryption(string text)
    {
        // SHA256 �ؽ� �˰����� ����Ͽ� ���ڿ��� ��ȣȭ�մϴ�.
        SHA256 sha256 = SHA256.Create();

        // �ü�� Ȥ�� ���α׷��� ���� string ǥ���ϴ� ����� �� �ٸ��Ƿ�
        // UTF8 ���� ����Ʈ �迭�� �ٲ���Ѵ�.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            // 16������ ��ȯ�Ͽ� ���ڿ��� �߰��մϴ�.
            // byte�� �ٽ� string���� �ٲ㼭 �̾���̱�
            resultText += b.ToString("x2");
        }

        return resultText;

    }

    public void Login()
    {
        // 1. ���̵� �Է��� Ȯ���Ѵ�.
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            LoginInputFields.ResultText.text = "���̵� �Է��ϼ���.";
            return;
        }

        // 2. ��й�ȣ �Է��� Ȯ���Ѵ�.
        string password = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            LoginInputFields.ResultText.text = "��й�ȣ�� �Է��ϼ���.";
            return;
        }

        // 3. PlayerPrefs���� ���̵�� ��й�ȣ�� �����´�.
        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            LoginInputFields.ResultText.text = "���̵�� ��й�ȣ�� Ȯ�����ּ���.";
            return;
        }

        string hashedPassword = PlayerPrefs.GetString(PREFIX + id);
        if (hashedPassword != Encryption(password + SALT))
        {
            LoginInputFields.ResultText.text = "���̵�� ��й�ȣ�� Ȯ�����ּ���.";
            return;
        }

        // 4. �´ٸ� �α���
        Debug.Log("�α��� ����");
        DOTween.KillAll();
        SceneManager.LoadScene(1);

    }

    // ���̵�� ��й�ȣ InputField ���� �ٲ���� ��쿡�� ȣ��
    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
