﻿using System;

public class CrystalTerrain : BaseTerrain
{
    
    protected override void Awake()
    {
        base.Awake();
        m_terrainType = TerrainType.Crystal;
        
    }


    public override Resource IsOut()
    {
        return Resource.Crystal;
    }
}