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
    private readonly Dictionary<BaseTower, GameObject> _towerCountText = new();
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
            towerMapDrawer.DestroyPreTower();
            HideArea();

            if (IsMouseClicked()) EditTower(globalMousePosition);
            return;
        }

        ShowArea();

        towerMapStander.UpdatePosition(globalMousePosition);
        var available = towerMapStander.IsTileAvailable();

        towerMapDrawer.DrawPreTower(towerMapStander.GetTowerCoords(), available, towerMapStander.PickedTower);

        if (IsMouseClicked() && available) PutTower();
    }

    public bool IsLevelStarted { get; set; } = false;

    public void StartPlacing(BaseTower tower)
    {
        if (towerMapDrawer.FlyingTower != null) towerMapDrawer.DestroyPreTower();
        towerMapStander.PickedTower = tower;
    }

    public void DestroyPickedTower()
    {
        var cell = _towerCountText[towerMapStander.PickedTower];
        var countText = cell.GetComponentInChildren<TextMeshProUGUI>();
        countText.text = (int.Parse(countText.text) + 1).ToString();
        cell.GetComponent<Button>().interactable = true;

        towerMapDrawer.DestroyPreTower();
        towerMapStander.PickedTower = null;
    }

    public void EditTower(Vector3 globalMousePosition)
    {
        var tilePosition = towerMapStander.GetTilePosition(globalMousePosition);
        var tower = towerMapStander.GetTower(tilePosition);

        if (tower == null) return;

        towerMapStander.DeleteTowerStandings(tower);
        towerMapStander.PickedTower = tower;
        towerMapDrawer.FlyingTower = tower;
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
            var tower = towerWithCount.Tower;

            var cell = Instantiate(itemPrefab, content.transform);

            var image = cell.GetComponent<Image>();
            var spriteRenderer = tower.GetComponentInChildren<SpriteRenderer>();
            image.sprite = spriteRenderer.sprite;
            image.color = spriteRenderer.color;

            var countText = cell.GetComponentInChildren<TextMeshProUGUI>();
            countText.text = towerWithCount.Count.ToString();

            cell.transform.SetParent(content.transform, false);

            cell.GetComponent<Button>().onClick.AddListener(() =>
            {
                var count = int.Parse(countText.text);
                if (count <= 0) return;

                count--;
                if (count <= 0) cell.GetComponent<Button>().interactable = false;

                countText.text = count.ToString();

                var towerObject = Instantiate(tower);
                _towerCountText[towerObject] = cell;
                StartPlacing(towerObject);
            });
        }
    }
}