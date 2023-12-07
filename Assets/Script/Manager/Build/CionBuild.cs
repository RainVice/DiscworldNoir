﻿using System;
using System.Linq;

public class CionBuild : BaseProduce
{
    protected override void OnProduce()
    {
        base.OnProduce();
        foreach (var obstaclesValue in obstacles.Values)
        {
            foreach (var baseObstacle in obstaclesValue)
            {
                if (baseObstacle is not BaseTerrain baseTerrain) continue;
                var isOut = baseTerrain.IsOut();
                if (isOut.HasFlag(Resource.Crystal))
                {
                    GetLine(baseTerrain).Push(false, () =>
                    {
                        ChangeNum(isOut);
                    });
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
                case BaseWay :
                    AddLine(obstacle);
                    break;
            }
        };
    }
    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(CrystalTerrain));
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Crystal) && GetNum(Resource.Crystal) > 0;
    }
}