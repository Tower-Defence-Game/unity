using System;
using UnityEngine;

[Serializable]
public struct SpawnOptions
{
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private int count;
    [SerializeField] private float wait;
    [SerializeField] private float every;

    public GameObject ObjectToSpawn => objectToSpawn;
    public int Count => count;
    public float Wait => wait;
    public float Every => every;
}
