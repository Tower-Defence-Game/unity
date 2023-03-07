using Classes.Combinations;
using Classes.Damage;
using Classes.Elements;
using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour, IHaveSpeed, IHaveHealth, IHaveResistance, ICanTakeDamage, ICanHaveEffects,
    ICanCombineElement
{
    [SerializeField] private float speed = 1;
    [SerializeField] private float health = 100;
    [SerializeField] private float resistance = 1;
    [SerializeField] private Element element;
    [SerializeField] private ElementCombinationList elementCombinationList;

    public float Speed { get; set; }
    public float Health { get; set; }
    public float Resistance { get; set; }
    public Element Element => element;
    public ElementCombinationList ElementCombinationList => elementCombinationList;
    public ElementsCombinationManager ElementsCombinationManager { get; private set; }
    public EffectManager EffectManager { get; private set; }
    public Vector3 Position => transform.position;

    protected virtual void Start()
    {
        ResetResistance();
        ResetSpeed();
        Health = health;

        EffectManager = new EffectManager(this);
        ElementsCombinationManager = new ElementsCombinationManager(this);
    }

    protected virtual void Update()
    {
        ResetResistance();
        ResetSpeed();

        ElementsCombinationManager.Update();
        EffectManager.Update(Time.deltaTime);
    }

    public virtual void TakeDamage(Damage damage)
    {
        Health -= damage.DamageValue / Resistance;
        ElementsCombinationManager.AddElement(damage.ElementType);
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(gameObject);
    }

    public void ResetSpeed()
    {
        Speed = speed;
    }

    public void ResetResistance()
    {
        Resistance = resistance;
    }
}