
using UnityEngine;
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
    
}


public enum BuildType
{
    // 攻击
    Attack = 0,
    // 防御
    Defense = 1,
    // 生产
    Production = 2,
    // 运输
    Way = 3
    
}