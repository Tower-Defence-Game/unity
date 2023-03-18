using UnityEngine;

namespace Classes.Effects
{
    public abstract class AbstractEffect : ScriptableObject
    {
        public AbstractEffect OriginEffect { get; set; }

        public abstract void Apply(object target);

        public virtual AbstractEffect Init()
        {
            return Instantiate(this);
        }

        public virtual void Recycle()
        {
            Destroy(this);
        }
    }
}