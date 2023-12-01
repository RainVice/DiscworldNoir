using System;

public abstract class BaseTerrain : BaseObstacle
{
    public TerrainType TerrainType => m_terrainType;

    protected TerrainType m_terrainType;
    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Terrain;
    }
}

public enum TerrainType
{
    // 树
    Tree,
    // 铁
    Iron,
    // 水晶
    Crystal
    
}