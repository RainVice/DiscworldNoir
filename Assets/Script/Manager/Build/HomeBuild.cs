/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseBuild
{
    // 攻击力
    private int attack;
    protected override void Awake()
    {
        base.Awake();
        attack = m_buildData.attack;
    }
    
    public override bool CanPlace()
    {
        var homeBuilds = GameManager.Instance.GetBuilds(typeof(HomeBuild));
        return homeBuilds.Count < 1;
    }
}