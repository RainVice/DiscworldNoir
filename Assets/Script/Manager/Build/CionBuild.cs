using System;
using System.Collections.Generic;

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
        foreach (var obstaclesValue in obstacles.Values)
        {
            foreach (var baseObstacle in obstaclesValue)
            {
                var isOut = baseObstacle.IsOut();
                if (isOut != Resource.None)
                {
                    m_lineDic[baseObstacle].Push(false, () =>
                    {
                        AddNum(isOut);
                    });
                }
            }
        }
    }
    protected override void OnWay()
    {
        base.OnWay();
        foreach (var obstaclesValue in obstacles.Values)
        {
            foreach (var baseObstacle in obstaclesValue)
            {
                var baseBuild = baseObstacle as BaseBuild;
                if (baseBuild == null) continue;
                if (baseBuild.IsNeed() == Resource.None) continue;
                if (GetNum(baseBuild.IsNeed()) > 0)
                {
                    m_lineDic[baseBuild].Push(true, () => { baseBuild.AddNum(baseBuild.IsNeed()); });
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