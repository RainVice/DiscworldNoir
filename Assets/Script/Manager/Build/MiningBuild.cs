using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// 铁矿挖掘控制类
/// </summary>
public class MiningBuild : BaseProduce
{    
    protected override void OnProduce()
    {
        base.OnProduce();
        foreach (var baseObstacle in obstacles.Values.SelectMany(obstaclesValue => obstaclesValue))
        {
            // 判断是否有钢铁，如果有则生产
            if (baseObstacle is not BaseTerrain baseTerrain) continue;
            var isOut = baseTerrain.IsOut();
            if (isOut.HasFlag(Resource.Iron))
            {
                GetLine(baseTerrain).Push(false, () =>
                {
                    ChangeNum(Resource.Iron);
                });
            }
        }
    }
    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = null;
        action = obstacle =>
        {
            switch (obstacle)
            {
                case IronTerrain :
                case FactoryBuild :
                case BaseWay :
                    AddLine(obstacle);
                    break;
            }
        };
    }
    
    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(IronTerrain));
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Iron) && GetNum(Resource.Iron) > 0;
    }
    
    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"生产速度：{productionSpeed} > {productionSpeed * Constant.Upgrade(level)} \n" +
            $"传输速度：{waySpeed} > {waySpeed * Constant.Upgrade(level)}\n" +
            $"当前血量：{hp}");
    }

    public override void Remove()
    {
        Destroy(gameObject);
    }

    public override void Upgrade()
    {
        base.Upgrade();
        maxHp *= (int)Constant.Upgrade(level);
        productionSpeed *= Constant.Upgrade(level);
        waySpeed *= Constant.Upgrade(level);
    }
    
}