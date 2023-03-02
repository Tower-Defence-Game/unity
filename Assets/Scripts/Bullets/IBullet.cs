using Classes.Damage;

namespace Bullets
{
    public interface IBullet
    {
        IDamage Damage { get; }
        Enemy Target { get; }

        public void Init(IDamage damage, Enemy target);
    }
}