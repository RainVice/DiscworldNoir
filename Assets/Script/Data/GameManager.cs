
using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance => instance;

    public GameObject CurSelectedObject
    {
        get => curSelectedObject;
        // 实例化预制体
        set => curSelectedObject = Instantiate(value, transform.position, Quaternion.identity);
    }
    // *************** 引用
    // 当前选择的一个物体
    private GameObject curSelectedObject;
    
    // 单例
    private static GameManager instance;


    private void Awake()
    {
        instance = this;
    }
}