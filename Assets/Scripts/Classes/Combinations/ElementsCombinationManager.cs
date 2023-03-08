using Classes.Elements;
using Interfaces.ObjectAbilities;

namespace Classes.Combinations
{
    public class ElementsCombinationManager
    {
        private readonly ICanCombineElement _owner;
        private ElementsManager ElementsManager => _owner.ElementsManager;
        private EffectsManager EffectsManager => _owner.EffectsManager;
        private ElementCombinationList ElementCombinationList => _owner.ElementCombinationList;

        public ElementsCombinationManager(ICanCombineElement owner)
        {
            _owner = owner;
        }

        public void Update()
        {
            var combination = CheckForCombination();

            if (combination == null)
            {
                return;
            }

            foreach (var element in combination.Elements)
            {
                ElementsManager.RemoveElement(element);
            }

            EffectsManager.AddEffect(combination.Effect);
        }

        // check for combination and apply effects
        public ElementCombination CheckForCombination()
        {
            if (ElementCombinationList == null)
            {
                return null;
            }

            var combination = ElementCombinationList.GetCombination(ElementsManager.Elements);

            return combination;
        }
    }
}