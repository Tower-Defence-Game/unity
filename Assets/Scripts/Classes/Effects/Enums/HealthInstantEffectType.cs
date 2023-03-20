namespace Classes.Effects.Enums
{
    public enum HealthInstantEffectType
    {
        RelativeToBaseHealth,
        RelativeToCurrentHealth,

        // todo: After implementing damage, add RelativeToTakenDamage
        RelativeToTakenDamage,
        Absolute
    }
}