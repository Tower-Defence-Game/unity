using Classes.Elements;

namespace Classes.Damage
{
    public class Damage
    {
        public Element ElementType { get; }
        public float DamageValue { get; }

        public Damage(Element elementType, float damageValue)
        {
            ElementType = elementType;
            DamageValue = damageValue;
        }
    }
}