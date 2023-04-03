using System;
using UnityEngine;
using Object = UnityEngine.Object;

[Serializable]
public class TowerMapDrawer
{
    [SerializeField] private Color availableColor = new(0f, 1f, 0f, 0.5f);
    [SerializeField] private Color unavailableColor = new(1f, 0f, 0f, 0.5f);
    [SerializeField] private GameObject availability;
    [SerializeField] private float flyingTowerAlpha;
    private BaseTower _flyingTower;

    public BaseTower FlyingTower
    {
        get => _flyingTower;
        set
        {
            _flyingTower = value;
            if (_flyingTower == null) return;

            _flyingTower.SetAlpha(flyingTowerAlpha);
        }
    }

    public GameObject FlyingAvailability { get; set; }

    public TowerMapDrawer(GameObject availability, float flyingTowerAlpha, BaseTower flyingTower, GameObject flyingAvailability)
    {
        this.availability = availability;
        this.flyingTowerAlpha = 0.85f;
        FlyingTower = flyingTower;
        FlyingAvailability = flyingAvailability;
    }

    public void DrawPreTower(Vector3 towerCoords, bool available, BaseTower pickedTower)
    {
        if (FlyingTower == null)
        {
            FlyingTower = pickedTower;
        }

        if (FlyingAvailability == null)
        {
            FlyingAvailability = Object.Instantiate(availability);
        }

        Transform towerTransform;
        (towerTransform = FlyingTower.transform).position = towerCoords;

        FlyingAvailability.transform.localScale = new Vector3(pickedTower.Size.x, pickedTower.Size.y, 0);
        FlyingAvailability.transform.position = towerTransform.position;
    
        var spriteRenderer = FlyingAvailability.GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.color = available ? availableColor : unavailableColor;
    }
    
    public void DestroyPreTower()
    {
        if (FlyingTower != null)
        {
            Object.Destroy(FlyingTower.gameObject);
            FlyingTower = null;
        }
        
        if (FlyingAvailability != null)
        {
            Object.Destroy(FlyingAvailability);
            FlyingAvailability = null;
        }
    }
}