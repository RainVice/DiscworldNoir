using System;
using Effect;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartUI : MonoBehaviour
{
    public Text titleText;
    public Button startButton;
    public Button exitButton;


    private void Start()
    {
        startButton.onClick.AddListener(() =>
        {
            this.CreateEffect()
                .AddEffect(titleText.transform.ScaleTFTo(Vector3.zero, 0.5f))
                .AddEffect(startButton.transform.ScaleTFTo(Vector3.zero, 0.5f))
                .AddEffect(exitButton.transform.ScaleTFTo(Vector3.zero, 0.5f))
                .Play(() =>
                {

                    GameManager.Instance.IsStart = true;
                    
                    UIManager.Instance.canvas.gameObject.SetActive(true);
                    // 关闭当前场景
                    SceneManager.UnloadSceneAsync("Start");
                });
        });
        
        exitButton.onClick.AddListener(() =>
        {
            #if UNITY_EDITOR
                // 如果在Unity编辑器中运行
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                // 如果在其他平台中运行
                Application.Quit();
            #endif

        });
    }
}
