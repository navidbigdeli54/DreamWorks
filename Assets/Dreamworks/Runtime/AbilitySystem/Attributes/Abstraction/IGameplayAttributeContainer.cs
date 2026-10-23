using DreamMachineGameStudio.DreamWorks.Foundation;

namespace DreamMachineGameStudio.DreamWorks.AbilitySystem.Attributes.Abstraction
{
    public interface IGameplayAttributeContainer
    {
        bool HasAttribute(FName attributeName);

        bool TryGetAttribute(FName attributeName, out FGameplayAttribute attribute);

        FGameplayAttribute GetAttribute(FName attributeName);

        FGameplayAttributeContainer GetAttributeContainer();
    }
}