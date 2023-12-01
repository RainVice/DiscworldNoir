public class IronTerrain : BaseTerrain
{
    protected override void Awake()
    {
        base.Awake();
        m_terrainType = TerrainType.Iron;
        
    }
}