using Classes.Damage;

namespace Bullets
{
    public interface IBullet
    {
        Damage Damage { get; }

        public void Init(Damage damage, Enemy target);
    }
}