/**Copyright 2016 - 2020, Dream Machine Game Studio. All Right Reserved.*/

#pragma warning disable CS0649

using System.Threading.Tasks;
using DreamMachineGameStudio.Dreamworks.Core;

namespace DreamMachineGameStudio.Dreamworks.HFSM
{
    public abstract class CHFSMBrain : CComponent
    {
        #region Fields
        private IHFSM _hfsm;
        #endregion

        #region Properties
        protected FHFSM Machine { get; private set; }
        #endregion

        #region Methods
        protected override async Task PreInitializeComponenetAsync()
        {
            await base.PreInitializeComponenetAsync();

            CanEverTick = true;

            CanEverLateTick = true;

            Machine = new FHFSM(this);
            _hfsm = Machine;

            await SetupHFSM();

            await _hfsm.PreInitializeAsync();
        }

        protected async override Task InitializeComponentAsync()
        {
            await base.InitializeComponentAsync();

            await _hfsm.InitializeAsync();
        }

        protected async override Task BeginPlayAsync()
        {
            await base.BeginPlayAsync();

            await _hfsm.BeginPlayAsync();
        }

        protected async override Task UninitializeCompoonentAsync()
        {
            await base.UninitializeCompoonentAsync();

            await _hfsm.UninitializeAsync();
        }

        protected override void TickComponent(float deltaTime)
        {
            base.TickComponent(deltaTime);

            _hfsm.Tick(deltaTime);
        }

        protected override void LateTickComponent(float deltaTime)
        {
            base.LateTickComponent(deltaTime);

            _hfsm.LateTick(deltaTime);
        }

        protected override void FixedTickComponent(float deltaTime)
        {
            base.FixedTickComponent(deltaTime);

            _hfsm.FixedTick(deltaTime);
        }

        protected abstract Task SetupHFSM();
        #endregion
    }
}