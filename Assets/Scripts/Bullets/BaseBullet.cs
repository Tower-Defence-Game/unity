using System.Collections;
using System.Collections.Generic;
using Bullets;
using Classes.Damage;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IBullet
{
    
    public IDamage Damage { get; private set; }
    public Enemy Target { get; private set; }
    
    [SerializeField]
    private float speed = 5f;
    
    public void Init(IDamage damage, Enemy target)
    {
        Damage = damage;
        Target = target;
    }
    
    void FixedUpdate()
    {
        var position = transform.position;
        Vector3 direction = position - Target.Position;
        
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        // move bullet towards the enemy
        position = Vector3.MoveTowards(position, Target.Position, speed * Time.fixedDeltaTime);
        transform.position = position;
        
        // check if bullet reached the enemy
        if (position == Target.Position)
        {
            Target.TakeDamage(Damage);
            Destroy(gameObject);
        }
    }
}
