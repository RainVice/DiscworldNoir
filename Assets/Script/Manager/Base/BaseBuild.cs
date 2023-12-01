
using System;
using UnityEngine;

/// <summary>
/// 所有建筑的基类
/// </summary>
public abstract class BaseBuild : BaseObstacle
{
    // 属性
    public Vector3Int CurPos
    {
        get => m_curPos;
        set => m_curPos = value;
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
    private Vector3Int m_curPos;
    
    public ObstacleType ObstacleType => m_obstacleType;


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

    // 是否可以放置
    public virtual bool CanPlace()
    {
        return true;
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