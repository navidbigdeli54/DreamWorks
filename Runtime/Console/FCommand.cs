namespace DreamMachineGameStudio.Dreamworks.Console
{
    public abstract class FCommand
    {
        #region Properties
        public string Name { get; }

        public object Target { get; }
        #endregion

        #region Constructors
        public FCommand(string name, object target)
        {
            Name = name;

            Target = target;
        }
        #endregion

        #region Public Methods
        public abstract void Execute(string[] parameters);
        #endregion
    }
}
