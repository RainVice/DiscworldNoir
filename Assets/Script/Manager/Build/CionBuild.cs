
using System;
using System.Collections.Generic;
using UnityEngine;

public class CionBuild : BaseBuild
{
    
    // 生产速度
    private int productionSpeed;
    // 运输速度
    private int waySpeed;
    // 大本营
    private HomeBuild m_homeBuild;
    // 水晶集合
    private List<CrystalTerrain> m_crystalTerrains = new();
    // 线路集合
    private List<WayBuild> m_wayBuilds = new();
    protected override void Awake()
    {
        base.Awake();
        productionSpeed = m_buildData.productionSpeed;
        waySpeed = m_buildData.waySpeed;
    }

    protected override void OnScan()
    {
        base.OnScan();

        // 扫描之前清理上一次数据
        m_homeBuild = null;
        m_crystalTerrains.Clear();
        m_wayBuilds.Clear();

        // 扫描周围的地形
        var layerPosition = new LayerPosition(distance);
        GameManager.Instance.Scan(layerPosition,transform.position, obstacle =>
        {
            switch (obstacle)
            {
                case CrystalTerrain terrain:
                    AddLine(terrain);
                    m_crystalTerrains.Add(terrain);
                    break;
                case HomeBuild home:
                    m_homeBuild = home;
                    AddLine(home);
                    break;
                case WayBuild way:
                    m_wayBuilds.Add(way);
                    AddLine(way);
                    break;
            }
        });
    }


    public override bool CanPlace()
    {
        return m_crystalTerrains.Count >= 1;
    }

}