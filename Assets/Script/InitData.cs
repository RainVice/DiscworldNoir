using DB;
using UnityEngine;

public class InitData : MonoBehaviour
{
    
    private void Awake()
    {
        var insertWapper = new InsertWapper<BaseData>();
        insertWapper.Do(new []
        {
            new BuildData()
            {
                buildName = nameof(HomeBuild),
                name = "大本营",
                distance = 6,
                level = 1,
                isPlace = false,
                buildType = BuildType.Attack,
                hp = 100,
                upgradePrice = 10,
                price = 10,
                attack = 10,
                productionSpeed = 1,
                waySpeed = 1,
                description = ""
            }
        });
    }
    
}