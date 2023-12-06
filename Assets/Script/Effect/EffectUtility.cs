using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public static class EffectUtility
    {
        
        public static EffectSet CreateEffect(this MonoBehaviour mono)
        {
            return new EffectSet(mono);
        }
        
        public static BaseEffect SlideTFTo(this Transform transform,Vector3 start,Vector3 end,float time = 1f,float delay = 0f) => 
            new TFEffect(transform,start,end,time,delay);
        public static BaseEffect SlideTFTo(this Transform transform,Vector3 end,float time = 1f,float delay = 0f) => 
            SlideTFTo(transform,transform.position,end,time,delay);
        public static BaseEffect SlideTF(this Transform transform, Vector3 distance, float time = 1f, float delay = 0f) => 
            SlideTFTo(transform, transform.position, transform.position + distance, time, delay);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="points"></param>
        /// <param name="time">每个点间隔时间</param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public static BaseEffect SlideTFsTo(this Transform transform, List<Vector3> points,float time = 1f,float delay = 0f) => 
            new TFEffects(transform,points,time,delay);
    }
}