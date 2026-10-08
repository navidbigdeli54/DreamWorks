using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.GameplayTags.Definitions;

namespace DreamMachineGameStudio.DreamWorks.GameplayTags.Abstraction
{
    public interface IGameplayTagSource
    {
        IEnumerable<FGameplayTagDefinition> GetGameplayTags();
    }
}