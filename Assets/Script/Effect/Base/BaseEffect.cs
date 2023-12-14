using UnityEngine;

namespace Effect
{
    public abstract class BaseEffect
    {
        public GameObject Go {
            get => go;
            protected set => go = value;
        }
        
        public EffectType EffectType => effectType;
        public bool IsDone => isDone;

        // 动画类型
        protected EffectType effectType;
        // 特效是否完成
        protected bool isDone;
        // 特效时间
        protected float time;
        // 特效延迟播放时间
        protected float delayTime;
        // 计时器
        protected float timer;
        // 执行动画的对象
        protected GameObject go;
        
        /// <summary>
        /// 特效执行逻辑
        /// </summary>
        public abstract void Do();
    }


    public enum EffectType
    {
        // 无
        None,
        // 移动
        Move,
        // 旋转
        Rotate,
        // 缩放
        Scale,
        // 颜色
        Color,
    }
    
}