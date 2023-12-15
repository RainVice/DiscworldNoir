using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 信息提示面板
/// </summary>
public class Tip : MonoBehaviour
{
    private Text text;

    private void Awake()
    {
        text = GetComponentInChildren<Text>();
        gameObject.SetActive(false);
    }

    public void Show(string msg)
    {
        gameObject.SetActive(true);
        text.text = msg;
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
    
    
}
