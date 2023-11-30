using DB;

public class Home : BaseData
{

    public Home()
    {
        
    }
    public Home(int distance, float hp, int level, bool isPlace, BuildType buildType, int upgradePrice, int price)
    {
        this.distance = distance;
        this.hp = hp;
        this.level = level;
        this.isPlace = isPlace;
        this.buildType = buildType;
        this.upgradePrice = upgradePrice;
        this.price = price;
    }

    // 距离
    public int distance;
    // 血量
    public double hp;
    // 等级
    public int level;
    // 是否放置
    public bool isPlace = false;
    // 类型
    public BuildType buildType;
    // 升级价格
    public int upgradePrice;
    // 价格
    public int price;

    public override string ToString()
    {
        return $"距离:{distance} 血量:{hp} 等级:{level} 是否放置:{isPlace} 类型:{buildType} 升级价格:{upgradePrice} 价格:{price}";
    }
}