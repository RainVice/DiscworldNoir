using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 整个游戏的管理器主要用于数据交换
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance => m_Instance;

    // ************** 属性 *****************
    public GameObject CurSelectedObject
    {
        get => m_CurSelectedObject;
        // 实例化预制体
        set => m_CurSelectedObject = value;
    }
    // *************** 变量 *****************
    // 单例
    private static GameManager m_Instance;

    // 当前选择的一个物体
    private GameObject m_CurSelectedObject;

    // 建筑集合分类
    private Dictionary<Type, List<BaseBuild>> m_Builds;

    // 建筑集合
    private Dictionary<Vector3Int, BaseBuild> m_BuildList;

    // 地形集合
    private Dictionary<Vector3Int, BaseTerrain> m_TerrainList;

    private void Awake()
    {
        m_Instance = this;
    }
    
    // 是否存在地形
    public bool HasTerrain(Vector3Int cellPos)
    {
        if (m_TerrainList == null)
        {
            m_TerrainList = new Dictionary<Vector3Int, BaseTerrain>();
        }
        return m_TerrainList.ContainsKey(cellPos);
    }
    
    /// <summary>
    /// 添加地形
    /// </summary>
    /// <param name="cellPos"> tile 坐标</param>
    /// <param name="terrain"> 地形 </param>
    public void AddTerrain(Vector3Int cellPos, GameObject terrain)
    {
        if (m_TerrainList == null)
        {
            m_TerrainList = new Dictionary<Vector3Int, BaseTerrain>();
        }
        var baseTerrain = terrain.GetComponent<BaseTerrain>();
        m_TerrainList.Add(cellPos, baseTerrain);
    }

    /// <summary>
    /// 添加建筑
    /// </summary>
    /// <param name="cellPos">tile 坐标</param>
    /// <param name="build">建筑物</param>
    public void AddBuild(Vector3Int cellPos, GameObject build)
    {
        if (m_BuildList == null)
        {
            m_BuildList = new Dictionary<Vector3Int, BaseBuild>();
        }

        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        m_BuildList.Add(cellPos, baseBuild);
    }

    /// <summary>
    /// 返回指定坐标的建筑
    /// </summary>
    /// <param name="cellPos">tile 坐标</param>
    /// <returns>建筑物</returns>
    public BaseBuild GetBuild(Vector3Int cellPos)
    {
        if (m_BuildList == null)
        {
            return null;
        }

        return m_BuildList.TryGetValue(cellPos, out var build) ? build : null;
    }


    /// <summary>
    /// 添加游戏对象进入字典中保存，方便下次批量调用
    /// </summary>
    /// <param name="build">建筑物</param>
    public void AddBuild(GameObject build)
    {
        if (m_Builds == null)
        {
            m_Builds = new Dictionary<Type, List<BaseBuild>>();
        }

        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        if (!m_Builds.ContainsKey(baseBuild.GetType()))
        {
            m_Builds.Add(baseBuild.GetType(), new List<BaseBuild>());
        }

        m_Builds[baseBuild.GetType()].Add(baseBuild);
    }

    /// <summary>
    /// 在字典中删除指定游戏对象
    /// </summary>
    /// <param name="build">建筑物</param>
    public void RemoveBuild(GameObject build)
    {
        if (m_Builds == null)
        {
            return;
        }

        BaseBuild baseBuild = build.GetComponent<BaseBuild>();
        if (m_Builds.ContainsKey(baseBuild.GetType()))
        {
            m_Builds[baseBuild.GetType()].Remove(baseBuild);
        }
    }
}