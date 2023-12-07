using System;

public class HarvesterBuild : BaseMaker
{

    public override Resource IsNeed()
    {
        return Resource.Tree;
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Wood) && GetNum(Resource.Wood) > 0;
    }


    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case HomeBuild:
                case AxeBuild:
                case BulletBuild:
                case WayBuild:
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
            if (GetNum(Resource.Tree) > 1)
            {
                ChangeNum(Resource.Tree, -1);
                ChangeNum(Resource.Wood,3);
            }
        };
    }

    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(AxeBuild)) || obstacles.ContainsKey(typeof(WayBuild));
    }
}