using Classes.Combinations;
using Classes.Damage;
using Classes.Elements;
using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Enemy : MonoBehaviour, IHaveSpeed, IHaveHealth, IHaveResistance, ICanTakeDamage,
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
    public ElementsManager ElementsManager { get; private set; }
    public ElementsCombinationManager ElementsCombinationManager { get; private set; }
    public EffectsManager EffectsManager { get; private set; }
    public ElementCombinationList ElementCombinationList => elementCombinationList;
    public Vector3 Position => transform.position;

    protected virtual void Start()
    {
        ResetResistance();
        ResetSpeed();
        Health = health;

        EffectsManager = new EffectsManager(this);
        ElementsManager = new ElementsManager(this);
        ElementsCombinationManager = new ElementsCombinationManager(this);
    }

    protected virtual void Update()
    {
        ResetResistance();
        ResetSpeed();

        ElementsCombinationManager.Update();
        EffectsManager.Update(Time.deltaTime);
    }

    public virtual void TakeDamage(Damage damage)
    {
        Health -= damage.DamageValue / Resistance;
        ElementsManager.AddElement(damage.ElementType);
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