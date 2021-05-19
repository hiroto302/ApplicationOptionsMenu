using UnityEngine;
using UnityEngine.UI;

public class SettingButton : MonoBehaviour
{
    [SerializeField] private Button settingButton;

    void Start()
    {
        settingButton.onClick.AddListener(HandleSettingButton);
    }

    // ボタンが押された時の表示
    void HandleSettingButton()
    {
        UIManager.Instance.SettingMenuSetActive(true);
    }
}
