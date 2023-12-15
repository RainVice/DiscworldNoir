/// <summary>
/// 铁矿
/// </summary>
public class IronTerrain : BaseTerrain
{
    protected override void Awake()
    {
        base.Awake();
        m_terrainType = TerrainType.Iron;
    }

    public override Resource IsOut()
    {
        return Resource.Iron;
    }
}