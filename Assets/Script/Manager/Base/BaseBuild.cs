using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using UnityEditor.Build;
using UnityEngine;

/// <summary>
/// 所有建筑的基类
/// </summary>
public abstract class BaseBuild : BaseObstacle
{
    // **************** 属性 *****************
    public ObstacleType ObstacleType => m_obstacleType;
    public Vector3Int CurPos
    {
        get => m_curPos;
        set => m_curPos = value;
    }

    public bool IsPlace
    {
        get => isPlace;
        set => isPlace = value;
    }

    // ***************** 变量 ******************
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

    // 数据
    protected BuildData m_buildData;

    // 当前位置
    protected Vector3Int m_curPos;

    // 未放置状态的上一步位置
    protected Vector3 m_lastPos;

    // 保存线段的集合
    protected List<Line> m_lines = new();


    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Build;
        m_buildData = GameManager.Instance.GetBuildData(GetType().Name);
        if (m_buildData == null) return;
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
        DestroyLines();
    }

    protected void Update()
    {
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
        OnScan();
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

    /// <summary>
    /// 扫描周围的格子
    /// </summary>
    protected virtual void OnScan()
    {
        // 销毁上一次资源
        DestroyLines();
    }

    /// <summary>
    /// 添加线
    /// </summary>
    /// <param name="end"></param>
    protected void AddLine(BaseObstacle end)
    {
        m_lines.Add(Line.DrawLine(transform.position, end.transform.position));
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
    /// 销毁所有线段
    /// </summary>
    protected void DestroyLines()
    {
        foreach (var line in m_lines)
        {
            Destroy(line.gameObject);
        }

        m_lines.Clear();
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