using Interfaces.ObjectProperties;
using UnityEngine;

namespace Classes.Effects
{
    [CreateAssetMenu(fileName = "SpeedModifierEffect", menuName = "Effects/Speed Modifier", order = 0)]
    public class SpeedModifierEffect : AbstractEffect
    {
        [Tooltip("Модификатор скорости")] [Min(0)] [SerializeField]
        private float modifier;

        public override void Apply(object target)
        {
            if (target is not IHaveSpeed haveSpeed)
            {
                return;
            }

            haveSpeed.Speed *= modifier;
        }
    }
}