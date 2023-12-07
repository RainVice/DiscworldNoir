using System;
using System.Collections.Generic;

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
}