using System;
using UnityEngine;

[Serializable]
public struct SpawnOptions
{
    [SerializeField] private GameObject _objectToSpawn;
    [SerializeField] private int _count;
    [SerializeField] private float _wait;
    [SerializeField] private float _every;

    public GameObject ObjectToSpawn => _objectToSpawn;
    public int Count => _count;
    public float Wait => _wait;
    public float Every => _every;
}
