﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // ************** 属性 *****************
    // 单例
    public static UIManager Instance { get; private set; }
    // *************** 引用 *****************
    public Canvas canvas;
    // 吐司
    public GameObject toastPrefab;
    // 提示
    public Tip tip;
    // 信息面板
    public GameObject infoPanel;
    // 水晶数量
    public Text crystalText;
    // 倒计时文本
    public Text timeText;
    // 背景
    public Image bg;
    // 天数文本
    public Text dayText;
    
    // *************** 变量 *****************
    private void Awake()
    {
        Instance = this;
    }

    public void CreateToast(string msg = null)
    {
        var instantiate = Instantiate(toastPrefab, canvas.transform);
        if (msg is not null)
        {
            instantiate.GetComponent<Toast>().Show(msg);
        }
        
    }
    
    public void ShowTip(string msg)
    {
        tip.Show(msg);
    }
    
    public void CloseTip()
    {
        tip.Close();
    }

    public void ShowInfo(BaseBuild baseBuild,Vector3 pos,string name,string msg)
    {
        var component = infoPanel.GetComponent<Info>();
        component.SetInfo(baseBuild,name,msg);
        infoPanel.transform.position = pos;
        infoPanel.SetActive(true);
    }

    public void HideInfo()
    {
        infoPanel.SetActive(false);
    }
    
    public void SetCrystal(int num)
    {
        crystalText.text = num.ToString();
    }
    
    public void SetTime(int time)
    {
        bg.gameObject.SetActive(GameManager.Instance.IsNight);
        timeText.text = time.ToString();
    }

    public void SetDay(int num)
    {
        dayText.text = $"已坚持\n{num}\n天";
    }
    
    public void GameOver()
    {
        SceneManager.LoadSceneAsync("End", LoadSceneMode.Additive);
        Time.timeScale = 0;
    }
    
}