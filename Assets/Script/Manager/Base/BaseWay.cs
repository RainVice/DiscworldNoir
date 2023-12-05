using System;
using System.Collections.Generic;

public abstract class BaseWay : BaseBuild
{
    // protected Resource needResource;
    // protected override void Awake()
    // {
    //     base.Awake();
    //     buildType = BuildType.Way;
    // }
    //
    // protected virtual void Notice(Resource resource,Action<bool> action)
    // {
    //     foreach (Resource value in Enum.GetValues(typeof(Resource)))
    //     {
    //         if (value is Resource.None or Resource.All) continue;
    //         var hasFlag = resource.HasFlag(value);
    //         if (hasFlag && GetNum(value) > 0)
    //         {
    //             action.Invoke(true);
    //         }
    //         else if (hasFlag)
    //         {
    //             needResource |= value;
    //             if (!obstacles.ContainsKey(typeof(WayBuild))) continue;
    //             foreach (var wayBuild in obstacles[typeof(WayBuild)].Cast<WayBuild>())
    //             {
    //                 wayBuild.Notice(value, isHave =>
    //                 {
    //                     if (!isHave) return;
    //                     needResource ^= value;
    //                     GetLine(wayBuild).Push(false, () =>
    //                     {
    //                         wayBuild.ChangeNum(value,-1);
    //                         ChangeNum(value);
    //                     });
    //                 });
    //             }
    //         }
    //     }
    // }
    //
    // protected virtual void SendNotice(Resource value)
    // {
    //     if (!obstacles.ContainsKey(typeof(WayBuild))) return;
    //     foreach (var wayBuild in obstacles[typeof(WayBuild)].Cast<WayBuild>())
    //     {
    //         wayBuild.Notice(value, isHave =>
    //         {
    //             if (!isHave) return;
    //             needResource ^= value;
    //             GetLine(wayBuild).Push(false, () =>
    //             {
    //                 wayBuild.ChangeNum(value,-1);
    //                 ChangeNum(value);
    //             });
    //         });
    //     }
    // }
    
    
    // 路径
    protected Dictionary<Resource,Tuple<BaseBuild,BaseBuild>> ways = new();
    
    
    
    
}