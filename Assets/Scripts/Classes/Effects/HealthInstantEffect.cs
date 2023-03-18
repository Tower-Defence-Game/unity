using System;
using Classes.Effects.Enums;
using Interfaces.ObjectProperties;
using UnityEngine;

namespace Classes.Effects
{
    public class HealthInstantEffect : AbstractInstantEffect
    {
        [SerializeField] [Tooltip("Тип модификатора здоровья")]
        private HealthInstantEffectType healthDurationEffectType;

        [SerializeField] [Tooltip("Значение модификатора")]
        private float healthModifier;

        [Tooltip("Можно ли выходить за пределы максимального здоровья")] [SerializeField]
        private bool canExceedMaxHealth;

        public override void Apply(object target)
        {
            if (target is not IHaveHealth haveHealth)
            {
                return;
            }

            var deltaHealth = healthDurationEffectType switch
            {
                HealthInstantEffectType.RelativeToBaseHealth => haveHealth.MaxHealth * healthModifier,
                HealthInstantEffectType.RelativeToCurrentHealth => haveHealth.Health * healthModifier,
                HealthInstantEffectType.Absolute => healthModifier,
                _ => throw new ArgumentOutOfRangeException()
            };

            // Если здоровье не может быть больше максимума, то ограничиваем дельту
            if (!canExceedMaxHealth && deltaHealth > 0)
            {
                deltaHealth = Mathf.Min(deltaHealth, haveHealth.MaxHealth - haveHealth.Health);
                deltaHealth = Mathf.Max(deltaHealth, 0);
            }

            haveHealth.Health += deltaHealth;
        }
    }
}