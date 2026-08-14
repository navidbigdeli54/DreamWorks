using DreamMachineGameStudio.DreamWorks.Core;

namespace DreamMachineGameStudio.DreamWorks.Modules.AbilitySystem.Attributes
{
    public interface IGameplayAttributeContainer
    {
        bool HasAttribute(FName attributeName);

        bool TryGetAttribute(FName attributeName, out FGameplayAttribute attribute);

        FGameplayAttribute GetAttribute(FName attributeName);

        FGameplayAttributeContainer GetAttributeContainer();
    }
}