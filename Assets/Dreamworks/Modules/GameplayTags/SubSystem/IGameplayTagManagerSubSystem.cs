using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction
{
    public interface IGameplayTagManagerSubSystem
    {
        bool IsRegistered(FGameplayTag gameplayTag);

        FGameplayTag RequestGameplayTag(string tagName);

        IReadOnlyCollection<FGameplayTag> GetRegisteredGameplayTags();
    }
}
