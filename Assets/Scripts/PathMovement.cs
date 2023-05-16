using System;
using System.Collections.Generic;
using System.Linq;
using Interfaces.ObjectProperties;
using UnityEngine;

[RequireComponent(typeof(IHaveSpeed))]
public class PathMovement : MonoBehaviour
{
    [SerializeField] private GameObject path;
    private IHaveSpeed _speedObject;
    private List<Transform> _pathNodes;
    private Animator _animator;
    private int _currentNode;
    private float _transitionDuration;
    private static readonly int Direction = Animator.StringToHash("direction");
    private bool IsReachedTheEnd => _pathNodes == null || _currentNode >= _pathNodes.Count;

    void Start()
    {
        _pathNodes = path.GetComponentsInChildren<Transform>().ToList();
        _speedObject = GetComponent<IHaveSpeed>();

        // GetComponentsInChildren может возвращать также объект самого родителя, так что тут нужна такая проверка
        if (_pathNodes[0].gameObject == path)
        {
            _pathNodes.RemoveAt(0);
        }

        gameObject.transform.position = _pathNodes[0].transform.position;

        _animator = GetComponent<Animator>();
    }

    private void OnDrawGizmosSelected()
    {
        if (!IsReachedTheEnd)
        {
            Gizmos.DrawLine(transform.position, _pathNodes[_currentNode].position);
        }
    }

    void Update()
    {
        if (IsReachedTheEnd)
        {
            return;
        }

        var speed = _speedObject.Speed * Time.deltaTime;
        var leftMovement = speed;

        do
        {
            var currentPosition = gameObject.transform.position;
            var currentDestination = _pathNodes[_currentNode].position;
            var currentDistance = Vector3.Distance(currentPosition, currentDestination);

            Vector3 direction = Vector3.Normalize(currentDestination - currentPosition);

            if (currentDistance < leftMovement)
            {
                gameObject.transform.position = currentDestination;
                leftMovement -= currentDistance;
                _currentNode++;
            }
            else
            {
                gameObject.transform.position += direction * leftMovement;
                leftMovement = 0;
            }

            if (_animator is null) continue;

            if (Math.Abs(direction.x) > Math.Abs(direction.y))
            {
                _animator.SetInteger(Direction, direction.x > 0 ? 1 : 3);
            }
            else
            {
                _animator.SetInteger(Direction, direction.y > 0 ? 0 : 2);
            }
        } while (leftMovement > 0 && !IsReachedTheEnd);
    }
}