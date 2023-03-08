using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Effects;
using Classes.Elements;
using UnityEngine;

namespace Classes.Combinations
{
    [CreateAssetMenu(fileName = "ElementCombination", menuName = "Elements/Combination", order = 0)]
    public class ElementCombination : ScriptableObject
    {
        [SerializeField] private List<Element> elements;
        [SerializeField] private AbstractEffect effect;

        public AbstractEffect Effect => effect;
        public List<Element> Elements => elements;

        public bool IsMatch(IReadOnlyList<Element> currentElements)
        {
            return elements.All(e => currentElements.Any(el => el == e));
        }

        private void OnValidate()
        {
            if (elements.Count < 2)
            {
                throw new ArgumentException("Element combination must have at least 2 elements");
            }

            if (elements.Any(e => e == null))
            {
                throw new ArgumentException("Element combination must not contain NoneElement");
            }

            // if elements contains two or more of the same element, throw an exception
            if (elements.Count != new HashSet<Element>(elements).Count)
            {
                throw new ArgumentException("Element combination must not contain duplicate elements");
            }

            if (effect == null)
            {
                throw new ArgumentException("Element combination must have an effect");
            }
        }
    }
}