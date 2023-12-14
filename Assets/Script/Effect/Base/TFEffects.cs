using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class TFEffects : BaseEffect
    {
        protected Vector3 reSetPosition;
        private Transform transform;
        private List<Vector3> points;
        private float timeTemp;
        private int next;
        
        public TFEffects(Transform transform, List<Vector3> points, float time, float delayTime)
        {
            go = transform.gameObject;
            reSetPosition = transform.position;
            effectType = EffectType.Move;
            this.transform = transform;
            this.points = points;
            this.time = time;
            this.delayTime = delayTime;
        }
        
        public override void Do()
        {
            timer += Time.deltaTime;
            timeTemp += Time.deltaTime;
            transform.position = Vector3.Lerp(points[next], points[next + 1], timeTemp / time);
            if (timeTemp >= time)
            {
                timeTemp = 0;
                next++;
            }
            if (timer >= time * (points.Count - 1)) isDone = true;
        }
    }
}