/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using UnityEngine;
using System.Threading.Tasks;

namespace DreamMachineGameStudio.Dreamworks.GamePlay.Health
{
    public class CExplodable : CHealth
    {
        #region Fields
        [SerializeField]
        private GameObject _explosionParticle;

        [SerializeField]
        private float _explosionRedius;

        [SerializeField]
        private int _damage;

        private bool _isExploded;
        #endregion

        #region Protected Methods
        protected override async Task InitializeComponentAsync()
        {
            await base.InitializeComponentAsync();

            OnDeath += OnExplode;
        }
        #endregion

        #region Private Methods
        private void OnExplode()
        {
            if (_isExploded)
            {
                return;
            }

            _isExploded = true;

            Instantiate(_explosionParticle, transform.position, transform.rotation);

            RaycastHit[] hits = Physics.SphereCastAll(transform.position, _explosionRedius, Vector3.up);
            if (hits != null && hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; ++i)
                {
                    RaycastHit currentHit = hits[i];

                    if (currentHit.collider.gameObject == gameObject)
                    {
                        continue;
                    }

                    IDamageable damagable = currentHit.transform.GetComponent<IDamageable>();

                    if (damagable != null)
                    {
                        damagable.TakeDamage(new FDamage(_damage));
                    }
                }
            }

            Destroy(gameObject);
        }
        #endregion
    }
}