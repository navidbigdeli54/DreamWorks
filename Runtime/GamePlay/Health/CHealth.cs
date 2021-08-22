/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

using System;
using UnityEngine;
using UnityEngine.Serialization;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public class CHealth : CComponent, IDamageable, IHealable
    {
        #region Fields
        protected const int MIN_HEALTH = 0;

        protected const int MAX_HEALTH = 100;

        [SerializeField]
        [FormerlySerializedAs("health")]
        protected int _health = MAX_HEALTH;
        #endregion

        #region Properties
        public int Health => _health;
        #endregion

        #region IHealable
        public bool CanHeal => _health < MAX_HEALTH;

        public Action<IHeal> OnHeal { get; set; }

        public void Heal(IHeal heal)
        {
            _health = Mathf.Clamp(_health += heal.Amount, MIN_HEALTH, MAX_HEALTH);

            OnHeal?.Invoke(heal);
        }
        #endregion

        #region IDamageable
        public Action OnDeath { get; set; }

        public Action<IDamage> OnTakeDamage { get; set; }

        public void TakeDamage(IDamage damage)
        {
            _health = Mathf.Clamp(_health - damage.Amount, MIN_HEALTH, MAX_HEALTH);

            OnTakeDamage?.Invoke(damage);

            if (_health == MIN_HEALTH)
            {
                OnDeath?.Invoke();
            }
        }
        #endregion
    }
}