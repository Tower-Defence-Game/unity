using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;


public class EnemyDetector : MonoBehaviour
{
    /**
     * Предполагается, что все враги будут находиться на 9-ом слое :)
     */
    private const int EnemyLayerMask = 1 << 9;
    
    [SerializeField, Range(1.5f, 10.5f)]
    private float targetingRange = 1.5f;

    private Collider2D[] _targetsBuffer = new Collider2D[1];
    [CanBeNull] public Enemy TargetEnemy { get; private set; }

    private void FixedUpdate()
    {
        AcquireTarget();
    }
    
    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.position;
        Gizmos.DrawWireSphere(position, targetingRange);
        
        if (TargetEnemy != null) {
            Gizmos.DrawLine(position, TargetEnemy.Position);
        }
    }

    private bool AcquireTarget()
    {
        if (TargetEnemy != null && Vector3.Distance(transform.position, TargetEnemy.Position) < targetingRange)
        {
            return true;
        }
        
        int hits = Physics2D.OverlapCircleNonAlloc(transform.position, targetingRange, _targetsBuffer, EnemyLayerMask);

        if (hits == 0)
        {
            TargetEnemy = null;
            return false;
        }
        
        TargetEnemy = _targetsBuffer[0].GetComponent<Enemy>();

        Debug.Assert(TargetEnemy != null, "TargetEnemy must be Enemy!");
        return true;
    } 
}
