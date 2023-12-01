
using System;
using UnityEngine;

public class CionBuild : BaseBuild
{
    
    // 生产速度
    private int productionSpeed;
    // 运输速度
    private int waySpeed;
    protected override void Awake()
    {
        base.Awake();
        productionSpeed = m_buildData.productionSpeed;
        waySpeed = m_buildData.waySpeed;
    }


    private void FixedUpdate()
    {
        // 扫描周围的地形
        var layerPosition = new LayerPosition(distance);
        layerPosition.Scan(transform.position, v3 =>
        {
            var baseObstacle = GameManager.Instance.GetObstacle(v3);
            if (baseObstacle.ObstacleType == ObstacleType.Terrain)
            {
                
            }
        });

    }
}