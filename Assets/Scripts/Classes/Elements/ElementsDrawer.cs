using System.Collections.Generic;
using System.Linq;
using Interfaces.ObjectAbilities;
using UnityEngine;

namespace Classes.Elements
{
    [RequireComponent(typeof(ICanHaveElements))]
    public class ElementsDrawer : MonoBehaviour
    {
        // get the ElementsManager from the GameObject
        [SerializeField] private SpriteRenderer objectSprite;
        [SerializeField] private float yOffset = 0.5f;
        [SerializeField] private float xOffset = 0.5f;

        [SerializeField] private bool drawOwnElement;

        // and draw each element sprite above the GameObject (for example Enemy)
        private ElementsManager _elementsManager;
        private readonly HashSet<KeyValuePair<Element, SpriteRenderer>> _elementsSpriteRenderers = new();

        private IEnumerable<Element> Elements =>
            drawOwnElement ? _elementsManager.Elements : _elementsManager.ElementsWithoutOwner;

        private void Start()
        {
            var elementsHolder = GetComponent<ICanHaveElements>();
            Debug.Assert(elementsHolder != null, "Game object must have ICanHaveElements implementation");

            _elementsManager = elementsHolder.ElementsManager;
        }

        private void Update()
        {
            // draw each element sprite above the GameObject
            // for example, if the GameObject has 2 elements, draw 2 sprites above the GameObject
            // if the GameObject has 3 elements, draw 3 sprites above the GameObject
            // etc.

            bool hasChanges = false;

            // filter out the elements drawers that are not in ElementsManager
            // and remove them from the HashSet (and destroy game objects)
            foreach (var pair in _elementsSpriteRenderers.Where(pair => Elements.All(element => element != pair.Key)))
            {
                _elementsSpriteRenderers.Remove(pair);
                Destroy(pair.Value.gameObject);
                hasChanges = true;
            }

            // add the new elements drawers to the HashSet that are not in the HashSet
            foreach (Element element in Elements.Where(element => element.Icon != null))
            {
                if (_elementsSpriteRenderers.Any(pair => pair.Key == element))
                {
                    continue;
                }

                var spriteRenderer = new GameObject().AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = element.Icon;
                spriteRenderer.transform.SetParent(transform);
                spriteRenderer.sortingOrder = objectSprite.sortingOrder;
                _elementsSpriteRenderers.Add(new KeyValuePair<Element, SpriteRenderer>(element, spriteRenderer));
                hasChanges = true;
            }

            if (!hasChanges || _elementsSpriteRenderers.Count == 0)
            {
                return;
            }

            float verticalOffset = objectSprite.size.y / 2 + yOffset;
            float horizontalOffset = _elementsSpriteRenderers.Sum(pair => pair.Value.size.x);
            horizontalOffset += xOffset * (_elementsSpriteRenderers.Count - 1);
            horizontalOffset /= -2;

            // set the position of each element sprite above the GameObject horizontally
            foreach (var pair in _elementsSpriteRenderers)
            {
                var size = pair.Value.size;
                pair.Value.transform.localPosition = new Vector3(horizontalOffset + size.x / 2, verticalOffset, 0);
                horizontalOffset += size.x + xOffset;
            }
        }
    }
}