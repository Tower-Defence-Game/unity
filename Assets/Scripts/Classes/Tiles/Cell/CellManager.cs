using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Classes.Tiles.Cell
{
    public class CellManager
    {
        private readonly GameObject _cellPrefab;
        private readonly GameObject _cellsContent;
        private readonly Dictionary<BaseTower, Cell> _cellByTower = new();
        private readonly UnityAction<BaseTower> _startPlacing;

        public CellManager(GameObject cellPrefab, GameObject cellsContent, UnityAction<BaseTower> startPlacing)
        {
            _cellPrefab = cellPrefab;
            _cellsContent = cellsContent;
            _startPlacing = startPlacing;
        }

        public void GenerateCell(TowerWithCount towerWithCount)
        {
            var tower = towerWithCount.Tower;
            var cell = new Cell(_cellPrefab, _cellsContent, towerWithCount);
            
            // Take tower
            cell.AddOnClickListener(() =>
            {
                if (cell.Count <= 0) return;

                cell.Count--;
                if (cell.Count <= 0) cell.Interactable = false;

                var towerObject = Object.Instantiate(tower);
                _cellByTower[towerObject] = cell;
                _startPlacing(towerObject);
            });
        }

        public void AddTower(BaseTower tower)
        {
            var cell = _cellByTower[tower];
            cell.Count += 1;
            cell.Interactable = true;
        }
    }
}