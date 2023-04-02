using System;
using UnityEngine;

[Serializable]
public class TowerWithCount
{
    public BaseTower Tower
    {
        get => tower;
        set => tower = value;
    }

    public int Count
    {
        get => count;
        set => count = value;
    }

    [SerializeField] private BaseTower tower;
    [SerializeField] private int count;
}