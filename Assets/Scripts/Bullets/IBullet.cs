using Classes.Damage;

namespace Bullets
{
    public interface IBullet
    {
        Damage Damage { get; }

        public void Init(object origin, Damage damage, Enemy target);
    }
}