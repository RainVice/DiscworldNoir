public class TreeTerrain : BaseTerrain
{
    protected override void Awake()
    {
        base.Awake();
        m_terrainType = TerrainType.Tree;
        
    }
}