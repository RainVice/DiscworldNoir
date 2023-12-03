using System;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    // 水晶库存
    private int m_CrystalCount;

    // 生产计时
    private float m_ProductionTime;

    // 运输计时
    private float m_WayTime;

    protected override void Awake()
    {
        base.Awake();
        productionSpeed = m_buildData.productionSpeed;
        waySpeed = m_buildData.waySpeed;
    }

    private void Update()
    {
        if (isPlace)
        {
            m_ProductionTime += Time.deltaTime;
            m_WayTime += Time.deltaTime;
            // 生产
            if (m_ProductionTime >= productionSpeed)
            {
                m_ProductionTime %= productionSpeed;
                foreach (var crystalTerrain in m_crystalTerrains)
                {
                    m_lineDic[crystalTerrain].Push(false, () => { m_CrystalCount++; });
                }
            }
            // 运输
            if (m_WayTime >= waySpeed && m_CrystalCount > 0)
            {
                if (m_CrystalCount > 0)
                {
                    m_WayTime %= waySpeed;
                    foreach (var wayBuild in m_wayBuilds)
                    {
                        m_lineDic[wayBuild].Push(true, () =>
                        {
                            wayBuild.AddCylinder();
                            m_CrystalCount--;
                        });
                    }
                }
                // 送到大本营
                if (m_homeBuild)
                {
                    m_lineDic[m_homeBuild].Push(true, () =>
                    {
                        m_CrystalCount--;
                        m_homeBuild.AddCrystal();
                    });
                }
                
            }
        }
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
        GameManager.Instance.Scan(layerPosition, transform.position, obstacle =>
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