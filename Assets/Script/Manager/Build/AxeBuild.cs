using System;
using System.Linq;
using UnityEngine;

public class AxeBuild : BaseProduce
{
    protected override void OnProduce()
    {
        base.OnProduce();
        foreach (var baseObstacle in obstacles.Values.SelectMany(obstaclesValue => obstaclesValue))
        {
            // 判断是否有树，如果有则生产
            if (baseObstacle is not BaseTerrain baseTerrain) continue;
            var isOut = baseTerrain.IsOut();
            if (isOut.HasFlag(Resource.Tree))
            {
                GetLine(baseTerrain).Push(false, () =>
                {
                    ChangeNum(Resource.Tree);
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
                case TreeTerrain :
                case HarvesterBuild :
                case BaseWay :
                    AddLine(obstacle);
                    break;
            }
        };
    }
    public override bool CanPlace()
    {
        return obstacles.ContainsKey(typeof(TreeTerrain));
    }

    public override bool IsHave(Resource resource)
    {
        return resource.HasFlag(Resource.Tree) && GetNum(Resource.Tree) > 0;
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