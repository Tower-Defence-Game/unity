using Interfaces.ObjectProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    // Tilemap, где есть только тайлы, на которые можно ставить башни
    [Tooltip("Tilemap, где есть только тайлы, на которые можно ставить башни")][SerializeField] 
    private Tilemap tilemap;
    [SerializeField] private Color availableColor = new Color(0f, 1f, 0f, 0.5f);
    [SerializeField] private Color unavailableColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private GameObject availability;
    [SerializeField] private float flyingTowerAlpha = 0.85f;
    public Tilemap Tilemap => tilemap;
    public BaseTower PickedTower { get; set; }
    public BaseTower FlyingTower { get; set; }
    public GameObject FlyingAvailability { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerStands { get; set; } = new();
    public bool IsLevelStarted { get; set; } = false;
    public Vector3 CenteringVector => new Vector3(PickedTower.Size.x / 2f - 0.5f, PickedTower.Size.y / 2f - 0.5f);
    public bool AfterStartDone { get; set; } = false;

    public void StartPlacing(BaseTower towerPrefab)
    {
        if (FlyingTower != null) DestroyPreTower();
        PickedTower = towerPrefab;
    }

    Vector3 GetTowerCoords(Vector3Int tilePosition)
    {
        return tilemap.GetCellCenterWorld(tilePosition) + CenteringVector;
    }

    bool IsTileForTower(Vector3Int tilePosition)
    {
        bool result = true;
        for(int x = 0; x < PickedTower.Size.x; x++)
        {
            for (int y = 0; y < PickedTower.Size.y; y++)
            {
                result = result && tilemap.GetTile(tilePosition + new Vector3Int(x, y, 0)) != null;
                if (!result) break;
            }
        }
        return result;
    }

    bool IsTowerStand(Vector3Int tilePosition)
    {
        bool result = false;
        for (int x = 0; x < PickedTower.Size.x; x++)
        {
            for (int y = 0; y < PickedTower.Size.y; y++)
            {
                TowerStands.TryGetValue(tilePosition + new Vector3Int(x, y, 0), out var tower);
                result = result || tower != null;
                if (result) break;
            }
        }
        return result;
    }

    bool IsTileAvailable(Vector3Int tilePosition)
    {
        return IsTileForTower(tilePosition) && !IsTowerStand(tilePosition);
    }

    void PutTower(BaseTower tower, Vector3Int tilePosition)
    {
        TowerStands[tilePosition] = Instantiate(tower, GetTowerCoords(tilePosition), Quaternion.identity);
        for (int x = 0; x < PickedTower.Size.x; x++)
        {
            for (int y = 0; y < PickedTower.Size.y; y++)
            {
                TowerStands[tilePosition + new Vector3Int(x, y, 0)] = TowerStands[tilePosition];
            }
        }
    }

    void DrawPreTower(Vector3Int tilePosition, bool available)
    {
        if (FlyingTower == null)
        {
            FlyingTower = Instantiate(PickedTower);
            FlyingAvailability = Instantiate(availability);
        }
        FlyingTower.transform.position = GetTowerCoords(tilePosition);
        var tempColor = FlyingTower.GetComponentInChildren<SpriteRenderer>().color;
        tempColor.a = flyingTowerAlpha;
        FlyingTower.GetComponentInChildren<SpriteRenderer>().color = tempColor;

        FlyingAvailability.transform.localScale = new Vector3(PickedTower.Size.x, PickedTower.Size.y, 0);
        FlyingAvailability.transform.position = FlyingTower.transform.position;

        var spriteRenderer = FlyingAvailability.GetComponentInChildren<SpriteRenderer>();

        spriteRenderer.color = available ? availableColor : unavailableColor;
    }

    void DestroyPreTower()
    {
        if (FlyingTower == null) return;
        
        Destroy(FlyingTower.gameObject);
        FlyingTower = null;
        Destroy(FlyingAvailability);
        FlyingAvailability = null;
    }

    void Update()
    {
        if (AfterStartDone) return;
        
        if (IsLevelStarted)
        {
            tilemap.GetComponent<TilemapRenderer>().sortingOrder = -1;
            AfterStartDone = true;
            DestroyPreTower();
            return;
        }
        
        if (PickedTower == null)
        {
            DestroyPreTower();
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - CenteringVector;

        Vector3Int selectedPosition = tilemap.WorldToCell(mousePosition);
        
        var available = IsTileAvailable(selectedPosition);
        DrawPreTower(selectedPosition, available);

        if (Input.GetMouseButtonDown(0) && available)
        {
            PutTower(PickedTower, selectedPosition);
        }
    }
}
