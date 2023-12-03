using UnityEngine;

namespace Effect
{
    public static class EffectUtility
    {
        
        public static EffectSet CreateEffect(this MonoBehaviour mono)
        {
            return new EffectSet(mono);
        }
        
        public static BaseEffect SlideTFTo(this Transform transform,Vector3 start,Vector3 end,float time = 1f,float delay = 1f) => 
            new TFEffect(transform,start,end,time,delay);
        public static BaseEffect SlideTFTo(this Transform transform,Vector3 end,float time,float delay) => 
            SlideTFTo(transform,transform.position,end,time,delay);
        public static BaseEffect SlideTF(this Transform transform, Vector3 distance, float time = 1f, float delay = 1f) => 
            SlideTFTo(transform, transform.position, transform.position + distance, time, delay);
    }
}