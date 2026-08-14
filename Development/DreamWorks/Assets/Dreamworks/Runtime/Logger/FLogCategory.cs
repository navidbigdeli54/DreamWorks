using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Log
{
    public sealed class FLogCategory
    {
        #region Properties
        public string Name { get; private set; }

        public ELogVerbosity MinVerbosity { get; private set; }

        public Color32 Color { get; private set; } = new Color32(255, 255, 255, 255);
        #endregion

        #region Constructors
        public FLogCategory(string name, ELogVerbosity minVerbosity)
        {
            Name = name;

            MinVerbosity = minVerbosity;
        }

        public FLogCategory(string name, ELogVerbosity minVerbosity, Color32 color)
            : this(name, minVerbosity)
        {

            Color = color;
        }
        #endregion
    }
}