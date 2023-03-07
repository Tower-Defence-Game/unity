using System.Collections.Generic;
using Classes.Effects;
using Interfaces.ObjectAbilities;

public class EffectManager
{
    private readonly List<AbstractEffect> _effects = new();
    private HashSet<AbstractEffect> _effectsOrigin = new();
    private readonly ICanHaveEffects _owner;

    public EffectManager(ICanHaveEffects owner)
    {
        _owner = owner;
    }

    public void AddEffect(AbstractEffect effect)
    {
        if (_effectsOrigin.Contains(effect) && effect.IsRefreshable)
        {
            _effects.FindAll(e => e.OriginEffect == effect).ForEach(e => e.Refresh());
        }

        if (_effectsOrigin.Contains(effect) && !effect.IsStackable)
        {
            return;
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
        foreach (var effect in _effects)
        {
            effect.Tick(deltaTime);
            effect.Apply(_owner);
        }

        var finishedEffects = _effects.FindAll(effect => effect.IsFinished);
        finishedEffects.ForEach(effect => effect.Recycle());
        _effects.RemoveAll(effect => effect.IsFinished);

        _effectsOrigin = new HashSet<AbstractEffect>();
        foreach (var effect in _effects)
        {
            _effectsOrigin.Add(effect.OriginEffect);
        }
    }
}