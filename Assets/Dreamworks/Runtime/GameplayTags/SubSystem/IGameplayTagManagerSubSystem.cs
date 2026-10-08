using System.Collections.Generic;

namespace DreamMachineGameStudio.DreamWorks.GameplayTags.Abstraction
{
    public interface IGameplayTagManagerSubSystem
    {
        bool IsRegistered(FGameplayTag gameplayTag);

        FGameplayTag RequestGameplayTag(string tagName);

        IReadOnlyCollection<FGameplayTag> GetRegisteredGameplayTags();
    }
}
