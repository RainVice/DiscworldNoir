public class WayBuild : BaseBuild
{
    
    private int m_CylinderCount;
    private int m_WaySpeed;

    protected override void Awake()
    {
        base.Awake();
        m_WaySpeed = m_buildData.waySpeed;
    }

    public void AddCylinder(int count = 1)
    {
        m_CylinderCount += count;
    }
    

}