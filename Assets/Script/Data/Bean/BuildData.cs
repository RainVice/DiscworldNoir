using DB;

public class BuildData : BaseData
{
    // 建筑名称
    public string buildName;
    // 建筑中文名
    public string name;
    // 范围
    public int distance;
    // 等级
    public int level;
    // 是否放置
    public bool isPlace = false;
    // 类型
    public BuildType buildType;
    // 生命
    public int hp;
    // 升级价格
    public int upgradePrice;
    // 价格
    public int price;
    // 攻击力
    public int attack;
    // 生产速度
    public int productionSpeed;
    // 运输速度
    public int waySpeed;
    // 描述
    public string description;
    
    public override string ToString()
    {
        return $"建筑名称：{name} \n范围：{distance} \n等级：{level} \n是否放置：{isPlace} \n类型：{buildType} \n生命：{hp} \n升级价格：{upgradePrice} \n价格：{price} \n攻击力：{attack} \n生产速度：{productionSpeed} \n运输速度：{waySpeed} \n描述：{description}";
    }
    
}