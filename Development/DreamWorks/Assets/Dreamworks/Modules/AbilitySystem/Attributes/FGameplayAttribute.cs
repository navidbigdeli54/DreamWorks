using DreamMachineGameStudio.DreamWorks.Core;

namespace DreamMachineGameStudio.DreamWorks.Modules.AbilitySystem.Attributes
{
    public sealed class FGameplayAttribute
    {
        #region Properties
        public FName Name { get; }

        public float BaseValue { get; private set; }

        public float CurrentValue { get; private set; }
        #endregion

        #region Constructors
        public FGameplayAttribute(FName name, float baseValue = 0)
        {
            Name = name;

            BaseValue = baseValue;

            CurrentValue = baseValue;
        }
        #endregion

        #region Public Methods
        internal void SetBaseValue(float value)
        {
            BaseValue = value;
        }

        internal void SetCurrentValue(float value)
        {
            CurrentValue = value;
        }

        internal void Reset()
        {
            CurrentValue = BaseValue;
        }
        #endregion
    }
}