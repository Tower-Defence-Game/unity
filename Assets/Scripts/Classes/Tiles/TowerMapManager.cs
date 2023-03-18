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
    [SerializeField] private GameObject aviability;
    public Tilemap Tilemap => tilemap;
    public BaseTower PickedTower { get; set; }
    public BaseTower FlyingTower { get; set; }
    public GameObject FlyingAviability { get; set; }
    public Dictionary<Vector3Int, BaseTower> TowerStands { get; set; } = new();
    public bool IsLevelStarted { get; set; } = false;
    public Vector3 CenteringVector
    {
        get
        {
            return new Vector3(PickedTower.Size.x / 2f - 0.5f, PickedTower.Size.y / 2f - 0.5f);
        }
    }

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
                BaseTower tower;
                TowerStands.TryGetValue(tilePosition + new Vector3Int(x, y, 0), out tower);
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
            FlyingAviability = Instantiate(aviability);

        }
        FlyingTower.transform.position = GetTowerCoords(tilePosition);
        var tempColor = FlyingTower.GetComponentInChildren<SpriteRenderer>().color;
        tempColor.a = 0.85f;
        FlyingTower.GetComponentInChildren<SpriteRenderer>().color = tempColor;

        FlyingAviability.transform.localScale = new Vector3(PickedTower.Size.x, PickedTower.Size.y, 0);
        FlyingAviability.transform.position = FlyingTower.transform.position;

        var spriteRenderer = FlyingAviability.GetComponentInChildren<SpriteRenderer>();

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
            Destroy(FlyingAviability);
            FlyingAviability = null;
        }
    }

    void Update()
    {
        if(IsLevelStarted || PickedTower == null)
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
