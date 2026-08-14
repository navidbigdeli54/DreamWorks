namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction
{
    public interface IGameplayTagContainer
    {
        bool HasGameplayTag(FGameplayTag gameplayTag);

        bool HasAllGameplayTags(FGameplayTagContainer gameplayTags);

        bool HasAnyGameplayTags(FGameplayTagContainer gameplayTags);
    }
}