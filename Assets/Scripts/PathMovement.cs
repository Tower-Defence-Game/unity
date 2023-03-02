using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject path;
    private Transform[] _pathNodes;
    private float _timer;
    private Vector3 _currentDestination;
    private int _currentNode;
    private Vector3 _startPosition;
    private float _transitionDuration;

    void Start()
    {
        _pathNodes = path.GetComponentsInChildren<Transform>();

        // GetComponentsInChildren может возвращать также объект самого родителя, так что тут нужна такая проверка
        _currentNode = _pathNodes[0].gameObject.GetInstanceID() == path.GetInstanceID() ? 1 : 0;
        ProcessNextNode();
    }

    void ProcessNextNode()
    {
        _timer = 0;
        _currentDestination = _pathNodes[_currentNode].position;
        _startPosition = gameObject.transform.position;

        float distance = Vector3.Distance(_startPosition, _currentDestination);
        _transitionDuration = distance / moveSpeed;
    }

    bool IsNodeComplete()
    {
        return gameObject.transform.position != _currentDestination;
    }

    void Update()
    {
        _timer += Time.deltaTime;
        if(IsNodeComplete())
        {
            float movementProgress = _timer / _transitionDuration;
            gameObject.transform.position = Vector3.Lerp(_startPosition, _currentDestination, movementProgress);
        }
        else
        {
            if(_currentNode < _pathNodes.Length - 1)
            {
                _currentNode++;
                ProcessNextNode();
            }
        }
    }
}
