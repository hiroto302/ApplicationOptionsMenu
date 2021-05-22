using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// たまに出現する SD タイプの Unity-Chan 制御スクリプト
public class UnityChan : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Canvas _unitychanCanvas;
    void OnMouseDown()
    {
        // 触れた時, RaiseHand animation 開始
        _animator.SetTrigger("HasTouched");
    }

    // RaiseHand animation の event で呼ばれるメソッド
    void OnHandRaise()
    {
        // 会話表示
        SetConversationPanelActive(true);
    }

    void OnHandDown()
    {
        // 会話非表示
        SetConversationPanelActive(false);
    }

    // 会話する内容が記述されてる Canvas の表示・非表示
    void SetConversationPanelActive(bool show)
    {
        _unitychanCanvas.gameObject.SetActive(show);
    }
}
