
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class BaseMaker : BaseBuild
{
    // 生产速度
    protected int productionSpeed;
    // 生产计时
    private float m_productionTimer;
    
    protected override void Awake()
    {
        base.Awake();
        buildType = BuildType.Maker;
        productionSpeed = m_buildData.productionSpeed;
    }

    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        // 判断是否可以调用制造事件
        if (productionSpeed != 0 && isPlace)
        {
            m_productionTimer += Time.fixedDeltaTime;
            if (m_productionTimer >= Constant.DEFAULTTIME / productionSpeed)
            {
                m_productionTimer %= Constant.DEFAULTTIME / productionSpeed;
                Make();
            }
        }
    }

    /// <summary>
    /// 制造子弹
    /// </summary>
    private void Make()
    {
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
                    GameManager.Instance.Send(vector3Ints,CurPos, OnMake(baseBuild,value));
                }
            }
        }
    }

    /// <summary>
    /// 制造事件
    /// </summary>
    protected abstract Action OnMake(BaseBuild baseBuild, Resource resource);


}