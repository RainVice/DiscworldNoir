/// <summary>
/// 防御物品的基类
/// </summary>
public abstract class BaseDefense: BaseBuild
{
    protected override void Awake()
    {
        base.Awake();
        buildType = BuildType.Defense;
    }
}