using System.Collections.Generic;
using DB;
using UnityEngine;

/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseBuild
{
    private void Start()
    {

        var queryWapper = new QueryWapper<Home>();
        var homes = queryWapper.Do();
        Debug.Log(homes[0]);
    }
}