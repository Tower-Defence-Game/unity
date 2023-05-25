using System.Collections.Generic;
using Classes.Tiles.Cell;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class TowerMapManager : MonoBehaviour
{
    [SerializeField] private TowerMapDrawer towerMapDrawer;
    [SerializeField] private TowerMapStander towerMapStander;
    [SerializeField] private List<TowerWithCount> towers;
    
    [FormerlySerializedAs("content")] [SerializeField] [Tooltip("Объект в ScrollArea")]
    private GameObject cellsContent;
    
    [SerializeField] [Tooltip("Объекты, которые нужно скрывать во время выставления башен")] 
    private GameObject hideWhenPlacing;
    
    [FormerlySerializedAs("itemPrefab")] [SerializeField] [Tooltip("Префаб ячейки для башни")]
    private GameObject cellPrefab;

    [SerializeField] [Tooltip("Максимальное кол-во звёзд, которое может быть на уровне")]
    private int maxStars = 4;
    
    [SerializeField] [Tooltip("Текст, где отображаются звёзды на уровне")]
    private TextMeshProUGUI starsText;
    
    private bool AfterStartDone { get; set; }
    private bool AreaHided { get; set; }

    private CellManager _cellManager;

    private void Start()
    {
        // Обязательно создавать перед FillContentUiByTowers
        _cellManager = new CellManager(cellPrefab, cellsContent, StartPlacing, maxStars, starsText);
        
        FillContentUiByTowers();
        StartManager.AddOnStart(OnStart);
    }

    private void Update()
    {
        if (AfterStartDone) return;

        var globalMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (towerMapStander.PickedTower == null)
        {
            towerMapDrawer.DestroyFlyingTower();
            HideArea();

            if (IsMouseClicked()) EditTower(globalMousePosition);
            return;
        }

        ShowArea();

        towerMapStander.UpdatePosition(globalMousePosition);
        var available = towerMapStander.IsTileAvailable();

        towerMapDrawer.DrawFlyingTower(towerMapStander.GetTowerCoords(), available, towerMapStander.PickedTower);

        if (IsMouseClicked() && available) PutTower();
    }
    
    public void OnStart()
    {
        towerMapStander.HideTilemap();
        towerMapDrawer.DestroyFlyingTower();
        AfterStartDone = true;
    }

    public void StartPlacing(BaseTower tower)
    {
        towerMapDrawer.DestroyFlyingTower();
        towerMapStander.PickedTower = tower;
    }

    public void DestroyPickedTower()
    {
        _cellManager.AddTower(towerMapStander.PickedTower);

        towerMapDrawer.DestroyFlyingTower();
        towerMapStander.PickedTower = null;
    }

    public void EditTower(Vector3 globalMousePosition)
    {
        var tilePosition = towerMapStander.GetTilePosition(globalMousePosition);
        var tower = towerMapStander.GetTower(tilePosition);

        if (tower == null) return;

        towerMapStander.DeleteTowerStandings(tower);
        towerMapStander.PickedTower = tower;
    }

    private void HideArea()
    {
        if (AreaHided) return;

        hideWhenPlacing.SetActive(true);
        AreaHided = true;
    }

    private void ShowArea()
    {
        if (!AreaHided) return;

        hideWhenPlacing.SetActive(false);
        AreaHided = false;
    }

    private void PutTower()
    {
        towerMapStander.PutTower(towerMapDrawer.FlyingTower);
        towerMapDrawer.FlyingTower = null;
    }

    private bool IsMouseClicked()
    {
        return Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
    }

    private void FillContentUiByTowers()
    {
        foreach (var towerWithCount in towers)
        {
            _cellManager.GenerateCell(towerWithCount);
        }
    }
}