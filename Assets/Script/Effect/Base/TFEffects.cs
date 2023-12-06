using System.Collections.Generic;
using UnityEngine;

namespace Effect
{
    public class TFEffects : BaseEffect
    {
        protected Vector3 reSetPosition;
        private Transform transform;
        private List<Vector3> points;
        public TFEffects(Transform transform, List<Vector3> points, float time, float delayTime)
        {
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
            var agv = (int)(timer / (time / (points.Count - 1)));
            transform.position = Vector3.Lerp(points[agv], points[agv + 1], timer / time);
            if (timer >= time)
                isDone = true;
        }
    }
}