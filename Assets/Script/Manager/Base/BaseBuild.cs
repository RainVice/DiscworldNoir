using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// 所有建筑的基类
/// </summary>
public abstract class BaseBuild : BaseObstacle
{
    // **************** 属性 *****************
    // 障碍类型
    public ObstacleType ObstacleType => m_obstacleType;
    // 当前座标
    public Vector3Int CurPos
    {
        get => m_curPos;
        set => m_curPos = value;
    }
    // 是否放置
    public bool IsPlace
    {
        get => isPlace;
        set => isPlace = value;
    }

    // ***************** 变量 ******************
    // 建筑中文名
    protected string name;
    // 范围
    protected int distance;
    // 等级
    protected int level;
    // 是否放置
    protected bool isPlace = false;
    // 类型
    protected BuildType buildType;
    // 生命
    protected int hp;
    // 升级价格
    protected int upgradePrice;
    // 价格
    protected int price;
    
    
    // 连接的障碍
    protected Dictionary<Type, List<BaseObstacle>> obstacles = new();
    // 库存物资
    protected Dictionary<Type, int> inventory = new();
    
    // 数据
    protected BuildData m_buildData;

    // 当前位置
    protected Vector3Int m_curPos;

    // 未放置状态的上一步位置
    protected Vector3 m_lastPos;

    // 保存线段的集合
    protected List<Line> m_lines = new();
    
    // 线条集合
    protected Dictionary<BaseObstacle,Line> m_lineDic = new();

    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Build;
        m_buildData = GameManager.Instance.GetBuildData(GetType().Name);
        if (m_buildData == null) return;
        name = m_buildData.name;
        distance = m_buildData.distance;
        level = m_buildData.level;
        isPlace = m_buildData.isPlace;
        buildType = m_buildData.buildType;
        hp = m_buildData.hp;
        upgradePrice = m_buildData.upgradePrice;
        price = m_buildData.price;
    }

    protected virtual void OnDestroy()
    {
        DestroyPre();
    }

    protected virtual void FixedUpdate()
    {
        // 调用移动事件
        OnMove();
    }
    /// <summary>
    /// 移动事件
    /// </summary>
    protected virtual void OnMove()
    {
        // 判断是否放置或者是否移动，如果是则不进行处理
        if (isPlace)
        {
            foreach (var mLine in m_lines)
            {
                mLine.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (m_lastPos == transform.position) return;
        m_lastPos = transform.position;
        // 扫描
        Scan();
        // 判断是否可以放置
        if (CanPlace())
        {
            var color = Color.white;
            color.a = 0.3f;
            GetComponent<SpriteRenderer>().color = color;
        }
        else
        {
            var color = Color.red;
            color.a = 0.3f;
            GetComponent<SpriteRenderer>().color = color;
        }

    }
    private void Scan()
    {        
        // 销毁上一次资源
        DestroyPre();
        OnScan(out var clean, out var action);
        clean?.Invoke();
        var layerPosition = new LayerPosition(distance);
        GameManager.Instance.Scan(layerPosition, transform.position, obstacle => action?.Invoke(obstacle));
    }

    /// <summary>
    /// 扫描周围的格子
    /// </summary>
    /// <param name="clean">扫描之前的清理</param>
    /// <param name="action">扫描后结果的处理</param>
    protected virtual void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = null;
    }
    
    /// <summary>
    /// 添加线
    /// </summary>
    /// <param name="end"></param>
    protected void AddLine(BaseObstacle end)
    {
        m_lineDic.Add(end,Line.DrawLine(transform.position, end.transform.position));
        if (!obstacles.ContainsKey(end.GetType()))
        {
            obstacles.Add(end.GetType(), new List<BaseObstacle>());
        }
        obstacles[end.GetType()].Add(end);
    }


    /// <summary>
    /// 获取线
    /// </summary>
    /// <param name="obs"></param>
    /// <returns></returns>
    public Line GetLine(BaseObstacle obs)
    {
        return m_lineDic[obs];
    }
    
    /// <summary>
    /// 销毁所有线段
    /// </summary>
    protected void DestroyPre()
    {
        foreach (var line in m_lineDic.Where(line => !line.Value.IsDestroyed()))
        {
            Destroy(line.Value.gameObject);
        }
        m_lineDic.Clear();
        obstacles.Clear();
    }


    /// <summary>
    /// 是否可以放置
    /// </summary>
    /// <returns></returns>
    public virtual bool CanPlace()
    {
        return true;
    }
    
    /// <summary>
    /// 添加物资数量
    /// </summary>
    /// <param name="num"> 默认数量是 1</param>
    public virtual void ChangeData(int num = 1){}

    /// <summary>
    /// 添加物资数量(指定类型)
    /// </summary>
    /// <param name="type"></param>
    /// <param name="num"></param>
    public virtual void ChangeData(Type type, int num = 1) { }

    /// <summary>
    /// 询问是否需要物质
    /// </summary>
    /// <returns></returns>
    public virtual Type IsNeed()
    {
        return null;
    }

    /// <summary>
    /// 增加资源
    /// </summary>
    /// <param name="type"></param>
    public void AddNum(Type type)
    {
        inventory.TryAdd(type, 0);
        inventory[type]++;
    }
    
    public int GetNum(Type type)
    {
        return inventory.TryGetValue(type, out var num) ? num : 0;
    }

    

}


[Flags]
public enum BuildType
{
    // 攻击
    Attack = 1,

    // 防御
    Defense = 2,

    // 生产
    Production = 4,

    // 运输
    Way = 8
}