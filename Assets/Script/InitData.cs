using DB;
using UnityEngine;

public class InitData : MonoBehaviour
{
    
    private void Awake()
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
                price = 10,
                attack = 10,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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
                hp = 50,
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