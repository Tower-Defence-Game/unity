using System.Collections.Generic;
using Interfaces.ObjectProperties;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerMapManager : MonoBehaviour, IHavePreStart
{
    [SerializeField] private TowerMapDrawer towerMapDrawer;
    [SerializeField] private TowerMapStander towerMapStander;
    [SerializeField] private List<TowerWithCount> towers;
    [SerializeField] private GameObject content;
    [SerializeField] private GameObject hideWhenPlacing;
    [SerializeField] private GameObject itemPrefab;
    private bool AfterStartDone { get; set; }
    private bool AreaHided { get; set; }

    private void Start()
    {
        FillContentUiByTowers();
    }

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

            HideArea();
            
            if (!IsMouseClicked()) return;
            var tilePosition = towerMapStander.GetTilePosition(globalMousePosition);
            var tower = towerMapStander.GetTower(tilePosition);

            if (tower == null) return;
            
            towerMapStander.DeleteTowerStandings(tower);
            towerMapStander.PickedTower = tower;
            towerMapDrawer.FlyingTower = tower;

            return;
        }

        ShowArea();

        towerMapStander.UpdatePosition(globalMousePosition);

        var available = towerMapStander.IsTileAvailable();
        towerMapDrawer.DrawPreTower(towerMapStander.GetTowerCoords(), available, towerMapStander.PickedTower);

        if (IsMouseClicked() && available) towerMapStander.PutTower();
    }

    public bool IsLevelStarted { get; set; } = false;

    public void StartPlacing(BaseTower towerPrefab)
    {
        if (towerMapDrawer.FlyingTower != null) towerMapDrawer.DestroyPreTower();
        towerMapStander.PickedTower = towerPrefab;
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

    private bool IsMouseClicked()
    {
        return Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject();
    }

    private void FillContentUiByTowers()
    {
        foreach (var towerWithCount in towers)
        {
            var tower = towerWithCount.Tower;
            
            var cell = Instantiate(itemPrefab, content.transform);
            
            var image = cell.GetComponent<Image>();
            var spriteRenderer = tower.GetComponentInChildren<SpriteRenderer>();
            image.sprite = spriteRenderer.sprite;
            image.color = spriteRenderer.color;

            var text = cell.GetComponentInChildren<TextMeshProUGUI>();
            text.text = towerWithCount.Count.ToString();
            
            cell.transform.SetParent(content.transform, false);

            cell.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (towerWithCount.Count <= 0) return;
                
                towerWithCount.Count--;
                if (towerWithCount.Count <= 0) cell.GetComponent<Button>().interactable = false;
                
                text.text = towerWithCount.Count.ToString();
                
                StartPlacing(tower);
            });
        }
    }
}