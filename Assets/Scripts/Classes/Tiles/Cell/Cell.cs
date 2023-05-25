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
        private readonly TextMeshProUGUI _countText;
        private readonly Button _button;
        private int _count;

        public int Count
        {
            get => int.Parse(_countText.text);
            set => _countText.text = value.ToString();
        }

        public Sprite Sprite { set => _image.sprite = value; }

        public Color Color { set => _image.color = value; }

        public bool Interactable { set => _button.interactable = value; }

        private Cell(GameObject cellPrefab, GameObject cellsContent)
        {
            _gameObject = Object.Instantiate(cellPrefab, cellsContent.transform);
            _gameObject.transform.SetParent(cellsContent.transform, false);

            _image = _gameObject.GetComponent<Image>();
            _countText = _gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _button = _gameObject.GetComponent<Button>();
        }

        public Cell(GameObject cellPrefab, GameObject cellsContent, TowerWithCount towerWithCount) 
            : this(cellPrefab, cellsContent)
        {
            var spriteRenderer = towerWithCount.Tower.GetComponentInChildren<SpriteRenderer>();
            
            Sprite = spriteRenderer.sprite;
            Color = spriteRenderer.color;
            Count = towerWithCount.Count;
        }

        public void AddOnClickListener(UnityAction call)
        {
            _button.onClick.AddListener(call);
        }
    }
}