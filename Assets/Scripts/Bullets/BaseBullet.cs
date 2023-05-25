using Bullets;
using Classes.Damage;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IBullet
{
    [SerializeField] protected float speed = 5f;

    public Damage Damage { get; private set; }
    protected Enemy Target { get; set; }

    protected object Origin;
    private Vector3 _direction;

    public void Init(object origin, Damage damage, Enemy target)
    {
        Origin = origin;
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
        _direction = position - targetPosition;

        float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // move bullet towards the enemy
        transform.position = Vector3.MoveTowards(position, targetPosition, speed * Time.fixedDeltaTime);

        // check if bullet reached the enemy
        if (IsGiveDamage(transform.position, Target.Position))
        {
            GiveDamage();
        }

        if (IsDestroy(transform.position, Target.Position))
        {
            Destroy(gameObject);
        }
    }

    protected virtual void GiveDamage()
    {
        Target.TakeDamage(Origin, Damage);
    }

    protected virtual bool IsGiveDamage(Vector3 position, Vector3 targetPosition)
    {
        return position == targetPosition;
    }

    protected virtual bool IsDestroy(Vector3 position, Vector3 targetPosition)
    {
        return position == targetPosition;
    }
}