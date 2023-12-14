using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        
        
        public static BaseEffect ColorTo(this Graphic graphic,Color start,Color end,float time = 1f,float delay = 0f) => 
            new ColorEffect(graphic,start,end,time,delay);
        public static BaseEffect ColorTo(this Graphic graphic,Color end,float time = 1f,float delay = 0f) => 
            ColorTo(graphic,graphic.color,end,time,delay);
        public static BaseEffect Color(this Graphic graphic,Color distance,float time = 1f,float delay = 0f) => 
            ColorTo(graphic,graphic.color,graphic.color + distance,time,delay);
        
        
        public static BaseEffect ScaleTFTo(this Transform transform,Vector3 start,Vector3 end,float time = 1f,float delay = 0f) => 
            new ScaleEffect(transform,start,end,time,delay);
        public static BaseEffect ScaleTFTo(this Transform transform,Vector3 end,float time = 1f,float delay = 0f) => 
            ScaleTFTo(transform,transform.localScale,end,time,delay);
        public static BaseEffect ScaleTF(this Transform transform, Vector3 distance, float time = 1f, float delay = 0f) => 
            ScaleTFTo(transform, transform.localScale, transform.localScale + distance, time, delay);

        
    }
}