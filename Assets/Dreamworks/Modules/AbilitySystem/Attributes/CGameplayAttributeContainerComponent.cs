using UnityEngine;
using DreamMachineGameStudio.DreamWorks.Core;
using DreamMachineGameStudio.DreamWorks.GameFramework;

namespace DreamMachineGameStudio.DreamWorks.Modules.AbilitySystem.Attributes
{
    [DisallowMultipleComponent]
    public class CGameplayAttributeContainerComponent : CGameFrameworkComponent, IGameplayAttributeContainer
    {
        #region Fields
        private readonly FGameplayAttributeContainer attributeContainer = new();
        #endregion

        #region Properties
        public FGameplayAttributeContainer Attributes => attributeContainer;
        #endregion

        #region IGameplayAttributeContainer Implementation
        bool IGameplayAttributeContainer.HasAttribute(FName attributeName)
        {
            return attributeContainer.HasAttribute(attributeName);
        }

        bool IGameplayAttributeContainer.TryGetAttribute(FName attributeName, out FGameplayAttribute attribute)
        {
            return attributeContainer.TryGetAttribute(attributeName, out attribute);
        }

        FGameplayAttribute IGameplayAttributeContainer.GetAttribute(FName attributeName)
        {
            return attributeContainer.GetAttribute(attributeName);
        }

        FGameplayAttributeContainer IGameplayAttributeContainer.GetAttributeContainer()
        {
            return attributeContainer;
        }
        #endregion
    }
}