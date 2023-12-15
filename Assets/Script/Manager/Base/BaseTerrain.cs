using System;
/// <summary>
/// 所有地形的基类
/// </summary>
public abstract class BaseTerrain : BaseObstacle
{
    public TerrainType TerrainType => m_terrainType;

    protected TerrainType m_terrainType;
    protected virtual void Awake()
    {
        m_obstacleType = ObstacleType.Terrain;
    }
    /// <summary>
    /// 询问是否需要传出物质
    /// </summary>
    /// <returns></returns>
    public virtual Resource IsOut()
    {
        return Resource.None;
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