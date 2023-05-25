using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Classes.Tiles.Cell
{
    public class CellManager
    {
        private readonly GameObject _cellPrefab;
        private readonly GameObject _cellsContent;
        private readonly Dictionary<BaseTower, Cell> _cellByTower = new();
        private readonly List<Cell> _cells = new();
        private readonly UnityAction<BaseTower> _startPlacing;

        private readonly int _maxStars;
        private int _currentStars;
        private readonly TextMeshProUGUI _starsText;

        private int CurrentStars
        {
            get => _currentStars;
            set
            {
                _currentStars = value;
                UpdateStarsText();
            }
        }
        
        private string StarsText
        {
            get => _starsText.text;
            set => _starsText.text = value;
        }

        public CellManager(GameObject cellPrefab, GameObject cellsContent, UnityAction<BaseTower> startPlacing, 
            int maxStars, TextMeshProUGUI starsText)
        {
            _cellPrefab = cellPrefab;
            _cellsContent = cellsContent;
            _startPlacing = startPlacing;
            _starsText = starsText;
            
            _maxStars = maxStars;
            _currentStars = _maxStars;
            UpdateStarsText();
        }

        public void GenerateCell(TowerWithCount towerWithCount)
        {
            var tower = towerWithCount.Tower;
            var cell = new Cell(_cellPrefab, _cellsContent, towerWithCount);
            _cells.Add(cell);
            
            // Take tower
            cell.AddOnClickListener(() =>
            {
                if (cell.Count <= 0) return;

                cell.Count--;
                CurrentStars -= tower.Stars;
                HideOrShowCells();

                var towerObject = Object.Instantiate(tower);
                _cellByTower[towerObject] = cell;
                _startPlacing(towerObject);
            });
        }

        public void AddTower(BaseTower tower)
        {
            var cell = _cellByTower[tower];
            
            cell.Count += 1;
            CurrentStars += tower.Stars;
            HideOrShowCells();
        }

        private void HideOrShowCells()
        {
            foreach (var cell in _cells)
            {
                if (cell.Stars > _currentStars || cell.Count <= 0) cell.Interactable = false;
                else cell.Interactable = true;
            }
        }

        private void UpdateStarsText()
        {
            StarsText = $"{_currentStars}/{_maxStars}";
        }
    }
}