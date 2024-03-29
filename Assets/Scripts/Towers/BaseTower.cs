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
    [SerializeField] private int stars = 1;

    private EnemyDetector _enemyDetector;
    private Animator _animator;
    private Enemy Enemy => _enemyDetector.TargetEnemy;

    private float _cooldownTimer = 0f;
    private static readonly int Shooting = Animator.StringToHash("shooting");
    private static readonly int Speed = Animator.StringToHash("speed");
    private const string ShootClipName = "Shoot";
    public Vector2Int Size => size;
    public int Stars => stars;

    public void SetAlpha(float alpha)
    {
        var tempColor = GetComponentInChildren<SpriteRenderer>().color;
        tempColor.a = alpha;
        GetComponentInChildren<SpriteRenderer>().color = tempColor;
    }

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
        _animator = GetComponent<Animator>();
        
        Debug.Assert(cooldown != 0, "Cooldown can't be 0.");
        
        var clips = _animator.runtimeAnimatorController.animationClips;
        var length = 1f;
        foreach (var clip in clips)
        {
            if (clip.name != ShootClipName) continue;
            length = clip.length;
        }
        _animator.SetFloat(Speed, 1f / length / cooldown);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Enemy == null)
        {
            _animator.SetBool(Shooting, false);
            return;
        }

        // rotate game object in 2D to the enemy
        Vector3 direction = Enemy.Position - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        turret.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (_cooldownTimer > 0)
        {
            _animator.SetBool(Shooting, false);
            _cooldownTimer -= Time.deltaTime;
        }

        PrepareShoot();
    }

    void PrepareShoot()
    {
        if (_animator is not null)
        {
            _animator.SetBool(Shooting, true);
        }
        else
        {
            Shoot();
        }
    }

    // Вызывается анимацией
    void Shoot()
    {
        var bulletInstance = Instantiate(bullet, turret.position, Quaternion.identity);
        var bulletScript = bulletInstance.GetComponent<IBullet>();

        Debug.Assert(bulletScript != null, "Bullet must implement IBullet interface!");

        bulletScript.Init(this, new Damage(element, damage), Enemy);
        _cooldownTimer = cooldown;
    }
}