using System;
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
    
    public override bool IsHave(Resource resource)
    {
        return GetNum(resource) > 0;
    }
    
    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"传输速度：{waySpeed} > {waySpeed * Constant.Upgrade(level)}\n" +
            $"当前血量：{hp}\n" +
            $"升级花费：{Constant.DEFAULTNUM * level}");
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        maxHp *= (int)Constant.Upgrade(level);
        hp = maxHp;
        waySpeed *= Constant.Upgrade(level);
    }
    
    
}