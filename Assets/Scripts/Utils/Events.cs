﻿using UnityEngine.Events;

// Event を管理するクラス
public class Events
{
    // GameManager の State が更新された時に発生する event (第一引数：変更した後の状態, 第二引数 : 変更する前の状態)
    [System.Serializable] public class EventGamState : UnityEvent<GameManager.GameState, GameManager.GameState>{}

    // 次の画面に移行する時行われるFade. StartMenu の Fade が完了した時に発生する event (In : false /Out : true, シーン名)
    [System.Serializable] public class EventLoadFadeComplete : UnityEvent<bool, string>{};

    // BGMの音量が変更された時発生する event
    [System.Serializable] public class EventBGMValue : UnityEvent<float>{};
}
