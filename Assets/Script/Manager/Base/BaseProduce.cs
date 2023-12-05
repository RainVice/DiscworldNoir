using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseProduce : BaseBuild
{
    
    // 生产速度
    protected int productionSpeed;
    // 生产计时
    private float m_productionTimer;
    protected override void Awake()
    {
        base.Awake();
        buildType = BuildType.Production;
        productionSpeed = m_buildData.productionSpeed;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // 判断是否可以调用生产事件
        if (productionSpeed != 0 && isPlace)
        {
            m_productionTimer += Time.fixedDeltaTime;
            if (m_productionTimer >= Constant.DEFAULTTIME / productionSpeed)
            {
                m_productionTimer %= Constant.DEFAULTTIME / productionSpeed;
                OnProduce();
            }
        }
    }
    
    /// <summary>
    /// 生产事件
    /// </summary>
    protected virtual void OnProduce() { }

    protected override void OnWay()
    {
        base.OnWay();
        foreach (var baseObstacle in obstacles.Values.SelectMany(obstaclesValue => obstaclesValue))
        {
            if (baseObstacle is not BaseBuild baseBuild) continue;
            var isNeed = baseBuild.IsNeed();
            if (isNeed == Resource.None) continue;
            foreach (Resource value in Enum.GetValues(typeof(Resource)))
            {
                if (!isNeed.HasFlag(value)) continue;
                if (GetNum(value) > 0)
                {
                    GetLine(baseBuild).Push(true, () =>
                    {
                        ChangeNum(value,-1);
                        baseBuild.ChangeNum(value);
                    });
                }
            }
        }
    }
}