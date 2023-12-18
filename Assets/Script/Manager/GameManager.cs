using System;
using System.Collections.Generic;
using System.Linq;
using DB;
using Effect;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

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
    
    public int CrystalNum {
        get => m_CrystalNum;
        set
        {
            m_CrystalNum = value;
            UIManager.Instance.SetCrystal(m_CrystalNum);
        }
    }
    
    public bool IsStart { get => m_IsStart; set => m_IsStart = value; }
    
    public bool IsNight { get => m_IsNight; set => m_IsNight = value; }
    
    public GameObject Home => m_Home;
    
    public int Day => m_Day;
    #endregion
    
    #region 引用
    // *************** 引用 *****************
    public Grid m_Grid;
    public GameObject LinePrefab;
    public GameObject CirclePrefab;
    // 敌人
    public GameObject EnemyPrefab;
    #endregion
    
    #region 变量
    
    // *************** 变量 *****************
    // 建筑集合分类
    private Dictionary<Type, List<BaseBuild>> m_Builds = new();

    // 建筑集合
    private Dictionary<Vector3Int, BaseBuild> m_BuildList = new();

    // 地形集合
    private Dictionary<Vector3Int, BaseTerrain> m_TerrainList = new();
    
    // 建筑数据
    private Dictionary<string, BuildData> m_BuildData = new();
    
    // 水晶数量
    private int m_CrystalNum = 30;
    
    // 游戏是否开始
    private bool m_IsStart = false;
    
    // 是否黑夜
    private bool m_IsNight = false;
    
    // 黑夜计时器
    private float m_Timer = 0;
    
    // 敌人生成计时器
    private float m_EnemyTimer = 5f;
    
    // 度过的天数
    private int m_Day = 0;
    
    
    //大本营
    private GameObject m_Home;
    // 最外层距离
    private float m_OutDistance = 0;
    
    #endregion

    #region 邻接矩阵
    // ************************ 邻接矩阵 ********************
    
    // 邻接表
    private List<Vector3Int> from = new();
    // 邻接矩阵
    private Line[,] table = new Line[5000,5000];
    // 邻接表长度
    private int length = 0;
    // 度的数量
    // private int degree = 0;
    
    // 注册邻接表
    public void RegisterFrom(Vector3Int v3i)
    {
        if (from.Contains(v3i)) return;
        from.Add(v3i);
        length++;
    }
    
    // 注册邻接矩阵
    public void RegisterTable(Vector3Int sv3i, Vector3Int ev3i,Line line)
    {
        RegisterFrom(sv3i);
        var start = from.IndexOf(sv3i);
        var end = from.IndexOf(ev3i);
        if (table is null) return;
        table[start, end] = line;
        table[end, start] = line;
    }
    
    // 删除节点
    public void RemoveNode(BaseBuild baseBuild)
    {
        var v3i = baseBuild.CurPos;

        if (m_Builds.ContainsKey(baseBuild.GetType()))
        {
            m_Builds[baseBuild.GetType()].Remove(baseBuild);
        }
        if (m_BuildList.ContainsKey(v3i))
        {
            m_BuildList.Remove(v3i);
        }
        
        if (!from.Contains(v3i)) return;
        var node = from.IndexOf(v3i);
        for (var i = 0; i < length; i++)
        {
            if (i == node) continue;
            if (table[node,i] is not null)
            {
                if (!table[node, i].IsDestroyed()) Destroy(table[node, i].gameObject);
                table[node, i] = null;
            }
            if (table[i, node] is not null)
            {
                if (!table[i, node].IsDestroyed()) Destroy(table[i, node].gameObject);
                table[i, node] = null;
            }
        }
        length--;
    }
    
    // 遍历矩阵
    public List<Vector3Int> FindOneWay(Vector3Int v3i, Resource resource, out BaseBuild baseBuild)
    {
        // 查找过的数据
        var find = new HashSet<Vector3Int>();
        var vector3Ints = StartFindOneWay(from.IndexOf(v3i), resource,find);
        if (vector3Ints is null)
        {
            baseBuild = null;
            return null;
        }
        baseBuild = GetBuild(vector3Ints[0]);
        return vector3Ints;
    }
    
    // 开始遍历
    private List<Vector3Int> StartFindOneWay(int index, Resource resource,HashSet<Vector3Int> find)
    {
        if (find.Contains(from[index])) return null;
        for (var i = 0; i < length; i++)
        {
            if (i == index) continue;
            if (table[index, i] is null) continue;
            find.Add(from[index]);
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
                    if (vector3Ints is null) continue;
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
        StartFindAllWay(from.IndexOf(v3i), resource, visited, allPaths);
        baseBuilds = allPaths.Select(vector3Ints => GetBuild(vector3Ints[0])).ToList();
        return allPaths;
    }

    // 开始遍历所有线路
    private void StartFindAllWay(int index, Resource resource, List<Vector3Int> visited, List<List<Vector3Int>> allPaths)
    {
        
        if (visited.Contains(from[index])) return;
        
        for (int i = 0; i < length; i++)
        {
            if (i == index) continue;
            if (table[index, i] is null || table[index,i].IsDestroyed()) continue;
            if (visited.Contains(from[i])) continue;
            if (m_BuildList.TryGetValue(from[i], out var build))
            {
                visited.Add(from[index]);
                var isHave = build.IsHave(resource);
                if (isHave)
                {
                    visited.Add(from[i]);
                    var vector3Ints = new List<Vector3Int>(visited);
                    visited.Remove(from[i]);
                    vector3Ints.Reverse();
                    var flag = true;
                    for (var j = 0; j < allPaths.Count; j++)
                    {
                        var allPath = allPaths[j];
                        if (allPath[0] != vector3Ints[0]) continue;
                        flag = false;
                        if (allPaths.Count > vector3Ints.Count)
                        {
                            allPaths[j] = vector3Ints;
                        }
                    }
                    if (!flag) continue;
                    allPaths.Add(vector3Ints);
                }
                else
                {
                    if (GetBuild(from[i]) is WayBuild)
                    {
                        StartFindAllWay(i,resource,visited,allPaths);
                    }
                }
                visited.Remove(from[index]);
            }
        }
    }
    
    // *****************************************************

    #endregion
    
    #region 生命周期
    private void Awake()
    {
        // 加载开始界面
        SceneManager.LoadSceneAsync("Start", LoadSceneMode.Additive);
        Instance = this;
        // 获取建筑数据
        InitBuilds();
    }

    private void FixedUpdate()
    {
        // 切换时间
        ChangeTime();
        // 生成怪物
        CreateEnemy();
    }

    #endregion


    /// <summary>
    /// 从数据库中读取建筑数据
    /// </summary>
    private void InitBuilds()
    {
        
        // 判断是否第一次启动
        var startDatas = new QueryWapper<StartData>().Do();
        if (startDatas.Count == 0)
        {
            // 第一次启动
            var startData = new StartData(true);
            var isInsert = new InsertWapper<StartData>(startData).Do();
            if (isInsert)
            { 
                var insertWapper = new InsertWapper<BuildData>();
                var @do = insertWapper.Do(new []
                {
                    new BuildData
                    {
                        buildName = nameof(HomeBuild),
                        name = "大本营",
                        distance = 4,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Attack,
                        hp = 100,
                        upgradePrice = 10,
                        price = 0,
                        attack = 15,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "只能放置一个，当大本营被摧毁，游戏就结束了"
                    },
                    new BuildData
                    {
                        buildName = nameof(AxeBuild),
                        name = "伐木机",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "用于伐木，获取必要的木材资源"
                    },
                    new BuildData
                    {
                        buildName = nameof(TurretBuild),
                        name = "加农炮",
                        distance = 3,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Attack,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 10,
                        productionSpeed = 0,
                        waySpeed = 1,
                        description = "用于攻击敌人，需要弹药工厂为其制造弹药"
                    },
                    new BuildData
                    {
                        buildName = nameof(BulletBuild),
                        name = "弹药工厂",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "用于生产弹药，需要提供铁锭，同时需要少许木材"
                    },
                    new BuildData
                    {
                        buildName = nameof(MiningBuild),
                        name = "采矿机（铁）",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "用于采集铁矿，传送到铁矿加工厂生产铁锭"
                    },
                    new BuildData
                    {
                        buildName = nameof(FactoryBuild),
                        name = "加工厂（铁）",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "从采矿机（铁）获取铁矿，生产铁锭"
                    },
                    new BuildData
                    {
                        buildName = nameof(HarvesterBuild),
                        name = "加工厂（木材）",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "从伐木机获取木材，加工成需要的木材"
                    },
                    new BuildData
                    {
                        buildName = nameof(CionBuild),
                        name = "水晶采集器",
                        distance = 2,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 1,
                        waySpeed = 1,
                        description = "采集水晶，水晶是游戏的通用货币"
                    },
                    new BuildData
                    {
                        buildName = nameof(WallBuild),
                        name = "城墙",
                        distance = 0,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Production,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 0,
                        waySpeed = 0,
                        description = "用于防御外部入侵"
                    },
                    new BuildData
                    {
                        buildName = nameof(WayBuild),
                        name = "运输线路",
                        distance = 4,
                        level = 1,
                        isPlace = false,
                        buildType = BuildType.Way,
                        hp = 30,
                        upgradePrice = 10,
                        price = 10,
                        attack = 0,
                        productionSpeed = 0,
                        waySpeed = 1,
                        description = "用于运输线路上的资源"
                    },
                });
            }
        }
        m_BuildData = new Dictionary<string, BuildData>();
        var buildDatas = new QueryWapper<BuildData>().Do();
        buildDatas.ForEach(buildData => m_BuildData.Add(buildData.buildName, buildData));
    }
    
    #region 建筑物
    
    /// <summary>
    /// 获取建筑数据
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public BuildData GetBuildData(string name)
    {
        if (m_BuildData is null) return null;
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
        if (m_TerrainList is null)
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
        // 保存最外层最大距离
        var offset = m_Grid.CellToWorld(cellPos) - m_Home.transform.position;
        if (offset.magnitude > m_OutDistance)
        {
            m_OutDistance = offset.magnitude;
        }
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
        // 扣除水晶
        CrystalNum -= m_BuildData[baseBuild.GetType().Name].price;
    }
    
    /// <summary>
    /// 添加游戏对象进入字典中保存，方便下次批量调用
    /// </summary>
    /// <param name="build">建筑物</param>
    public void AddBuild(GameObject build)
    {
        if (build.GetComponent<BaseObstacle>() is HomeBuild)
        {
            m_Home = build;
        }
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
        if (m_BuildList is null)
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
        if (m_Builds is null)
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
        if (baseBuild is not null) return baseBuild;
        var baseTerrain = GetTerrain<BaseTerrain>(m_Grid.LocalToCell(vector3));
        return baseTerrain;
    }
    
    #endregion
    
    #region 功能
    
    // 生成怪物
    private void CreateEnemy()
    {

        if (Home is null || Home.IsDestroyed()) return;
        if (m_IsNight)
        {
            m_EnemyTimer += Time.fixedDeltaTime;
            if (m_EnemyTimer >= Constant.ENEMYCD / (1f + (0.05f * m_Day)))
            {
                m_EnemyTimer = 0;
                // 生成怪物
                float angle = Random.Range(0, 360);
                var radians = angle * Mathf.Deg2Rad;
                // 使用三角函数计算Vector2的x和y分量
                var x = Mathf.Cos(radians);
                var y = Mathf.Sin(radians);
                // 创建Vector2,此为生成怪物的坐标
                var vector = (new Vector3(x, y,0).normalized * (m_OutDistance + Random.Range(3f, 5f))) + m_Home.transform.position;
                // todo 生成怪物
                Instantiate(EnemyPrefab, vector, Quaternion.identity);
            }
        }
    }
    
    // 切换时间
    public void ChangeTime()
    {
        if (!m_IsStart)
        {
            return;
        }
        m_Timer += Time.fixedDeltaTime;
        UIManager.Instance.SetTime((int)m_Timer);
        if (m_Timer >= Constant.NightCD)
        {
            m_Timer = 0f;
            m_IsNight = !m_IsNight;
            if (!m_IsNight)
            {
                m_Day++;
                UIManager.Instance.SetDay(m_Day);
            }
        }
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
            if (baseObstacle is not null)
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