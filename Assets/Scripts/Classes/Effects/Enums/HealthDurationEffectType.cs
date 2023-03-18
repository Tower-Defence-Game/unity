namespace Classes.Effects.Enums
{
    public enum HealthDurationEffectType
    {
        RelativeToBaseHealth,
        RelativeToCurrentHealth,

        // todo: After implementing damage, add RelativeToTakenDamage
        RelativeToTakenDamage,
        Absolute
    }
}