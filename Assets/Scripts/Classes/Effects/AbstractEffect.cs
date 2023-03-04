using UnityEngine;

namespace Classes.Effects
{
    public abstract class AbstractEffect : ScriptableObject
    {
        [SerializeField] private bool isStackable;
        [SerializeField] private bool isRefreshable;
        public bool IsStackable => isStackable;
        public bool IsRefreshable => isRefreshable;
        public AbstractEffect OriginEffect { get; set; }
        public abstract float Duration { get; }
        public virtual float TimeLeft { get; protected set; }
        public bool IsFinished => TimeLeft < 0;

        public abstract void Apply(object target);

        public AbstractEffect Init()
        {
            var effect = Instantiate(this);
            effect.TimeLeft = Duration;
            effect.OriginEffect = this;
            return effect;
        }

        public void Recycle()
        {
            Destroy(this);
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