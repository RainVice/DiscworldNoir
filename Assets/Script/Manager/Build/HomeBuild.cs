using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseAttack
{
    // 水晶数量
    private int m_CrystalCount;

    public override bool CanPlace()
    {
        var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
        return homeBuilds.Count < 1;
    }
    /// <summary>
    /// 添加水晶数量
    /// </summary>
    /// <param name="num"> 默认数量是 1</param>
    public override void ChangeData(int num = 1)
    {
        m_CrystalCount+=num;
    }
    
    public override Resource IsNeed()
    {
        return Resource.Crystal | Resource.Iron | Resource.Tree | Resource.Bullet;
    }

    // 向外询问一下是否有物资
    protected override void OnWay()
    {
        base.OnWay();
        foreach (Resource value in Enum.GetValues(typeof(Resource)))
        {
            if (value is Resource.All or Resource.None) continue;
            if (IsNeed().HasFlag(value))
            {
                BaseBuild baseBuild;
                var forEachTable = GameManager.Instance.FindOneWay(CurPos, value,out baseBuild);
                if (forEachTable != null)
                {
                    GameManager.Instance.Send(forEachTable,CurPos, () =>
                    {
                        baseBuild.ChangeNum(value, -1);
                        ChangeNum(value);
                    });
                }

                // var findAllWay = GameManager.Instance.FindAllWay(CurPos, value, out List<BaseBuild> baseBuilds);
                // for (var i = 0; i < findAllWay.Count; i++)
                // {
                //     var vector3Ints = findAllWay[i];
                //     var baseBuild = baseBuilds[i];
                //     GameManager.Instance.Send(vector3Ints,CurPos, () =>
                //     {
                //         baseBuild.ChangeNum(value, -1);
                //         ChangeNum(value);
                //     });
                // }
            }
        }
    }
}