using Classes.Combinations;
using Interfaces.ObjectProperties;

namespace Interfaces.ObjectAbilities
{
    public interface ICanCombineElement : IHaveElement
    {
        public ElementsCombinationManager ElementsCombinationManager { get; }
        public ElementCombinationList ElementCombinationList { get; }
    }
}