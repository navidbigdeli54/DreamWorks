using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameplayTags.Definitions;

namespace DreamMachineGameStudio.DreamWorks.GameplayTags.Abstraction
{
    public interface IGameplayTagProvider
    {
        IEnumerable<FGameplayTagDefinition> GetGameplayTags();
    }
}