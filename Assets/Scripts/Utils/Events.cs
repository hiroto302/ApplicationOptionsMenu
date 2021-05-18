using UnityEngine.Events;

// Event を管理するクラス
public class Events
{
    // GameManager の State が更新された時に発生する event (第一引数：変更した後の状態, 第二引数 : 変更する前の状態)
    [System.Serializable] public class EventGamState : UnityEvent<GameManager.GameState, GameManager.GameState>{}
}
