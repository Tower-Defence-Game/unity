using UnityEngine;

namespace Classes.Effects
{
    public abstract class AbstractDurationEffect : AbstractEffect
    {
        [SerializeField] private bool isStackable;
        [SerializeField] private bool isRefreshable;
        [SerializeField] private float duration;
        public bool IsStackable => isStackable;
        public bool IsRefreshable => isRefreshable;
        public float Duration => duration;
        public virtual float TimeLeft { get; protected set; }
        public bool IsFinished => TimeLeft < 0;

        public override AbstractEffect Init()
        {
            var effect = Instantiate(this);
            effect.TimeLeft = Duration;
            effect.OriginEffect = this;
            return effect;
        }

        public virtual void Tick(float deltaTime)
        {
            TimeLeft -= deltaTime;
        }

        public virtual void Refresh()
        {
            TimeLeft = Duration;
        }
    }
}