using UnityEngine;

namespace PrototypePattern
{
    /// <summary>
    /// Interface for prototype pattern.
    /// </summary>
    public interface IPrototype
    {
        /// <summary>
        /// Creates a clone of the GameObject.
        /// </summary>
        /// <returns>A new cloned GameObject.</returns>
        GameObject Clone();

        /// <summary>
        /// Creates a clone of the GameObject at the specified position.
        /// </summary>
        /// <param name="position">The position for the cloned GameObject.</param>
        /// <returns>A new cloned GameObject at the given position.</returns>
        GameObject Clone(Vector3 position);
    }
}