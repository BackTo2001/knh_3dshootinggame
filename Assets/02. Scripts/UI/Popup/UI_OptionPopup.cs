public class UI_OptionPopup : UI_Popup
{

    public void OnClickContinueButton()
    {
        GameManager.Instance.Continue();

        Close();
    }

    public void OnClickRestartButton()
    {
        GameManager.Instance.Restart();
    }

    public void OnClickExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickCreditButton()
    {
        PopupManager.Instance.Open(PopupType.UI_CreditPopup);
    }
}
