using System.Collections.Generic;
using Interfaces.ObjectProperties;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    // Tilemap, где есть только тайлы, на которые можно ставить башни
    [Tooltip("Tilemap, где есть только тайлы, на которые можно ставить башни")] [SerializeField]
    private Tilemap tilemap;
    [SerializeField] private TowerMapDrawer towerMapDrawer;
    public Tilemap Tilemap => tilemap;
    public BaseTower PickedTower { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerStands { get; set; } = new();
    public Vector3 CenteringVector => new(PickedTower.Size.x / 2f - 0.5f, PickedTower.Size.y / 2f - 0.5f);
    public bool AfterStartDone { get; set; }

    private void Update()
    {
        if (AfterStartDone) return;

        if (IsLevelStarted)
        {
            HideTilemap();
            AfterStartDone = true;
            towerMapDrawer.DestroyPreTower();
            return;
        }

        if (PickedTower == null)
        {
            towerMapDrawer.DestroyPreTower();
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - CenteringVector;

        var selectedPosition = tilemap.WorldToCell(mousePosition);

        var available = IsTileAvailable(selectedPosition);
        towerMapDrawer.DrawPreTower(GetTowerCoords(selectedPosition), available, PickedTower);

        if (Input.GetMouseButtonDown(0) && available) PutTower(PickedTower, selectedPosition);
    }

    public bool IsLevelStarted { get; set; } = false;

    public void StartPlacing(BaseTower towerPrefab)
    {
        if (towerMapDrawer.FlyingTower != null) towerMapDrawer.DestroyPreTower();
        PickedTower = towerPrefab;
    }

    private Vector3 GetTowerCoords(Vector3Int tilePosition)
    {
        return tilemap.GetCellCenterWorld(tilePosition) + CenteringVector;
    }

    private bool IsTileForTower(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
        {
            if (tilemap.GetTile(tilePosition + new Vector3Int(x, y, 0)) == null)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsTowerStand(Vector3Int tilePosition)
    {
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
        {
            TowerStands.TryGetValue(tilePosition + new Vector3Int(x, y, 0), out var tower);
            if (tower != null) return true;
        }

        return false;
    }

    private bool IsTileAvailable(Vector3Int tilePosition)
    {
        return IsTileForTower(tilePosition) && !IsTowerStand(tilePosition);
    }

    private void PutTower(BaseTower tower, Vector3Int tilePosition)
    {
        TowerStands[tilePosition] = Instantiate(tower, GetTowerCoords(tilePosition), Quaternion.identity);
        for (var x = 0; x < PickedTower.Size.x; x++)
        for (var y = 0; y < PickedTower.Size.y; y++)
            TowerStands[tilePosition + new Vector3Int(x, y, 0)] = TowerStands[tilePosition];
    }

    private void HideTilemap()
    {
        tilemap.GetComponent<TilemapRenderer>().sortingOrder = -1;
    }
}