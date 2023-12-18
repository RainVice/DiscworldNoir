
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 结束界面的UI
/// </summary>
public class EndUI : MonoBehaviour
{
    public Text dayText;

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadSceneAsync("Game");
            Time.timeScale = 1;
        }
    }

    private void Start()
    {
        dayText.text = $"游戏结束，本次经历了 {GameManager.Instance.Day} 次黑夜";
    }
    
}
