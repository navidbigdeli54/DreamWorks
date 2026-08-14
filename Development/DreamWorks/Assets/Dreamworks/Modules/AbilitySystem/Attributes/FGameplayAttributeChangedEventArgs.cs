namespace DreamMachineGameStudio.DreamWorks.Modules.AbilitySystem.Attributes
{
    public readonly struct FGameplayAttributeChangedEventArgs
    {
        #region Properties
        public FGameplayAttribute Attribute { get; }

        public float OldValue { get; }

        public float NewValue { get; }

        public EGameplayAttributeValueChangeType ChangeType { get; }
        #endregion

        #region Constructors
        public FGameplayAttributeChangedEventArgs(FGameplayAttribute attribute, float oldValue, float newValue, EGameplayAttributeValueChangeType valueType)
        {
            Attribute = attribute;
            OldValue = oldValue;
            NewValue = newValue;
            ChangeType = valueType;
        }
        #endregion
    }
}