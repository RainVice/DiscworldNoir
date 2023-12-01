
using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 所有建筑的基类
/// </summary>
public abstract class BaseBuild : MonoBehaviour
{
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