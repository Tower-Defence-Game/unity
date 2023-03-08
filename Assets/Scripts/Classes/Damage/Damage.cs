using Classes.Elements;
using JetBrains.Annotations;

namespace Classes.Damage
{
    public class Damage
    {
        [CanBeNull] public Element ElementType { get; }
        public float DamageValue { get; }

        public Damage([CanBeNull] Element elementType, float damageValue)
        {
            ElementType = elementType;
            DamageValue = damageValue;
        }
    }
}