using Bullets;
using Classes.Damage;
using Classes.Elements;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(EnemyDetector))]
public class BaseTower : MonoBehaviour
{
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private float damage = 50f;
    [SerializeField] [CanBeNull] private Element element;
    [SerializeField] private Transform turret = default;
    [SerializeField] private Transform tower = default;
    [SerializeField] private BaseBullet bullet = default;
    [SerializeField] private Vector2Int size = Vector2Int.one;

    private EnemyDetector _enemyDetector;
    private Enemy Enemy => _enemyDetector.TargetEnemy;

    private float _cooldownTimer = 0f;
    public Vector2Int Size => size;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
        Gizmos.DrawCube(transform.position + new Vector3(0, 0, 0), new Vector3(size.x, size.y, .1f));
    }

    private void OnValidate()
    {
        if (turret == null)
        {
            turret = transform;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _enemyDetector = GetComponent<EnemyDetector>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Enemy == null)
        {
            return;
        }

        // rotate game object in 2D to the enemy
        Vector3 direction = Enemy.Position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        turret.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
            return;
        }

        _cooldownTimer = cooldown;
        Shoot();
    }

    void Shoot()
    {
        var bulletInstance = Instantiate(bullet, tower.position, Quaternion.identity);
        var bulletScript = bulletInstance.GetComponent<IBullet>();

        Debug.Assert(bulletScript != null, "Bullet must implement IBullet interface!");

        bulletScript.Init(this, new Damage(element, damage), Enemy);
    }
}