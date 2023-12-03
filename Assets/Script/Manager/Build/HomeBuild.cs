using UnityEngine;

/// <summary>
/// 大本营控制类
/// </summary>
public class HomeBuild : BaseBuild
{
    // 攻击力
    private int attack;
    
    // 水晶数量
    private int m_CrystalCount;
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
    /// <summary>
    /// 添加水晶数量
    /// </summary>
    /// <param name="num"> 默认数量是 1</param>
    public void AddCrystal(int num = 1)
    {
        m_CrystalCount+=num;
        Debug.Log(m_CrystalCount);
    }
}