using NaughtyAttributes;
using Terra.Components;
using Terra.ID;
using Terra.Particles;
using UnityEngine;

namespace Terra.Core.Generics
{
    /// <summary>
    /// Class that represents object inside the game world
    /// </summary>
    [RequireComponent(typeof(LookAtCameraComponent))]
    [RequireComponent(typeof(VFXController))]
    public abstract class Entity : InGameMonobehaviour, IUniqueable
    {

        [Foldout("References")] [SerializeField] VFXController _vfxController;
        [Foldout("Debug"), ReadOnly] [SerializeField] private int _id = -1;
        public virtual int Identity => _id;
        public VFXController VFXController => _vfxController;

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
            _id = newID;
        }

        protected virtual void OnValidate()
        {
            if (!_vfxController)
            {
                _vfxController = GetComponent<VFXController>();
            }
        }
    }
}