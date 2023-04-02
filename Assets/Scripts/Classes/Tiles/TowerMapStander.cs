using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

[Serializable]
public class TowerMapStander
{
    // Tilemap, где есть только тайлы, на которые можно ставить башни
    [Tooltip("Tilemap, где есть только тайлы, на которые можно ставить башни")] [SerializeField]
    private Tilemap tilemap;

    public Tilemap Tilemap => tilemap;
    public BaseTower PickedTower { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerStands { get; set; } = new();
    public Vector3 CenteringVector => PickedTower == null ? 
        Vector3.zero : new Vector3(PickedTower.Size.x / 2f - 0.5f, PickedTower.Size.y / 2f - 0.5f);

    public Vector3Int SelectedPosition { get; set; }

    public void UpdatePosition(Vector3 globalMousePosition)
    {
        SelectedPosition = GetTilePosition(globalMousePosition);
    }

    public Vector3Int GetTilePosition(Vector3 globalMousePosition)
    {
        Vector2 mousePosition = globalMousePosition - CenteringVector;
        return tilemap.WorldToCell(mousePosition);
    }

    public Vector3 GetTowerCoords()
    {
        return GetTowerCoords(SelectedPosition);
    }

    public Vector3 GetTowerCoords(Vector3Int tilePosition)
    {
        return tilemap.GetCellCenterWorld(tilePosition) + CenteringVector;
    }

    private bool IsTileForTower(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            if (tilemap.GetTile(tilePosition + new Vector3Int(x, y, 0)) == null)
                return false;

        return true;
    }

    private bool IsTowerStand(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
        {
            if (GetTower(tilePosition + new Vector3Int(x, y, 0)) != null) return true;
        }

        return false;
    }
    
    public BaseTower GetTower(Vector3Int tilePosition)
    {
        TowerStands.TryGetValue(tilePosition, out var tower);
        return tower;
    }

    public void DeleteTowerStandings(BaseTower towerToDelete)
    {
        foreach (var tower in 
                 TowerStands.Where(x => x.Value == towerToDelete).ToList())
        {
            TowerStands.Remove(tower.Key);
        }
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

    public void PutTower(BaseTower tower, Vector3Int tilePosition)
    {
        TowerStands[tilePosition] = Object.Instantiate(tower, GetTowerCoords(tilePosition), Quaternion.identity);
        TowerStands[tilePosition].SetAlpha(1.0f);
        
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            TowerStands[tilePosition + new Vector3Int(x, y, 0)] = TowerStands[tilePosition];
        PickedTower = null;
    }


    public void HideTilemap()
    {
        tilemap.GetComponent<TilemapRenderer>().enabled = false;
    }
}