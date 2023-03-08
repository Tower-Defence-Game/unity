using System;
using System.Collections.Generic;
using System.Linq;
using Classes.Elements;
using JetBrains.Annotations;
using UnityEngine;

namespace Classes.Combinations
{
    [CreateAssetMenu(fileName = "ElementCombination", menuName = "Elements/Combination list", order = 1)]
    public class ElementCombinationList : ScriptableObject
    {
        [SerializeField] private List<ElementCombination> combinations;

        [CanBeNull]
        public ElementCombination GetCombination(IReadOnlyList<Element> currentElements)
        {
            return combinations.FirstOrDefault(c => c.IsMatch(currentElements));
        }

        private void OnValidate()
        {
            if (combinations.Count < 1)
            {
                throw new ArgumentException("Element combination list must have at least 1 combination");
            }
        }
    }
}