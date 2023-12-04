using System;
using System.Collections.Generic;
using System.Linq;
using DB;
using UnityEditor.Scripting;
using UnityEngine;

/// <summary>
/// 整个游戏的管理器主要用于数据交换
/// </summary>
public class GameManager : MonoBehaviour
{

    // ************** 属性 *****************
    // 单例
    public static GameManager Instance { get; private set; }
    // 当前选择的一个物体
    public GameObject CurSelectedObject { get; set; }
    
    // *************** 引用 *****************
    public Grid m_Grid;
    public GameObject LinePrefab;
    
    // *************** 变量 *****************
    // 建筑集合分类
    private Dictionary<Type, List<BaseBuild>> m_Builds;

    // 建筑集合
    private Dictionary<Vector3Int, BaseBuild> m_BuildList;

    // 地形集合
    private Dictionary<Vector3Int, BaseTerrain> m_TerrainList;
    
    // 建筑数据
    private Dictionary<string, BuildData> m_BuildData;

    private void Awake()
    {
        Instance = this;
        // 获取建筑数据
        InitBuilds();
    }

    /// <summary>
    /// 获取建筑数据
    /// </summary>
    private void InitBuilds()
    {
        m_BuildData = new Dictionary<string, BuildData>();
        var buildDatas = new QueryWapper<BuildData>().Do();
        buildDatas.ForEach(buildData =>
        {
            m_BuildData.Add(buildData.buildName, buildData);
        });
    }
    
    /// <summary>
    /// 获取建筑数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public BuildData GetBuildData(string name)
    {
        if (m_BuildData == null) return null;
        return m_BuildData.TryGetValue(name, out var data) ? data : null;
    }
    
    /// <summary>
    /// 模糊查询建筑数据
    /// </summary>
    /// <param name="name">查询的名称</param>
    /// <returns></returns>
    public BuildData GetBuildDataByFuzzy(string name)
    {
        return (from key in m_BuildData.Keys where key.Contains(name) select m_BuildData[key]).FirstOrDefault();
    }
    
    /// <summary>
    /// 是否存在地形
    /// </summary>
    /// <param name="cellPos">地形坐标</param>
    /// <returns></returns>
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
        m_TerrainList ??= new Dictionary<Vector3Int, BaseTerrain>();
        var baseTerrain = terrain.GetComponent<BaseTerrain>();
        m_TerrainList.Add(cellPos, baseTerrain);
    }
    
    /// <summary>
    /// 获取地形
    /// </summary>
    /// <param name="cellPos">地形位置</param>
    /// <typeparam name="T">地形类型</typeparam>
    /// <returns>返回地形</returns>
    public T GetTerrain<T>(Vector3Int cellPos) where T : BaseTerrain
    {
        m_TerrainList ??= new Dictionary<Vector3Int, BaseTerrain>();
        if (m_TerrainList.TryGetValue(cellPos, out var value))
        {
            return value as T;
        }
        return null;
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
        var baseBuild = build.GetComponent<BaseBuild>();
        baseBuild.CurPos = cellPos;
        baseBuild.IsPlace = true;
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

        var baseBuild = build.GetComponent<BaseBuild>();
        if (!m_Builds.ContainsKey(baseBuild.GetType()))
        {
            m_Builds.Add(baseBuild.GetType(), new List<BaseBuild>());
        }
        m_Builds[baseBuild.GetType()].Add(baseBuild);
    }
    
    /// <summary>
    /// 获取指定类型的所有建筑
    /// </summary>
    /// <typeparam name="T"> 建筑类型</typeparam>
    /// <returns> 建筑集合</returns>
    public List<BaseBuild> GetBuilds(Type type)
    {
        m_Builds??= new Dictionary<Type, List<BaseBuild>>();
        if (!m_Builds.ContainsKey(type))
        {
            return new List<BaseBuild>();
        }
        var baseBuilds = m_Builds[type];
        var builds = baseBuilds;
        return builds;
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
    
    
    /// <summary>
    /// 获取障碍
    /// </summary>
    /// <param name="vector3"></param>
    /// <returns></returns>
    public BaseObstacle GetObstacle(Vector3 vector3)
    {
        var baseBuild = GetBuild(m_Grid.LocalToCell(vector3));
        if (baseBuild != null) return baseBuild;
        var baseTerrain = GetTerrain<BaseTerrain>(m_Grid.LocalToCell(vector3));
        return baseTerrain != null ? baseTerrain : null;
    }
    
    /// <summary>
    /// 扫描附近物品
    /// </summary>
    /// <param name="layerPosition"></param>
    /// <param name="curVector3"></param>
    /// <param name="action"></param>
    public void Scan(LayerPosition layerPosition,Vector3 curVector3,Action<BaseObstacle> action)
    {
        layerPosition.Scan(curVector3, v3 =>
        {
            var baseObstacle = GetObstacle(v3);
            if (baseObstacle != null)
            {
                action.Invoke(baseObstacle);
            }
        });
    }
    
    
    
}