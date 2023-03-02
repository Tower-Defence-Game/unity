namespace Classes.Damage
{
    public class PhysicalDamage : IDamage
    {
        public PhysicalDamage(float damage)
        {
            Damage = damage;
        }

        public DamageType DamageType => DamageType.Physical;
        public float Damage { get; }
    }
}