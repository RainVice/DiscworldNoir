using System;
using System.Collections.Generic;
using System.Linq;
using DB;
using Effect;
using UnityEditor.Scripting;
using UnityEngine;

/// <summary>
/// 整个游戏的管理器主要用于数据交换
/// </summary>
public class GameManager : MonoBehaviour
{

    #region 属性
    // ************** 属性 *****************
    // 单例
    public static GameManager Instance { get; private set; }
    // 当前选择的一个物体
    public GameObject CurSelectedObject { get; set; }
    #endregion
    
    #region 引用
    // *************** 引用 *****************
    public Grid m_Grid;
    public GameObject LinePrefab;
    public GameObject CirclePrefab;
    #endregion
    
    #region 变量
    
    // *************** 变量 *****************
    // 建筑集合分类
    private Dictionary<Type, List<BaseBuild>> m_Builds;

    // 建筑集合
    private Dictionary<Vector3Int, BaseBuild> m_BuildList;

    // 地形集合
    private Dictionary<Vector3Int, BaseTerrain> m_TerrainList;
    
    // 建筑数据
    private Dictionary<string, BuildData> m_BuildData;
    #endregion

    #region 邻接矩阵
    // ************************ 邻接矩阵 ********************
    
    // 邻接表
    private Vector3Int[] from = new Vector3Int[2000];
    // 邻接矩阵
    private Line[,] table = new Line[2000,2000];
    // 邻接表长度
    private int length = 0;
    // 度的数量
    private int degree = 0;
    
    // 注册邻接表
    public void RegisterFrom(Vector3Int v3i)
    {
        if (!from.Contains(v3i)) from[length++] = v3i;
    }
    
    // 注册邻接矩阵
    public void RegisterTable(Vector3Int sv3i, Vector3Int ev3i,Line line)
    {
        if (!from.Contains(sv3i)) from[length++] = sv3i;
        var start = Array.IndexOf(from,sv3i);
        var end = Array.IndexOf(from,ev3i);
        if (table == null) return;
        table[start, end] = line;
        table[end, start] = line;
        degree++;
    }
    
    // 删除节点
    public void RemoveNode(Vector3Int v3i)
    {
        if (!from.Contains(v3i)) return;
        var node = Array.IndexOf(from, v3i);
        for (var i = 0; i < length; i++)
        {
            if (i == node) continue;
            Destroy(table[node, i]);
            Destroy(table[i,node]);
            table[node, i] = null;
            table[i, node] = null;
            degree--;
        }
        length--;
    }
    
    // 遍历矩阵
    public List<Vector3Int> FindOneWay(Vector3Int v3i, Resource resource, out BaseBuild baseBuild)
    {
        // 查找过的数据
        var find = new List<Vector3Int>();
        var vector3Ints = StartFindOneWay(Array.IndexOf(from, v3i), resource,find);
        if (vector3Ints == null)
        {
            baseBuild = null;
            return null;
        }
        baseBuild = GetBuild(vector3Ints[0]);
        return vector3Ints;
    }
    
    // 开始遍历
    private List<Vector3Int> StartFindOneWay(int index, Resource resource,List<Vector3Int> find)
    {
        if (find.Contains(from[index])) return null;
        for (var i = 0; i < length; i++)
        {
            find.Add(from[index]);
            if (i == index) continue;
            if (table[index, i] == null) continue;
            if (m_BuildList.TryGetValue(from[i], out var build))
            {
                var isHave = build.IsHave(resource);
                if (isHave)
                {
                    var vector3Ints = new List<Vector3Int> { from[i] };
                    return vector3Ints;
                }
                else
                {
                    if (GetBuild(from[i]) is not WayBuild) continue;
                    var vector3Ints = StartFindOneWay(i, resource, find);
                    if (vector3Ints == null) continue;
                    vector3Ints.Add(from[i]);
                    return vector3Ints;
                }
            }
        }
        return null;
    }
    
    // 查找所有线路
    public List<List<Vector3Int>> FindAllWay(Vector3Int v3i, Resource resource, out List<BaseBuild> baseBuilds)
    {
        var allPaths = new List<List<Vector3Int>>();
        var visited = new List<Vector3Int>();
        StartFindAllWay(Array.IndexOf(from, v3i), resource, visited, allPaths);
        baseBuilds = allPaths.Select(vector3Ints => GetBuild(vector3Ints[0])).ToList();
        return allPaths;
    }

    // 开始遍历所有线路
    private List<Vector3Int> StartFindAllWay(int index, Resource resource, List<Vector3Int> visited, List<List<Vector3Int>> allPaths)
    {
        for (int i = 0; i < length; i++)
        {
            // todo 查找所有的线路
        }
        return null;
    }
    
    // *****************************************************

    #endregion
    
    private void Awake()
    {
        Instance = this;
        // 获取建筑数据
        InitBuilds();
    }


    /// <summary>
    /// 从数据库中读取建筑数据
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
    
    #region 建筑物
    
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
        m_BuildList ??= new Dictionary<Vector3Int, BaseBuild>();
        var baseBuild = build.GetComponent<BaseBuild>();
        // 初始化建筑被放置时的一些属性
        baseBuild.CurPos = cellPos;
        baseBuild.IsPlace = true;
        // 注册邻接表
        RegisterFrom(baseBuild.CurPos);
        // 邻接矩阵中注册
        var dictionary = baseBuild.GetLines();
        foreach (var keyValuePair in dictionary)
        {
            RegisterTable(baseBuild.CurPos,keyValuePair.Key.CurPos,keyValuePair.Value);
        }
        m_BuildList.Add(cellPos, baseBuild);
    }
    
    /// <summary>
    /// 添加游戏对象进入字典中保存，方便下次批量调用
    /// </summary>
    /// <param name="build">建筑物</param>
    public void AddBuild(GameObject build)
    {
        m_Builds ??= new Dictionary<Type, List<BaseBuild>>();
        var baseBuild = build.GetComponent<BaseBuild>();
        if (!m_Builds.ContainsKey(baseBuild.GetType()))
        {
            m_Builds.Add(baseBuild.GetType(), new List<BaseBuild>());
        }
        m_Builds[baseBuild.GetType()].Add(baseBuild);
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
    
    #endregion
    
    #region 功能
    
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
    
    /// <summary>
    /// Vector3Int 转 Vector3
    /// </summary>
    /// <param name="vector3Int"></param>
    /// <returns></returns>
    public Vector3 V3IToV3(Vector3Int vector3Int)
    {
        return m_Grid.CellToWorld(vector3Int);
    }
    
    
    /// <summary>
    /// 发送物品
    /// </summary>
    /// <param name="cellPos"> tile 坐标</param>
    /// <param name="onFinish"> 完成事件</param>
    public void Send(List<Vector3Int> cellPos,Vector3Int end,Action onFinish)
    {
        cellPos.Add(end);
        var instantiate = Instantiate(CirclePrefab,cellPos[0],Quaternion.identity);
        var vector3S = cellPos.Select(vector3Int => m_Grid.CellToWorld(vector3Int)).ToList();
        this.CreateEffect().AddEffect(instantiate.transform.SlideTFsTo(vector3S)).Play(() =>
        {
            Destroy(instantiate);
            onFinish.Invoke();
        });
    }
    
    /// <summary>
    /// LayerPosition 转 Vector3Int
    /// </summary>
    /// <param name="lp"></param>
    /// <returns></returns>
    public Vector3Int LpToVector3Int(LayerPosition lp)
    {
        return m_Grid.WorldToCell(lp.GetVector3());
    }
    
    #endregion
    
    
    
}