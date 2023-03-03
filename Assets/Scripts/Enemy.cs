using Classes.Damage;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /**
     * Здоровье врага
     */
    [SerializeField] private float health = 100f;

    /**
     * Текущее здоровье врага
     */
    [SerializeField] private float currentHealth = default;

    public Vector3 Position => transform.position;

    public Collider2D Collider { get; private set; }

    void Start()
    {
        // Если текущее здоровье не задано, то присваиваем ему значение по умолчанию
        if (currentHealth == default)
        {
            currentHealth = health;
        }

        Collider = GetComponent<Collider2D>();

        // Проверяем, что у врага есть коллайдер
        Debug.Assert(Collider != null, "Enemy must have a collider");
    }

    void Die()
    {
        // TODO: Сделать смерть через фабрику (и анимацию)
        Destroy(gameObject);
    }

    public void TakeDamage(IDamage damage)
    {
        // TODO: Сделать проверку на разные типы урона

        currentHealth -= damage.Damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }
}