using UnityEngine;

namespace Effect
{
    public class TFEffect : BaseEffect
    {
        protected Vector3 reSetPosition;
        private Transform transform;
        private Vector3 start;
        private Vector3 end;

        public TFEffect(Transform transform, Vector3 start, Vector3 end, float time, float delayTime)
        {
            reSetPosition = transform.position;
            effectType = EffectType.Move;
            this.transform = transform;
            this.start = start;
            this.end = end;
            this.time = time;
            this.delayTime = delayTime;
        }

        public override void Do()
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(start, end, timer / time);
            if (timer >= 1)
                isDone = true;
        }
    }
}