using Bullets;
using Classes.Damage;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float speed = 5f;

    public Damage Damage { get; private set; }
    public Enemy Target { get; private set; }

    private object _origin;

    public void Init(object origin, Damage damage, Enemy target)
    {
        _origin = origin;
        Damage = damage;
        Target = target;
    }

    void FixedUpdate()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        var position = transform.position;
        var targetPosition = Target.Position;
        Vector3 direction = position - targetPosition;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // move bullet towards the enemy
        position = Vector3.MoveTowards(position, targetPosition, speed * Time.fixedDeltaTime);
        transform.position = position;

        // check if bullet reached the enemy
        if (position == targetPosition)
        {
            Target.TakeDamage(_origin, Damage);
            Destroy(gameObject);
        }
    }
}