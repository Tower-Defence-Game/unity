using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

[Serializable]
public class TowerMapStander
{
    [FormerlySerializedAs("tilemap")] [Tooltip("Tilemap, где есть только тайлы, на которые можно ставить башни")] [SerializeField]
    private Tilemap towerTilemap;
    public BaseTower PickedTower { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerByPosition { get; set; } = new();

    // magic vector, idk how it works, but it works
    public Vector3 CenteringVector => PickedTower == null
        ? Vector3.zero
        : new Vector3(PickedTower.Size.x / 2f - 0.5f, PickedTower.Size.y / 2f - 0.5f);

    public Vector3Int SelectedPosition { get; set; }

    public void UpdatePosition(Vector3 globalMousePosition)
    {
        SelectedPosition = GetTilePosition(globalMousePosition);
    }

    public Vector3Int GetTilePosition(Vector3 globalMousePosition)
    {
        Vector2 mousePosition = globalMousePosition - CenteringVector;
        return towerTilemap.WorldToCell(mousePosition);
    }

    public Vector3 GetTowerCoords()
    {
        return GetTowerCoords(SelectedPosition);
    }

    public Vector3 GetTowerCoords(Vector3Int tilePosition)
    {
        return towerTilemap.GetCellCenterWorld(tilePosition) + CenteringVector;
    }

    private bool IsTileForTower(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            if (towerTilemap.GetTile(tilePosition + new Vector3Int(x, y, 0)) == null)
                return false;

        return true;
    }

    private bool IsTowerStand(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            if (GetTower(tilePosition + new Vector3Int(x, y, 0)) != null)
                return true;

        return false;
    }

    public BaseTower GetTower(Vector3Int tilePosition)
    {
        TowerByPosition.TryGetValue(tilePosition, out var tower);
        return tower;
    }

    public void DeleteTowerStandings(BaseTower towerToDelete)
    {
        foreach (var tower in
                 TowerByPosition.Where(x => x.Value == towerToDelete).ToList())
            TowerByPosition.Remove(tower.Key);
    }

    public void DeleteTower(ref BaseTower towerToDelete)
    {
        DeleteTowerStandings(towerToDelete);
        Object.Destroy(towerToDelete);
        towerToDelete = null;
    }

    public bool IsTileAvailable()
    {
        return IsTileAvailable(SelectedPosition);
    }

    public bool IsTileAvailable(Vector3Int tilePosition)
    {
        return IsTileForTower(tilePosition) && !IsTowerStand(tilePosition);
    }

    public void PutTower()
    {
        PutTower(PickedTower, SelectedPosition);
    }

    public void PutTower(BaseTower tower)
    {
        PutTower(tower, SelectedPosition);
    }

    public void PutTower(BaseTower tower, Vector3Int tilePosition)
    {
        tower.transform.position = GetTowerCoords(tilePosition);
        TowerByPosition[tilePosition] = tower;
        TowerByPosition[tilePosition].SetAlpha(1.0f);

        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            TowerByPosition[tilePosition + new Vector3Int(x, y, 0)] = TowerByPosition[tilePosition];
        PickedTower = null;
    }


    public void HideTilemap()
    {
        towerTilemap.GetComponent<TilemapRenderer>().enabled = false;
    }
}