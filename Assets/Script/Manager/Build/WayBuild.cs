using System;
using System.Collections.Generic;

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
    
    
    public override Resource IsOuts()
    {
        return Resource.All;
    }
    
    
    
}