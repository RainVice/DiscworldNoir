using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProduce : BaseBuild
{
    
    // 生产速度
    protected int productionSpeed;
    // 运输速度
    protected int waySpeed;
    // 生产计时
    private float m_productionTimer;
    // 运输计时
    private float m_wayTimer;
    
    protected override void Awake()
    {
        base.Awake();
        buildType = BuildType.Production;
        productionSpeed = m_buildData.productionSpeed;
        waySpeed = m_buildData.waySpeed;
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
        // 判断是否可以调用运输事件
        if (waySpeed!= 0 && isPlace)
        {
            m_wayTimer += Time.fixedDeltaTime;
            if (m_wayTimer >= Constant.DEFAULTTIME / waySpeed)
            {
                m_wayTimer %= Constant.DEFAULTTIME / waySpeed;
                OnWay();
            }
        }
    }
    
    /// <summary>
    /// 生产事件
    /// </summary>
    protected virtual void OnProduce() { }

    /// <summary>
    /// 运输事件
    /// </summary>
    protected virtual void OnWay() { }

    
}