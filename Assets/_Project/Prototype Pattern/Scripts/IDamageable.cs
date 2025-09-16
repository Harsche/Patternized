using UnityEngine;

namespace PrototypePattern
{
    /// <summary>
    /// Interface for objects that can take damage and be destroyed.
    /// Provides properties for health management and methods for handling damage events.
    /// </summary>
    public interface IDamageable
    {
        /// <summary>
        /// Gets or sets the current health value of the object.
        /// </summary>
        public float Health { get; }

        /// <summary>
        /// Gets or sets whether this object can be targeted for attacks.
        /// </summary>
        public bool Targetable { get; }

        /// <summary>
        /// Gets or sets whether this object is invincible and immune to damage.
        /// </summary>
        public bool Invincible { get; }

        /// <summary>
        /// Called when the object is hit with damage and knockback.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        /// <param name="knockback">Knockback force vector.</param>
        public void OnHit(int damage, Vector2 knockback);

        /// <summary>
        /// Called when the object is hit with damage only.
        /// </summary>
        /// <param name="damage">Amount of damage to apply.</param>
        public void OnHit(int damage);

        /// <summary>
        /// Called when the object's health reaches zero or below.
        /// </summary>
        public void OnDeath();
    }
}