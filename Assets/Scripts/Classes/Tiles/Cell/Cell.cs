using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Classes.Tiles.Cell
{
    public class Cell
    {
        private readonly GameObject _gameObject;
        private readonly Image _image;
        private readonly Button _button;
        
        private readonly TextMeshProUGUI _countText;
        private readonly TextMeshProUGUI _starsText;

        public int Count
        {
            get => int.Parse(_countText.text);
            set => _countText.text = value.ToString();
        }

        public int Stars
        {
            get => int.Parse(_starsText.text);
            set => _starsText.text = value.ToString();
        }

        public Sprite Sprite { set => _image.sprite = value; }

        public Color Color { set => _image.color = value; }

        public bool Interactable
        {
            get => _button.interactable;
            set => _button.interactable = value;
        }

        private Cell(GameObject cellPrefab, GameObject cellsContent)
        {
            _gameObject = Object.Instantiate(cellPrefab, cellsContent.transform);
            _gameObject.transform.SetParent(cellsContent.transform, false);

            _image = _gameObject.GetComponent<Image>();
            _button = _gameObject.GetComponent<Button>();
            
            var texts = _gameObject.GetComponentsInChildren<TextMeshProUGUI>();
            _countText = texts[0];
            _starsText = texts[1];
        }

        public Cell(GameObject cellPrefab, GameObject cellsContent, TowerWithCount towerWithCount) 
            : this(cellPrefab, cellsContent)
        {
            var tower = towerWithCount.Tower;
            var spriteRenderer = tower.GetComponentInChildren<SpriteRenderer>();
            
            Sprite = spriteRenderer.sprite;
            Color = spriteRenderer.color;
            Count = towerWithCount.Count;
            Stars = tower.Stars;
        }

        public void AddOnClickListener(UnityAction call)
        {
            _button.onClick.AddListener(call);
        }
    }
}