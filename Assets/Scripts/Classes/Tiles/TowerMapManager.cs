using System.Collections.Generic;
using Interfaces.ObjectProperties;
using UnityEngine;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    [SerializeField] private TowerMapDrawer towerMapDrawer;
    [SerializeField] private TowerMapStander towerMapStander;
    [SerializeField] private List<TowerWithCount> towers;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject itemPrefab;
    private bool AfterStartDone { get; set; }

    private void Update()
    {
        if (AfterStartDone) return;

        if (IsLevelStarted)
        {
            towerMapStander.HideTilemap();
            towerMapDrawer.DestroyPreTower();
            AfterStartDone = true;
            return;
        }
        
        var globalMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (towerMapStander.PickedTower == null)
        {
            if(towerMapDrawer.FlyingTower != null) towerMapDrawer.DestroyPreTower();
            content.SetActive(true);
            
            if (!Input.GetMouseButtonDown(0)) return;
            var tilePosition = towerMapStander.GetTilePosition(globalMousePosition);
            var tower = towerMapStander.GetTower(tilePosition);

            if (tower == null) return;
            
            towerMapStander.DeleteTowerStandings(tower);
            towerMapStander.PickedTower = tower;
            towerMapDrawer.FlyingTower = tower;

            return;
        }

        content.SetActive(false);

        towerMapStander.UpdatePosition(globalMousePosition);

        var available = towerMapStander.IsTileAvailable();
        towerMapDrawer.DrawPreTower(towerMapStander.GetTowerCoords(), available, towerMapStander.PickedTower);

        if (Input.GetMouseButtonDown(0) && available) towerMapStander.PutTower();
    }

    public bool IsLevelStarted { get; set; } = false;

    public void StartPlacing(BaseTower towerPrefab)
    {
        if (towerMapDrawer.FlyingTower != null) towerMapDrawer.DestroyPreTower();
        towerMapStander.PickedTower = towerPrefab;
    }
}