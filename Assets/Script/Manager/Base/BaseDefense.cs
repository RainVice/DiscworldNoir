
    public abstract class BaseDefense: BaseBuild
    {
        protected override void Awake()
        {
            base.Awake();
            buildType = BuildType.Defense;
        }
    }