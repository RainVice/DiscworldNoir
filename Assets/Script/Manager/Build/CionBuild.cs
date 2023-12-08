using System;
using System.Linq;
using UnityEngine;

public class CionBuild : BaseProduce
{
    protected override void OnProduce()
    {
        base.OnProduce();
        foreach (var obstaclesValue in obstacles.Values)
        {
            foreach (var baseObstacle in obstaclesValue)
            {
                if (baseObstacle is not BaseTerrain baseTerrain) continue;
                var isOut = baseTerrain.IsOut();
                if (isOut.HasFlag(Resource.Crystal))
                {
                    GetLine(baseTerrain).Push(false, () =>
                    {
                        ChangeNum(isOut);
                    });
                }
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
                case CrystalTerrain :
                case HomeBuild :
                case BaseWay :
                    AddLine(obstacle);
                    break;
            }
        };
    }
    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(CrystalTerrain));
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Crystal) && GetNum(Resource.Crystal) > 0;
    }
    
    
    public override void ShowInfo()
    {
        UIManager.Instance.ShowInfo(this,transform.position,
            m_buildData.name,
            $"最大血量：{maxHp} > {(int)(maxHp * Constant.Upgrade(level))}\n" +
            $"生产速度：{productionSpeed} > {productionSpeed * Constant.Upgrade(level)} \n" +
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
        productionSpeed *= Constant.Upgrade(level);
        waySpeed *= Constant.Upgrade(level);
    }
    
}