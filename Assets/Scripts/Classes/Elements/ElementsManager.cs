using System.Collections.Generic;
using System.Linq;
using Interfaces.ObjectAbilities;
using Interfaces.ObjectProperties;
using JetBrains.Annotations;

namespace Classes.Elements
{
    public class ElementsManager
    {
        private readonly List<Element> _elements = new();
        private readonly ICanHaveElements _owner;
        public IReadOnlyList<Element> Elements => _elements;
        public IReadOnlyList<Element> ElementsWithoutOwner => _elements.Where(e => e != OwnerElement).ToList();

        [CanBeNull]
        public Element OwnerElement => _owner is IHaveElement ownerWithElement ? ownerWithElement.Element : null;

        public ElementsManager(ICanHaveElements owner)
        {
            _owner = owner;
            ResetElements();
        }

        public void AddElement(Element element)
        {
            if (element == null || element == OwnerElement)
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
            if (element == OwnerElement)
            {
                return;
            }

            _elements.Remove(element);
        }

        public void ResetElements()
        {
            _elements.Clear();

            if (OwnerElement != null)
            {
                _elements.Add(OwnerElement);
            }
        }
    }
}