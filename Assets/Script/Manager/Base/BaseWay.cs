
    public abstract class BaseWay : BaseBuild
    {
        protected override void Awake()
        {
            base.Awake();
            buildType = BuildType.Way;
        }
    }