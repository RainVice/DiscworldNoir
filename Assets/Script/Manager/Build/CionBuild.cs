using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CionBuild : BaseProduce
{
    // 大本营
    private HomeBuild m_homeBuild;

    // 水晶集合
    private List<CrystalTerrain> m_crystalTerrains = new();

    // 线路集合
    private List<WayBuild> m_wayBuilds = new();

    // 水晶库存
    private int m_CrystalCount;

    // 生产计时
    private float m_ProductionTime;

    // 运输计时
    private float m_WayTime;
    
    protected override void OnProduce()
    {
        base.OnProduce();
        foreach (var baseObstacle in obstacles.Keys.Where(
                     obstaclesKey => obstaclesKey.IsSubclassOf(
                         typeof(BaseTerrain)
                         )
                     ).SelectMany(
                     obstaclesKey => obstacles[obstaclesKey]
                     )
                 )
        {
            m_lineDic[baseObstacle].Push(false, () =>
            {
                AddNum(baseObstacle.GetType());
            });
        }
    }
    protected override void OnWay()
    {
        base.OnWay();
        
        if (GetNum(typeof(CrystalTerrain)) > 0)
        {
            if (obstacles.ContainsKey(typeof(HomeBuild)))
            {
                foreach (var homeBuild in obstacles[typeof(HomeBuild)].OfType<HomeBuild>())
                {
                    m_lineDic[homeBuild].Push(true, () => { homeBuild.AddNum(typeof(CrystalTerrain)); });
                }
            }
            else if (obstacles.ContainsKey(typeof(WayBuild)))
            {
                foreach (var wayBuild in obstacles[typeof(WayBuild)].OfType<WayBuild>())
                {
                    m_lineDic[wayBuild].Push(true, () => { wayBuild.AddNum(typeof(CrystalTerrain)); });
                }
            }
        }
    }

    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case CrystalTerrain :
                case HomeBuild :
                case WayBuild :
                    AddLine(obstacle);
                    break;
            }
        };
    }

    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(CrystalTerrain));
    }
}