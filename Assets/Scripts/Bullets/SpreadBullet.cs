using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Elements;
using UnityEngine;

namespace Bullets
{
    public class SpreadBullet : BaseBullet
    {
        private const int EnemyLayerMask = 1 << 9;

        [SerializeField] private Element element;
        [SerializeField, Range(0.1f, 10.5f)] private float spreadRadius = 1.5f;
        
        private readonly Collider2D[] _targetsBuffer = new Collider2D[32];
        private readonly List<Enemy> _targets = new();

        private readonly Dictionary<Enemy, bool> _used = new();
        private int _targetIndex;
        
        protected override void GiveDamage()
        {
            Target.TakeDamage(Origin, Damage);
            _used[Target] = true;
            
            var hits = Physics2D.OverlapCircleNonAlloc(Target.transform.position, spreadRadius, _targetsBuffer, EnemyLayerMask);
            for (var hit = 0; hit < Math.Min(hits, _targetsBuffer.Length); hit++)
            {
                var targetEnemy = _targetsBuffer[hit].GetComponent<Enemy>();
                if (_used.ContainsKey(targetEnemy) || targetEnemy.ElementsManager.Elements.All(el => el != element)) continue;
                
                _targets.Add(targetEnemy);
            }
            
            NextTarget();
        }

        private void NextTarget()
        {
            if (_targetIndex >= _targets.Count) return;
            
            Target = _targets[_targetIndex];
            _targetIndex++;
        }
    }
}