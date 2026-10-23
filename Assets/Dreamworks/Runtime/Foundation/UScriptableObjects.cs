using UnityEngine;

namespace DreamMachineGameStudio.DreamWorks.Foundation
{
    /// <summary>
    /// Serves as a base class for creating custom Unity ScriptableObject types.
    /// </summary>
    /// <remarks>This abstract class provides a foundation for defining reusable data containers in Unity.
    /// Derived classes can be used to store and manage data independently of MonoBehaviour instances, making them
    /// suitable for sharing data across scenes or objects.</remarks>
    public abstract class UScriptableObjects : ScriptableObject
    {
        
    }
}