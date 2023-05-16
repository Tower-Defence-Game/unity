namespace Interfaces.ObjectProperties
{
    public interface IHaveHealth
    {
        public float Health { get; set; }
        public float MaxHealth { get; }

        public bool IsDamaged => Health < MaxHealth;
        public float HealthPercentage => Health / MaxHealth;
    }
}