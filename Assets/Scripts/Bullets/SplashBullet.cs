using System;
using UnityEngine;

namespace Bullets
{
    public class SplashBullet : BaseBullet
    {
        private const int EnemyLayerMask = 1 << 9;
        
        [SerializeField, Range(0.1f, 10.5f)] private float targetingRange = 1.5f;
        
        private readonly Collider2D[] _targetsBuffer = new Collider2D[128];
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, targetingRange);
        }
        
        protected override void GiveDamage()
        {
            var hits = Physics2D.OverlapCircleNonAlloc(transform.position, targetingRange, _targetsBuffer, EnemyLayerMask);

            for (var hit = 0; hit < Math.Min(hits, _targetsBuffer.Length); hit++)
            {
                var targetEnemy = _targetsBuffer[hit].GetComponent<Enemy>();
                targetEnemy.TakeDamage(Origin, Damage);
            }
        }
    }
}