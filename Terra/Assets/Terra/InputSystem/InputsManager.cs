using Terra.Core.Generics;
using UnityEngine;

namespace Terra.InputSystem
{
    public class InputsManager : PersistentMonoSingleton<InputsManager>
    {
        private InputSystem _inputSystem;

        public InputSystem InputSystem => _inputSystem;
        public InputSystem.PlayerControlsActions PlayerControls => _inputSystem.PlayerControls;
        public InputSystem.AllTimeActions AllTimeControls => _inputSystem.AllTime;

        protected override void Awake()
        {
            base.Awake();
            _inputSystem = new InputSystem();
            
#if UNITY_EDITOR
            // Activate global controls
            EnableAllTimeControls();
            // Activate player controls
            EnablePlayerControls();
#endif
        }


        /// <summary>
        /// Method handles switching enable state of <see cref="AllTimeControls"/>
        /// </summary>
        public void SetAllTimeControlsState(bool value)
        {
            if (value)
            {
                EnableAllTimeControls();
            }
            else
            {
                DisableAllTimeControls();
            }
        }

        /// <summary>
        /// Method handles switching enable state of <see cref="PlayerControls"/>
        /// </summary>
        public void SetPlayerControlsState(bool value)
        {
            if (value)
            {
                EnablePlayerControls();
            }
            else
            {
                DisablePlayerControls();
            }
        }



        /// <summary>
        /// Method enables AllTime controls
        /// </summary>
        private void EnableAllTimeControls()
        {
            if (_inputSystem == null)
            {
                Debug.LogError("InputActions or AllTime is null in " + this);
                return;
            }

            _inputSystem.AllTime.Enable();
        }

        /// <summary>
        /// Method enables AllTime controls
        /// </summary>
        private void DisableAllTimeControls()
        {
            if (_inputSystem == null)
            {
                Debug.LogError("InputActions or AllTime is null in " + this);
                return;
            }

            _inputSystem.AllTime.Disable();
        }


        /// <summary>
        /// Method enables Player controls
        /// </summary>
        private void EnablePlayerControls()
        {
            if (_inputSystem == null)
            {
                Debug.LogError("InputActions or PlayerControls is null in " + this);
                return;
            }

            _inputSystem.PlayerControls.Enable();
        }

        /// <summary>
        /// Method disables Player controls
        /// </summary>
        private void DisablePlayerControls()
        {
            if (_inputSystem == null)
            {
                Debug.LogError("PlayerControls is null in " + this);
                return;
            }

            _inputSystem.PlayerControls.Disable();
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            
            _inputSystem.Dispose();
            _inputSystem = null;
        }
    }
}
