using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            // 判断是否是建筑物
            if (baseObstacle is not BaseBuild baseBuild) continue;
            // 询问是否需要物质，需要物资则查找物质，没有物质就继续向下一级访问
            var isNeed = baseBuild.IsNeed();
            if (isNeed != Resource.None)
            {     
                foreach (Resource value in Enum.GetValues(typeof(Resource)))
                {
                    if (value is Resource.None or Resource.All) continue;
                    if (isNeed.HasFlag(value) && GetNum(value) > 0)
                    {
                        isNeed ^= value;
                        needResource ^= value;
                        GetLine(baseBuild).Push(true, () =>
                        {
                            baseBuild.ChangeNum(value);
                            ChangeNum(value,-1);
                        });
                        
                    }
                    else if (isNeed.HasFlag(value))
                    {
                        SendNotice(value);
                    }
                }
                needResource |= isNeed;   
            }
            
            // 判断是否需要传出物质,如果没有传出物质则继续向下一级访问
            var isOut = baseBuild.IsOut();
            if (isOut == Resource.None) continue;
            foreach (Resource value in Enum.GetValues(typeof(Resource)))
            {
                Debug.Log(needResource);
                if (value is Resource.All or Resource.None) continue;
                if (!isOut.HasFlag(value)) continue;
                if (!needResource.HasFlag(value)) continue;
                if (baseBuild.GetNum(value) <= 0) continue;
                GetLine(baseBuild).Push(false, () =>
                {
                    baseBuild.ChangeNum(value,-1);
                    ChangeNum(value);
                });
            }
        }
    }

    public override Resource IsNeed()
    {
        return needResource;
    }
    
    
}