using System;
using DB;
using UnityEngine;

/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseBuild
{
    // 攻击力
    private int attack;
    private void Awake()
    {
        var buildData = GameManager.Instance.GetBuildData(nameof(HomeBuild));
        if (buildData!= null)
        {
            distance = buildData.distance;
            level = buildData.level;
            isPlace = buildData.isPlace;
            buildType = buildData.buildType;
            hp = buildData.hp;
            upgradePrice = buildData.upgradePrice;
            price = buildData.price;
            attack = buildData.attack;
        }
    }


    public override bool CanPlace()
    {
        var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
        return homeBuilds.Count < 1;
    }
}