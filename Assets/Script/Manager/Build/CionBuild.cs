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
        
        foreach (var keyValuePair in inventory)
        {
            // todo 推送到路线
        }
        
        foreach (var obstaclesKey in obstacles.Keys)
        {
            if (obstaclesKey.IsSubclassOf(typeof(BaseWay)))
            {
                foreach (var baseObstacle in obstacles[obstaclesKey])
                {
                    
                }
            }
        }
        

        if (GetNum(typeof(CrystalTerrain)) > 0)
        {
            if (obstacles.ContainsKey(typeof(HomeBuild)))
            {
                var homeBuild = obstacles[typeof(HomeBuild)][0] as HomeBuild;
                if (homeBuild != null)
                    m_lineDic[homeBuild].Push(true, () => { homeBuild.AddNum(typeof(CrystalTerrain)); });
            }
        }
        else
        {
            // todo 推送到路线
        }
        
        // if (m_CrystalCount > 0)
        // {
        //     foreach (var wayBuild in m_wayBuilds)
        //     {
        //         m_lineDic[wayBuild].Push(true, () =>
        //         {
        //             wayBuild.AddCylinder();
        //             m_CrystalCount--;
        //         });
        //     }
        //     // 送到大本营
        //     if (m_homeBuild)
        //     {
        //         m_lineDic[m_homeBuild].Push(true, () =>
        //         {
        //             m_CrystalCount--;
        //             m_homeBuild.ChangeData();
        //         });
        //     }
        // }
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