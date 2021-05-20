using UnityEngine;
using UnityEngine.UI;

// Darkmode の切り替えを伝える
public class DarkmodeToggle : MonoBehaviour
{
    [SerializeField] Slider slider;

    public bool darkmode = true; // Darkmodeに変更するか

    void Start()
    {
        InitializeSetting();
        slider.onValueChanged.AddListener(HandleDarkmodeToggle);
        SettingMenu.InitializeSettings += InitializeSetting;
    }

    void HandleDarkmodeToggle(float value)
    {
        // value = 1(on) / 0(off)
        darkmode = (value == 1) ? true : false;
    }

    // 初期化処理
    void InitializeSetting()
    {
        darkmode = UIManager.Instance.Darkmode;
    }
}
