using UnityEngine;

namespace PrototypePattern
{
    /// <summary>
    /// Interface for prototype pattern.
    /// </summary>
    public interface IPrototype<Type> where Type : Component
    {
        /// <summary>
        /// Creates a clone of the component.
        /// </summary>
        /// <returns>A new cloned component of type T.</returns>
        Type Clone();

        /// <summary>
        /// Creates a clone of the component at the specified position.
        /// </summary>
        /// <param name="position">The position for the cloned component.</param>
        /// <returns>A new cloned component of type T at the given position.</returns>
        Type Clone(Vector3 position);
    }
}