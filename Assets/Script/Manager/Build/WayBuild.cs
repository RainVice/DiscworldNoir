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
    
    protected override void OnWay()
    {
        base.OnWay();

        foreach (var baseObstacle in obstacles.Values.SelectMany(obstaclesValue => obstaclesValue))
        {
            var isNeed = baseObstacle.IsNeed();
            if (isNeed != Resource.None) continue;
            // 查询自己是否有对应资源
            foreach (Resource value in Enum.GetValues(typeof(Resource)))
            {
                
                if (value is Resource.None or Resource.All) continue;
                // 如果当前已有传输路径
                if (ways.ContainsKey(value)) continue;
                
                if (isNeed.HasFlag(value) && GetNum(value) > 0)
                {
                    
                }
            }
        }
    }


    public override bool IsHave(Resource resource)
    {
        return GetNum(resource) > 0;
    }
}