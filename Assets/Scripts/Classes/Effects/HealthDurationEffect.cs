using System;
using Classes.Effects.Enums;
using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using UnityEngine;

namespace Classes.Effects
{
    public class HealthDurationEffect : AbstractDurationEffect
    {
        [SerializeField] [Tooltip("Тип модификатора здоровья")]
        private HealthDurationEffectType healthDurationEffectType;

        [SerializeField] [Tooltip("Значение модификатора")]
        private float healthModifier;

        [SerializeField] [Tooltip("Может ли здоровье стать больше максимума")]
        private bool canExceedMaxHealth;

        [SerializeField] [Tooltip("Применить эффект сразу")]
        private bool startImmediately;

        [SerializeField] [Tooltip("Периодичность эффекта в секундах")]
        private float intervalInSeconds;

        private float _timeLeftToNextTick;

        public override AbstractEffect Init()
        {
            if (!startImmediately)
            {
                _timeLeftToNextTick = intervalInSeconds;
            }

            return base.Init();
        }

        public override void Apply(object target)
        {
            if (target is not IHaveHealth haveHealth)
            {
                Debug.LogWarning("HealthDurationEffect is applied to object without IHaveHealth");
                return;
            }

            if (
                healthDurationEffectType == HealthDurationEffectType.RelativeToTakenDamage &&
                target is not ICanTakeDamage
            )
            {
                Debug.LogWarning(
                    "HealthDurationEffect with RelativeToTakenDamage type is applied to object without ICanTakeDamage"
                );
                return;
            }

            if (_timeLeftToNextTick > 0)
            {
                return;
            }

            _timeLeftToNextTick += intervalInSeconds;

            var deltaHealth = healthDurationEffectType switch
            {
                HealthDurationEffectType.RelativeToBaseHealth => haveHealth.MaxHealth * healthModifier,
                HealthDurationEffectType.RelativeToCurrentHealth => haveHealth.Health * healthModifier,
                HealthDurationEffectType.RelativeToTakenDamage =>
                    (((ICanTakeDamage)target).LastTakenDamage?.DamageValue ?? 0f) * healthModifier,
                HealthDurationEffectType.Absolute => healthModifier,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (!canExceedMaxHealth && deltaHealth > 0)
            {
                deltaHealth = Mathf.Min(deltaHealth, haveHealth.MaxHealth - haveHealth.Health);
                deltaHealth = Mathf.Max(deltaHealth, 0);
            }

            haveHealth.Health += deltaHealth;
        }

        public override void Tick(float deltaTime)
        {
            _timeLeftToNextTick -= deltaTime;
            base.Tick(deltaTime);
        }
    }
}