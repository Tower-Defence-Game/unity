namespace Classes.Damage
{
    public interface IDamage
    {
        public DamageType DamageType { get; }
        public float Damage { get; }
    }
}