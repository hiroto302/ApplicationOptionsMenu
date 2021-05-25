using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SkyBox を制御するクラス
public class SkyBoxManager : MonoSingleton<SkyBoxManager>
{
    [SerializeField] Material _eveningSky; // 夕空
    [SerializeField] Material _nighSky;    // 夜空

    void Start()
    {
        GameManager.Instance.OnGameStateChange.AddListener(HandleGameStateChange);
    }

    void HandleGameStateChange(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        // StartScene => MainScen
        if(previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING)
        {
            ChangeSkyBoxMaterial(_nighSky);
        }

        // MainScene => StartScene
        if(previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
        {
            // Start Menu が FadeIn 完了した時、 ChangeEveningSky を実行する
        }
    }
    void ChangeSkyBoxMaterial(Material skyMaterial)
    {
        RenderSettings.skybox = skyMaterial;
    }

    public void ChangeEveningSky()
    {
        ChangeSkyBoxMaterial(_eveningSky);
    }

}
