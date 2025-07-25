using System;
using NaughtyAttributes;
using UIExtensionPackage.ExtendedUI.Enums;
using UIExtensionPackage.UISystem.Core.Interfaces;
using UnityEngine;

namespace UIExtensionPackage.UISystem.Core.Base
{
    /// <summary>
    /// Class representing any object in UI.
    /// </summary>
    public abstract class UIObject : MonoBehaviour, IInitializable
    {
        [Foldout("Debug")][SerializeField, ReadOnly] private ActiveState _activeState = ActiveState.Enabled;
        [Foldout("Debug")][SerializeField, ReadOnly] private InteractionState _interactionState = InteractionState.None;
        public bool IsInitialized { get; set; }
        public ActiveState ActiveState => _activeState;
        public InteractionState InteractionState => _interactionState;
        public bool IsInteractionDisabled  => _interactionState == InteractionState.Disabled;
        public bool IsActive => _activeState == ActiveState.Enabled;
        
        public Action OnEnabled;
        public Action OnDisabled;
        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (IsInitialized) return;
            IsInitialized = true;
            
            if (this is IWithSetupUI setup)
            {
                setup.SetUp();
            }

            if (this is IAttachListenersUI attachListeners)
            {
                attachListeners.AttachListeners();
            }
            
            
            Enable();
            
            SetInteractionState(InteractionState.None);
            
        }
        

        /// <summary>
        /// Enable object
        /// </summary>
        public void Enable()
        {
            if (IsActive) return;
            SetActiveState(ActiveState.Enabled);
            HandleEnable();
        }

        /// <summary>
        /// Method called during enabling
        /// </summary>
        protected virtual void HandleEnable() { }
        
        /// <summary>
        /// Disable object
        /// </summary>
        public void Disable()
        {
            if (!IsActive) return;
            SetActiveState(ActiveState.Disabled);
            HandleDisable();
        }
        
        /// <summary>
        /// Method called during disabling
        /// </summary>
        protected virtual void HandleDisable() { }
        /// <summary>
        /// Method changes object's current interaction state
        /// </summary>
        protected virtual void SetInteractionState(InteractionState state)
        {
            if (!IsActive)
                _interactionState = InteractionState.Disabled;
            _interactionState = state;
        }
        /// <summary>
        /// Method changes object's current active state
        /// </summary>
        public void SetActiveState(ActiveState state)
        {
            _activeState = state;
        }
        
        protected virtual void CleanUp()
        {
        }
        protected virtual void OnDestroy()
        {
            CleanUp();
            
            if (this is IWithSetupUI setup)
            {
                setup.TearDown();
            }

            if (this is IAttachListenersUI attachListeners)
            {
                attachListeners.DetachListeners();
            }
        }

    }
}