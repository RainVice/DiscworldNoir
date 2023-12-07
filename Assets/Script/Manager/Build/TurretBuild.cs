using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 炮台建筑控制类
/// </summary>
public class TurretBuild : BaseAttack
{
    public override Resource IsNeed()
    {
        return Resource.Bullet;
    }

    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(HomeBuild)) || obstacles.ContainsKey(typeof(BulletBuild)) || obstacles.ContainsKey(typeof(WayBuild));
    }

    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case HomeBuild:
                case BulletBuild:
                case WayBuild:
                    AddLine(obstacle);
                    break;
            }
        };
    }
    
    
    // 向外询问一下是否有物资
    protected override void OnWay()
    {
        base.OnWay();
        foreach (Resource value in Enum.GetValues(typeof(Resource)))
        {
            if (value is Resource.All or Resource.None) continue;
            if (IsNeed().HasFlag(value))
            {
                var findAllWay = GameManager.Instance.FindAllWay(CurPos, value, out List<BaseBuild> baseBuilds);
                for (var i = 0; i < findAllWay.Count; i++)
                {
                    var vector3Ints = findAllWay[i];
                    var baseBuild = baseBuilds[i];
                    GameManager.Instance.Send(vector3Ints,CurPos, () =>
                    {
                        baseBuild.ChangeNum(value, -1);
                        ChangeNum(value);
                    });
                }
            }
        }
    }
    
    
    
}