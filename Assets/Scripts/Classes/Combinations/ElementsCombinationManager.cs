using System.Collections.Generic;
using System.Linq;
using Classes.Elements;
using Interfaces.ObjectAbilities;

namespace Classes.Combinations
{
    public class ElementsCombinationManager
    {
        private readonly List<Element> _elements = new();
        private readonly ICanCombineElement _owner;

        public ElementsCombinationManager(ICanCombineElement owner)
        {
            _owner = owner;
            ResetElements();
        }

        public void AddElement(Element element)
        {
            if (element == _owner.Element)
            {
                return;
            }

            if (_elements.Any(e => e == element))
            {
                return;
            }

            _elements.Add(element);
        }

        public void RemoveElement(Element element)
        {
            if (element == _owner.Element)
            {
                return;
            }

            _elements.Remove(element);
        }

        public void ResetElements()
        {
            _elements.Clear();
            _elements.Add(_owner.Element);
        }

        public void Update()
        {
            if (_owner is not ICanHaveEffects effectHolder)
            {
                return;
            }

            var combination = CheckForCombination();

            if (combination == null)
            {
                return;
            }

            foreach (var element in combination.Elements)
            {
                RemoveElement(element);
            }

            effectHolder.EffectManager.AddEffect(combination.Effect);
        }

        // check for combination and apply effects
        public ElementCombination CheckForCombination()
        {
            var combination = _owner.ElementCombinationList.GetCombination(_elements);

            return combination;
        }
    }
}