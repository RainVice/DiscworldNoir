using System;
using System.Linq;

public class WayBuild : BaseWay
{
    
    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            if (obstacle as BaseBuild)
            {
                var baseBuild = obstacle as BaseBuild;
                if (! (baseBuild as WallBuild))
                {
                    AddLine(baseBuild);
                }
            }
        };
    }
    
    public override bool IsHave(Resource resource)
    {
        return GetNum(resource) > 0;
    }
}