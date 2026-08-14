using System;
using System.Collections.Generic;
using DreamMachineGameStudio.DreamWorks.Core;

namespace DreamMachineGameStudio.DreamWorks.Modules.AbilitySystem.Attributes
{
    public sealed class FGameplayAttributeContainer
    {
        #region Fields
        private readonly Dictionary<FName, FGameplayAttribute> attributes = new();
        #endregion

        #region Events
        public event Action<FGameplayAttributeChangedEventArgs> AttributeChanged;

        public event Action<FGameplayAttribute> AttributeAdded;

        public event Action<FGameplayAttribute> AttributeRemoved;
        #endregion

        #region Properties
        public IReadOnlyCollection<FGameplayAttribute> Attributes => attributes.Values;
        #endregion

        #region Public Methods
        public bool AddAttribute(FGameplayAttribute attribute)
        {
            if (attribute == null)
            {
                throw new ArgumentNullException(nameof(attribute));
            }

            if (attributes.ContainsKey(attribute.Name))
            {
                return false;
            }

            attributes.Add(attribute.Name, attribute);

            AttributeAdded?.Invoke(attribute);

            return true;
        }

        public bool RemoveAttribute(FName attributeName)
        {
            if (!attributes.Remove(attributeName, out FGameplayAttribute attribute))
            {
                return false;
            }

            AttributeRemoved?.Invoke(attribute);

            return true;
        }

        public bool HasAttribute(FName attributeName)
        {
            return attributes.ContainsKey(attributeName);
        }

        public bool TryGetAttribute(FName attributeName, out FGameplayAttribute attribute)
        {
            return attributes.TryGetValue(attributeName, out attribute);
        }

        public FGameplayAttribute GetAttribute(FName attributeName)
        {
            if (!attributes.TryGetValue(attributeName, out FGameplayAttribute attribute))
            {
                throw new InvalidOperationException($"Gameplay Attribute '{attributeName}' does not exist.");
            }

            return attribute;
        }

        public float GetBaseValue(FName attributeName)
        {
            return GetAttribute(attributeName).BaseValue;
        }

        public float GetCurrentValue(FName attributeName)
        {
            return GetAttribute(attributeName).CurrentValue;
        }

        public void SetBaseValue(FName attributeName, float value)
        {
            FGameplayAttribute attribute = GetAttribute(attributeName);

            float oldValue = attribute.BaseValue;

            if (Math.Abs(oldValue - value) < float.Epsilon)
            {
                return;
            }

            attribute.SetBaseValue(value);

            AttributeChanged?.Invoke(new FGameplayAttributeChangedEventArgs(attribute, oldValue, value, EGameplayAttributeValueChangeType.Base));
        }

        public void SetCurrentValue(FName attributeName, float value)
        {
            FGameplayAttribute attribute = GetAttribute(attributeName);

            float oldValue = attribute.CurrentValue;

            if (Math.Abs(oldValue - value) < float.Epsilon)
            {
                return;
            }

            attribute.SetCurrentValue(value);

            AttributeChanged?.Invoke(new FGameplayAttributeChangedEventArgs(attribute, oldValue, value, EGameplayAttributeValueChangeType.Current));
        }

        public void Clear()
        {
            attributes.Clear();
        }
        #endregion
    }
}