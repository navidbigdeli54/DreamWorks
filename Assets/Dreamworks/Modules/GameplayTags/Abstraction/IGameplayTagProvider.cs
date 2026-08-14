using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction
{
    public interface IGameplayTagProvider
    {
        IEnumerable<FGameplayTagDefinition> GetGameplayTags();
    }
}