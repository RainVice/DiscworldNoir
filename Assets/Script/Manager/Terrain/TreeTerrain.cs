public class TreeTerrain : BaseTerrain
{
    protected override void Awake()
    {
        base.Awake();
        m_terrainType = TerrainType.Tree;
    }

    public override Resource IsOut()
    {
        return Resource.Tree;
    }
}