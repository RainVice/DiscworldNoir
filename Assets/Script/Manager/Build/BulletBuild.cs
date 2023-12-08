using System;
using System.Collections.Generic;
using UnityEngine;

public class BulletBuild : BaseMaker
{
    public override Resource IsNeed()
    {
        return Resource.Ingot | Resource.Wood;
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Bullet) && GetNum(Resource.Bullet) > 0;
    }

    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case HomeBuild :
                case TurretBuild :
                case WayBuild :
                case HarvesterBuild :
                case FactoryBuild :
                    AddLine(obstacle);
                    break;
            }
        };
    }


    protected override Action OnMake(BaseBuild baseBuild, Resource resource)
    {
        return () =>
        {
            baseBuild.ChangeNum(resource, -1);
            ChangeNum(resource);
            if (GetNum(Resource.Ingot) > 1 && GetNum(Resource.Wood) > 1)
            {
                ChangeNum(Resource.Ingot, -1);
                ChangeNum(Resource.Wood, -1);
                ChangeNum(Resource.Bullet);
            }
        };
    }

    public override bool CanPlace()
    {
        return 
            (
                obstacles.ContainsKey(typeof(FactoryBuild)) 
                && obstacles.ContainsKey(typeof(HarvesterBuild))
                ) || 
               obstacles.ContainsKey(typeof(WayBuild));
    }
    
    
    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"制造速度：{productionSpeed} > {productionSpeed * Constant.Upgrade(level)} \n" +
            $"传输速度：{waySpeed} > {waySpeed * Constant.Upgrade(level)}\n" +
            $"当前血量：{hp}" );
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        maxHp *= (int)Constant.Upgrade(level);
        productionSpeed *= Constant.Upgrade(level);
        waySpeed *= Constant.Upgrade(level);
    }
    
    
}