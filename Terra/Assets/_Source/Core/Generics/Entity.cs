using NaughtyAttributes;
using Terra.Components;
using Terra.EffectsSystem;
using Terra.ID;
using UnityEngine;

namespace Terra.Core.Generics
{
    /// <summary>
    /// Class that represents object inside the game world
    /// </summary>
    [RequireComponent(typeof(LookAtCameraComponent))]
    public abstract class Entity : InGameMonobehaviour, IUniqueable
    {
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private StatusContainer _statusContainer;
        [Foldout("Debug"), ReadOnly] [SerializeField]
        private int id = -1;

        public StatusContainer StatusContainer => _statusContainer;
        public int Identity => id;

        /// <summary>
        /// Handles registering object with unique ID
        /// </summary>
        /// <remarks>If there is no need for object to be registered, override this method</remarks>
        public virtual void RegisterID()
        {
            IDFactory.RegisterID(this);
        }

        /// <summary>
        /// Handles returning unique ID
        /// </summary>
        /// <remarks>If there is no need for object to be registered, override this method</remarks>
        public virtual void ReturnID()
        {
            IDFactory.ReturnID(this);
        }

        public void SetID(int newID)
        {
            id = newID;
        }
    }
}