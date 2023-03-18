using Classes.Damage;

namespace Interfaces.ObjectAbilities
{
    public interface ICanTakeDamage
    {
        public Damage LastTakenDamage { get; }
        public void TakeDamage(object origin, Damage damage);
    }
}