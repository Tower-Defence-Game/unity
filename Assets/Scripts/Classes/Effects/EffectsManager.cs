using System.Collections.Generic;
using System.Linq;
using Classes.Effects;
using Interfaces.ObjectAbilities;

public class EffectsManager
{
    // Здесь мы храним все инициализированные эффекты, наложенные на объект
    private readonly List<AbstractEffect> _effects = new();

    // Здесь мы храним только оригинальные эффекты, наложенные на объект (конкретные ассеты)
    private HashSet<AbstractEffect> _effectsOrigin = new();
    private readonly ICanHaveEffects _owner;

    private IEnumerable<AbstractDurationEffect> DurationEffects => _effects.OfType<AbstractDurationEffect>().ToList();
    private IEnumerable<AbstractInstantEffect> InstantEffects => _effects.OfType<AbstractInstantEffect>().ToList();

    public EffectsManager(ICanHaveEffects owner)
    {
        _owner = owner;
    }

    public void AddEffect(AbstractEffect effect)
    {
        if (effect is AbstractDurationEffect durationEffect)
        {
            if (_effectsOrigin.Contains(effect) && durationEffect.IsRefreshable)
            {
                _effects.FindAll(e => e.OriginEffect == effect).Cast<AbstractDurationEffect>().ToList()
                    .ForEach(e => e.Refresh());
            }

            if (_effectsOrigin.Contains(effect) && !durationEffect.IsStackable)
            {
                return;
            }
        }


        effect = effect.Init();
        _effectsOrigin.Add(effect);
        _effects.Add(effect);
    }

    public void RemoveEffect(AbstractEffect effect)
    {
        _effectsOrigin.Remove(effect);

        var effects = _effects.FindAll(e => e.OriginEffect == effect);
        effects.ForEach(e => e.Recycle());
        _effects.RemoveAll(e => effect.OriginEffect == effect);
    }

    public void ResetEffects()
    {
        foreach (var effect in _effects)
        {
            effect.Recycle();
        }

        _effectsOrigin.Clear();
        _effects.Clear();
    }

    public void Update(float deltaTime)
    {
        // Применяем эффекты
        foreach (var effect in _effects)
        {
            effect.Apply(_owner);
        }

        #region Обработка длительных эффектов

        // Тикаем длительные эффекты
        foreach (var effect in DurationEffects)
        {
            effect.Tick(deltaTime);
        }

        // Удаляем завершенные эффекты
        var finishedEffects = DurationEffects.Where(effect => effect.IsFinished);

        foreach (var finishedEffect in finishedEffects)
        {
            finishedEffect.Recycle();
            _effects.Remove(finishedEffect);
        }

        #endregion

        #region Обработка мгновенных эффектов

        foreach (var effect in InstantEffects)
        {
            effect.Recycle();
            _effects.Remove(effect);
        }

        #endregion

        // Обновляем список оригинальных эффектов

        _effectsOrigin = new HashSet<AbstractEffect>();
        foreach (var effect in _effects)
        {
            _effectsOrigin.Add(effect.OriginEffect);
        }
    }
}