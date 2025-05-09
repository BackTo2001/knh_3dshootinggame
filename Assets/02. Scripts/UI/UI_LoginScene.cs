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

    [Header("패널")]
    public GameObject LoginPanel;
    public GameObject RegisterPanel;

    [Header("로그인")]
    public UI_InputFields LoginInputFields;


    [Header("회원가입")]
    public UI_InputFields RegisterInputFields;

    private const string PREFIX = "ID_";
    private const string SALT = "1028038";

    // 게임 시작하면 로그인 패널
    private void Start()
    {
        LoginPanel.SetActive(true);
        RegisterPanel.SetActive(false);

        LoginCheck();
    }

    // 회원가입 버튼 클릭
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

    // 회원가입
    public void Register()
    {
        // 1. 아이디 입력을 확인한다.
        string id = RegisterInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            RegisterInputFields.ResultText.text = "아이디를 입력하세요.";
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = RegisterInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            RegisterInputFields.ResultText.text = "비밀번호를 입력하세요.";
            return;
        }

        // 3. 2차 비밀번호 입력을 확인하고, 1차 비밀번호와 입력과 같은지 확인한다.
        string passwordConfirm = RegisterInputFields.PasswordConfirmInputField.text;
        if (string.IsNullOrEmpty(passwordConfirm))
        {
            RegisterInputFields.ResultText.text = "비밀번호를 입력하세요.";
            return;
        }
        if (password != passwordConfirm)
        {
            RegisterInputFields.ResultText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        // 4. PlayerPrefs를 이용해서 아이디와 비밀번호를 저장한다.
        PlayerPrefs.SetString(PREFIX + id, Encryption(password + SALT));

        // 5. 로그인 창으로 돌아간다. (이때, 아이디는 자동 입력되어 있다.)
        OnClickGotoLoginButton();
    }

    public string Encryption(string text)
    {
        // SHA256 해시 알고리즘을 사용하여 문자열을 암호화합니다.
        SHA256 sha256 = SHA256.Create();

        // 운영체제 혹은 프로그래밍 언어별로 string 표현하는 방식이 다 다르므로
        // UTF8 버전 바이트 배열로 바꿔야한다.
        byte[] bytes = Encoding.UTF8.GetBytes(text);
        byte[] hash = sha256.ComputeHash(bytes);

        string resultText = string.Empty;
        foreach (byte b in hash)
        {
            // 16진수로 변환하여 문자열에 추가합니다.
            // byte를 다시 string으로 바꿔서 이어붙이기
            resultText += b.ToString("x2");
        }

        return resultText;

    }

    public void Login()
    {
        // 1. 아이디 입력을 확인한다.
        string id = LoginInputFields.IDInputField.text;
        if (string.IsNullOrEmpty(id))
        {
            LoginInputFields.ResultText.text = "아이디를 입력하세요.";
            return;
        }

        // 2. 비밀번호 입력을 확인한다.
        string password = LoginInputFields.PasswordInputField.text;
        if (string.IsNullOrEmpty(password))
        {
            LoginInputFields.ResultText.text = "비밀번호를 입력하세요.";
            return;
        }

        // 3. PlayerPrefs에서 아이디와 비밀번호를 가져온다.
        if (!PlayerPrefs.HasKey(PREFIX + id))
        {
            LoginInputFields.ResultText.text = "아이디와 비밀번호를 확인해주세요.";
            return;
        }

        string hashedPassword = PlayerPrefs.GetString(PREFIX + id);
        if (hashedPassword != Encryption(password + SALT))
        {
            LoginInputFields.ResultText.text = "아이디와 비밀번호를 확인해주세요.";
            return;
        }

        // 4. 맞다면 로그인
        Debug.Log("로그인 성공");
        DOTween.KillAll();
        SceneManager.LoadScene(1);

    }

    // 아이디와 비밀번호 InputField 값이 바뀌었을 경우에만 호출
    public void LoginCheck()
    {
        string id = LoginInputFields.IDInputField.text;
        string password = LoginInputFields.PasswordInputField.text;

        LoginInputFields.ConfirmButton.enabled = !string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(password);
    }
}
