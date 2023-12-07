using System;

public class FactoryBuild : BaseMaker
{
    public override Resource IsNeed()
    {
        return Resource.Iron;
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Ingot) && GetNum(Resource.Ingot) > 0;
    }

    
    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case HomeBuild :
                case MiningBuild :
                case BulletBuild :
                case WayBuild :
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
            if (GetNum(Resource.Iron) > 1)
            {
                ChangeNum(Resource.Iron, -1);
                ChangeNum(Resource.Ingot);
            }
        };
    }

    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(MiningBuild)) || obstacles.ContainsKey(typeof(WayBuild));
    }
}