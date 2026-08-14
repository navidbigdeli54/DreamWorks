using System;
using System.Linq;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Abstraction;
using DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Definitions;

namespace DreamMachineGameStudio.DreamWorks.Modules.GameplayTags.Providers
{
    public sealed class FReflectionGameplayTagSource : IGameplayTagSource
    {
        #region IGameplayTagSource
        public IEnumerable<FGameplayTagDefinition> GetGameplayTags()
        {
            IEnumerable<Type> providerTypes = AppDomain.CurrentDomain
                    .GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x =>
                        typeof(IGameplayTagProvider).IsAssignableFrom(x) &&
                        !x.IsAbstract &&
                        !x.IsInterface);

            foreach (Type providerType in providerTypes)
            {
                IGameplayTagProvider provider = (IGameplayTagProvider)Activator.CreateInstance(providerType);

                foreach (FGameplayTagDefinition tag in provider.GetGameplayTags())
                {
                    yield return tag;
                }
            }
        }
        #endregion
    }
}