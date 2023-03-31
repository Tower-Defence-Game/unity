using Interfaces.ObjectProperties;
using UnityEngine;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    [SerializeField] private TowerMapDrawer towerMapDrawer;
    [SerializeField] private TowerMapStander towerMapStander;
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

        if (towerMapStander.PickedTower == null)
        {
            towerMapDrawer.DestroyPreTower();
            return;
        }

        var globalMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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