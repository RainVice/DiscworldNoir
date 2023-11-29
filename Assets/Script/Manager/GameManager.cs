
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 整个游戏的管理器主要用于数据交换
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager Instance => m_instance;

    // ************** 属性 *****************
    public GameObject CurSelectedObject
    {
        get => m_curSelectedObject;
        // 实例化预制体
        set => m_curSelectedObject = value;
    }
    
    // *************** 变量 *****************
    // 单例
    private static GameManager m_instance;
    // 当前选择的一个物体
    private GameObject m_curSelectedObject;
    // 建筑集合分类
    private Dictionary<Type,List<BaseBuild>> m_builds;
    // 建筑集合
    private Dictionary<Vector3Int,BaseBuild> m_buildList;

    private void Awake()
    {
        m_instance = this;
    }
    
    /// <summary>
    /// 添加建筑
    /// </summary>
    /// <param name="cellPos"></param>
    /// <param name="build"></param>
    public void AddBuild(Vector3Int cellPos,GameObject build)
    {
        if (m_buildList == null)
        {
            m_buildList = new Dictionary<Vector3Int, BaseBuild>();
        }
        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        m_buildList.Add(cellPos,baseBuild);
    }
    
    /// <summary>
    /// 返回指定坐标的建筑
    /// </summary>
    /// <param name="cellPos"></param>
    /// <returns></returns>
    public BaseBuild GetBuild(Vector3Int cellPos)
    {
        if (m_buildList == null)
        {
            return null;
        }
        return m_buildList.TryGetValue(cellPos, out var build) ? build : null;
    }
    
    
    /// <summary>
    /// 添加游戏对象进入字典中保存，方便下次批量调用
    /// </summary>
    /// <param name="build"></param>
    public void AddBuild(GameObject build)
    {
        if (m_builds == null)
        {
            m_builds = new Dictionary<Type, List<BaseBuild>>();
        }
        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        if (!m_builds.ContainsKey(baseBuild.GetType()))
        {
            m_builds.Add(baseBuild.GetType(), new List<BaseBuild>());
        }
        m_builds[baseBuild.GetType()].Add(baseBuild);
    }
    
    /// <summary>
    /// 在字典中删除指定游戏对象
    /// </summary>
    /// <param name="build"></param>
    public void RemoveBuild(GameObject build)
    {
        if (m_builds == null)
        {
            return;
        }
        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        if (m_builds.ContainsKey(baseBuild.GetType()))
        {
            m_builds[baseBuild.GetType()].Remove(baseBuild);
        }
    }
    
}