
using System;
using UnityEngine;

public class CionBuild : BaseBuild
{
    
    // 生产速度
    public int productionSpeed;
    // 运输速度
    public int waySpeed;
    protected override void Awake()
    {
        base.Awake();
        productionSpeed = m_buildData.productionSpeed;
        waySpeed = m_buildData.waySpeed;
    }


    private void FixedUpdate()
    {
        
    }
}