using UnityEngine;
using UnityEngine.UI;

namespace Effect
{
    public class ColorEffect : BaseEffect
    {
        protected Color reColor;
        private Graphic graphic;
        private Color start;
        private Color end;
        public ColorEffect(Graphic graphic, Color start, Color end, float time, float delayTime)
        {
            go = graphic.gameObject;
            reColor = graphic.color;
            effectType = EffectType.Color;
            this.graphic = graphic;
            this.start = start;
            this.end = end;
            this.time = time;
            this.delayTime = delayTime;
        }
        public override void Do()
        {
            timer += Time.deltaTime;
            graphic.color = Color.Lerp(start, end, timer / time);
            if (timer >= time)
                isDone = true;
        }
    }
    
    
    
}