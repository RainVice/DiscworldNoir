using System;
using UnityEngine;

public class FactoryBuild : BaseMaker
{
    public override Resource IsNeed()
    {
        return Resource.Iron;
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Ingot) && GetNum(Resource.Ingot) > 0;
    }

    
    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case HomeBuild :
                case MiningBuild :
                case BulletBuild :
                case WayBuild :
                    AddLine(obstacle);
                    break;
            }
        };
    }
    
    protected override Action OnMake(BaseBuild baseBuild, Resource resource)
    {
        return () =>
        {
            baseBuild.ChangeNum(resource, -1);
            ChangeNum(resource);
            if (GetNum(Resource.Iron) > 1)
            {
                ChangeNum(Resource.Iron, -1);
                ChangeNum(Resource.Ingot);
            }
        };
    }

    public override bool CanPlace()
    {
        return base.CanPlace() && obstacles.ContainsKey(typeof(MiningBuild)) || obstacles.ContainsKey(typeof(WayBuild));
    }
    
    
    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"制造速度：{productionSpeed} > {productionSpeed * Constant.Upgrade(level)} \n" +
            $"传输速度：{waySpeed} > {waySpeed * Constant.Upgrade(level)}\n" +
            $"当前血量：{hp}\n" +
            $"升级花费：{Constant.DEFAULTNUM * level}");
    }
    
    public override void Upgrade()
    {
        base.Upgrade();
        maxHp *= (int)Constant.Upgrade(level);
        hp = maxHp;
        productionSpeed *= Constant.Upgrade(level);
        waySpeed *= Constant.Upgrade(level);
    }
    
    
}