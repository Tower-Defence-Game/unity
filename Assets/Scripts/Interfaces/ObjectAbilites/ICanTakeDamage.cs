using Classes.Damage;

namespace Interfaces.ObjectAbilities
{
    public interface ICanTakeDamage
    {
        public void TakeDamage(Damage damage);
    }
}