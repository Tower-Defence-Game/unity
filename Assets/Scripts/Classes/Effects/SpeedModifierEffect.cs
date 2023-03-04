using Interfaces.ObjectProperties;
using UnityEngine;

namespace Classes.Effects
{
    [CreateAssetMenu(fileName = "SpeedModifierEffect", menuName = "Effects/Speed Modifier", order = 0)]
    public class SpeedModifierEffect : AbstractEffect
    {
        [SerializeField] private float duration;
        [SerializeField] private float modifier;
        public override float Duration => duration;

        public override void Apply(object target)
        {
            if (target is not IHaveSpeed haveSpeed)
            {
                return;
            }

            haveSpeed.Speed = Mathf.Max(0, haveSpeed.Speed * modifier);
        }
    }
}