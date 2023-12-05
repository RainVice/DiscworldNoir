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
    protected string CName;
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
    // 运输速度
    protected int waySpeed;
    // 运输计时
    private float m_wayTimer;

    
    
    // 连接的障碍
    protected Dictionary<Type, List<BaseObstacle>> obstacles = new();
    // 库存物资
    protected Dictionary<Resource, int> inventory = new();
    // protected Dictionary<Type, int> inventory = new();
    
    // 数据
    protected BuildData m_buildData;

    // 当前位置
    protected Vector3Int m_curPos;

    // 未放置状态的上一步位置
    protected Vector3 m_lastPos;

    // 线条集合
    protected Dictionary<BaseObstacle,Line> m_lineDic = new();
    
    // 链接者
    protected List<BaseBuild> m_connecter = new();

    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Build;
        m_buildData = GameManager.Instance.GetBuildData(GetType().Name);
        if (m_buildData == null) return;
        CName = m_buildData.name;
        distance = m_buildData.distance;
        level = m_buildData.level;
        isPlace = m_buildData.isPlace;
        buildType = m_buildData.buildType;
        hp = m_buildData.hp;
        upgradePrice = m_buildData.upgradePrice;
        price = m_buildData.price;
        waySpeed = m_buildData.waySpeed;
    }

    protected virtual void OnDestroy()
    {
        foreach (var baseBuild in m_connecter)
        {
            baseBuild.DestroyLine(this);
        }

        DestroyPre();
    }

    protected virtual void FixedUpdate()
    {
        // 调用移动事件
        OnMove();
        // 判断是否可以调用运输事件
        if (waySpeed!= 0 && isPlace)
        {
            m_wayTimer += Time.fixedDeltaTime;
            if (m_wayTimer >= Constant.DEFAULTTIME / waySpeed)
            {
                m_wayTimer %= Constant.DEFAULTTIME / waySpeed;
                OnWay();
            }
        }
    }


    public void DestroyLine(BaseObstacle baseObstacle)
    {
        var line = m_lineDic[baseObstacle];
        m_lineDic.Remove(baseObstacle);
        Destroy(line);
    }

    /// <summary>
    /// 运输事件
    /// </summary>
    protected virtual void OnWay() { }

    
    /// <summary>
    /// 移动事件
    /// </summary>
    protected virtual void OnMove()
    {
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

        if (end is BaseBuild baseBuild)
        {
            m_connecter.Add(baseBuild);
        }
        
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
        m_connecter.Clear();
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
    /// 更改资源
    /// </summary>
    /// <param name="type"></param>
    public void ChangeNum(Resource resource,int num = 1)
    {
        inventory.TryAdd(resource, 0);
        inventory[resource] += num;
    }
    
    public int GetNum(Resource resource)
    {
        return inventory.TryGetValue(resource, out var num) ? num : 0;
    }
    
    
    
    
}