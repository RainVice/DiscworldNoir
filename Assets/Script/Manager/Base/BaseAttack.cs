
    public abstract class BaseAttack : BaseBuild
    {
        
        // 攻击力
        protected int attack;
        
        protected override void Awake()
        {
            base.Awake();
            attack = m_buildData.attack;
            buildType = BuildType.Attack;

        }
        
    }