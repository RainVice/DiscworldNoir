using System;
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
}