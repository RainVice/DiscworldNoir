using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Effect
{
    public class EffectSet
    {
        
        private bool isRun = true;
        private bool isDone;
        private MonoBehaviour mono;
        private Action finishCallback;
        private Dictionary<GameObject,List<BaseEffect>> effects = new();
        
        public EffectSet(MonoBehaviour mono)
        {
            this.mono = mono;
        }
        
        /// <summary>
        /// 播放特效
        /// </summary>
        /// <param name="action"> 完成时的回调</param>
        public void Play(Action action = null)
        {
            mono.StartCoroutine(Run());
            finishCallback = action;
        }
        
        
        /// <summary>
        /// 添加完成回调
        /// </summary>
        /// <param name="action"></param>
        public void AddFinishCallback(Action action)
        {
            finishCallback += action;
        }
        
        

        /// <summary>
        /// 添加特效，有同类型的特效则覆盖
        /// </summary>
        /// <param name="effect"></param>
        public EffectSet AddEffect(BaseEffect effect)
        {
            if (EffectCenter.effects.TryGetValue(effect.Go, out var eff))
            {
                if (eff.Contains(effect.EffectType))
                {
                    Debug.LogWarning("当前对象已有同类型动画集存在,添加失败");
                }
                else
                {
                    EffectCenter.effects[effect.Go].Add(effect.EffectType);
                }
            }
            else
            {
                EffectCenter.effects.Add(effect.Go, new List<EffectType>(new[] { effect.EffectType }));
            }
            if (!effects.ContainsKey(effect.Go))
            {
                effects[effect.Go] = new List<BaseEffect>();
            }
            effects[effect.Go].Add(effect);
            return this;
        }

        /// <summary>
        /// 添加特效列表
        /// </summary>
        /// <param name="effectList"></param>
        /// <returns></returns>
        public EffectSet AddEffects(IEnumerable<BaseEffect> effectList)
        {
            foreach (var baseEffect in effectList)
            {
                AddEffect(baseEffect);
            }
            return this;
        }
        /// <summary>
        /// 运行特效
        /// </summary>
        /// <returns></returns>
        private IEnumerator Run()
        {
            var num = effects.Count;
            while (isRun && !isDone)
            {
                foreach (var effect in effects.Values.SelectMany(value => value))
                {
                    if (!effect.IsDone)
                    {
                        effect.Do();
                    }
                    else
                    {
                        num--;
                    }
                }
                if (num == 0)
                {
                    isDone = true;
                }
                yield return null;
            }

            if (isRun && isDone)
            {
                finishCallback?.Invoke();
                Stop();
            }
        }

        /// <summary>
        /// 停止特效
        /// </summary>
        /// <param name="action"> 停止的回调</param>
        public void Stop(Action action = null)
        {
            isRun = false;
            foreach (var effectsKey in effects.Keys)
            {
                EffectCenter.effects.Remove(effectsKey);
            }
            action?.Invoke();
        }
        
    }
}