using Bullets;
using Classes.Damage;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float speed = 5f;

    public Damage Damage { get; private set; }
    public Enemy Target { get; private set; }


    public void Init(Damage damage, Enemy target)
    {
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
            Target.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}