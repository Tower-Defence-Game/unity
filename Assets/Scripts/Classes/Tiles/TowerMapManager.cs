using Interfaces.ObjectProperties;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    // Tilemap, где есть только тайлы, на которые можно ставить башни
    [Tooltip("Tilemap, где есть только тайлы, на которые можно ставить башни")][SerializeField] 
    private Tilemap tilemap;
    [SerializeField] private Color aviableColor = Color.green;
    [SerializeField] private Color unaviableColor = Color.red;
    public Tilemap Tilemap => tilemap;
    public BaseTower PickedTower { get; set; }
    public BaseTower FlyingTower { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerStands { get; set; } = new();
    public bool IsLevelStarted { get; set; } = false;

    public void StartPlacing(BaseTower towerPrefab)
    {
        if (FlyingTower != null) DestroyPreTower();
        PickedTower = towerPrefab;
    }

    Vector3 GetTowerCoords(Vector3Int tilePosition)
    {
        return tilemap.GetCellCenterWorld(tilePosition);
    }

    bool IsTileForTower(Vector3Int tilePosition)
    {
        return tilemap.GetTile(tilePosition) != null;
    }

    bool IsTowerStand(Vector3Int tilePosition)
    {
        BaseTower result = null;
        TowerStands.TryGetValue(tilePosition, out result);
        return result != null;
    }

    bool IsTileAvailable(Vector3Int tilePosition)
    {
        return IsTileForTower(tilePosition) && !IsTowerStand(tilePosition);
    }

    void PutTower(BaseTower tower, Vector3Int tilePosition)
    {
        if(IsTowerStand(tilePosition))
        {
            print("Башня уже стоит тут.");
            return;
        }

        TowerStands[tilePosition] = Instantiate(tower, GetTowerCoords(tilePosition), Quaternion.identity);
    }

    void DrawPreTower(Vector3Int tilePosition, bool available)
    {
        if (FlyingTower == null)
        {
            FlyingTower = Instantiate(PickedTower);
        }
        FlyingTower.transform.position = GetTowerCoords(tilePosition);

        var spriteRenderer = FlyingTower.GetComponent<SpriteRenderer>();

        if (available)
        {
            spriteRenderer.color = aviableColor;
        }
        else
        {
            spriteRenderer.color = unaviableColor;
        }
    }

    void DestroyPreTower()
    {
        if (FlyingTower != null)
        {
            Destroy(FlyingTower.gameObject);
            FlyingTower = null;
        }
    }

    void Update()
    {
        if(IsLevelStarted || PickedTower == null)
        {
            DestroyPreTower();
            return;
        }

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int selectedPosition = tilemap.WorldToCell(mousePosition);
        
        var available = IsTileAvailable(selectedPosition);
        DrawPreTower(selectedPosition, available);

        if (Input.GetMouseButtonDown(0) && available)
        {
            PutTower(PickedTower, selectedPosition);
        }
    }
}
