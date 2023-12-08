using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseAttack
{

    public override bool CanPlace()
    {
        var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
        return homeBuilds.Count < 1;
    }

    public override Resource IsNeed()
    {
        return Resource.Crystal | Resource.Ingot | Resource.Wood | Resource.Bullet;
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
                var findAllWay = GameManager.Instance.FindAllWay(CurPos, value, out List<BaseBuild> baseBuilds);
                for (var i = 0; i < findAllWay.Count; i++)
                {
                    var vector3Ints = findAllWay[i];
                    var baseBuild = baseBuilds[i];
                    GameManager.Instance.Send(vector3Ints,CurPos, () =>
                    {
                        baseBuild.ChangeNum(value, -1);
                        ChangeNum(value);
                    });
                }
            }
        }
    }

    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"攻击力：{attack} > {attack * Constant.Upgrade(level)} \n" +
            $"当前血量：{hp}");
    }

    public override void Remove()
    {
        UIManager.Instance.CreateToast("大本营不可移除");
    }

    public override void Upgrade()
    {
        base.Upgrade();
        attack *= (int)Constant.Upgrade(level);
        maxHp *= (int)Constant.Upgrade(level);
    }
}