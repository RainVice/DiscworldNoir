using System;

public class WayBuild : BaseWay
{
    
    private int m_CylinderCount;


    protected override void OnScan(out Action clean, out Action<BaseObstacle> action)
    {
        clean = () =>
        {
            
        };
        action = obstacle =>
        {
            if (obstacle as BaseBuild)
            {
                var baseBuild = obstacle as BaseBuild;
                if (! (baseBuild as WallBuild))
                {
                    AddLine(baseBuild);
                }
            }
        };
    }
    
    public void AddCylinder(int count = 1)
    {
        m_CylinderCount += count;
    }
}