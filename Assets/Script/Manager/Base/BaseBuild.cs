
using UnityEngine;

public abstract class BaseBuild : MonoBehaviour
{
    // ***************** 变量 ******************
    // 范围
    private int distance;
    // 等级
    private int level;
    // 是否放置
    private bool isPlace = false;
    // 类型
    private BuildType buildType;
    // 生命
    private int hp;
    // 升级价格
    private int upgradePrice;
    // 价格
    private int price;
    
}


public enum BuildType
{
    // 攻击
    Attack,
    // 防御
    Defense,
    // 生产
    Production
}