//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Terra/InputSystem/InputSystem.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Terra.InputSystem
{
    public partial class @InputSystem: IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @InputSystem()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem"",
    ""maps"": [
        {
            ""name"": ""AllTime"",
            ""id"": ""2410fd40-d1ef-442a-a02a-90e8d0726988"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""e3af951a-5b4e-45e4-b2ae-bf375405748b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""dab7213b-c391-424d-a153-d68eb9203ac7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerControls"",
            ""id"": ""aeedc490-2d77-4603-bec0-393438b5e74f"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""d0071202-755e-4982-ac72-0f390d849832"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MeleeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""0c2f8ac1-efe5-4033-a8b2-491188c85c8e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""RangeAttack"",
                    ""type"": ""Button"",
                    ""id"": ""5b87042a-2765-4ba4-aad8-8b10e8614921"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""UseItem"",
                    ""type"": ""Button"",
                    ""id"": ""1e1df2ea-e1f2-41db-ad6f-279630d7f153"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interaction"",
                    ""type"": ""Button"",
                    ""id"": ""7c1c9f39-5b8f-4259-9562-8527cdb5bdd1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""a15efa4e-1a14-4e96-a821-321df36f7525"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7bf82692-615d-4ba5-8da7-2b0210069229"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""MeleeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24c8bffa-50b1-46c0-b8db-58c199277673"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""RangeAttack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5137e63c-9e8b-40ec-a3e3-8f349b7ee2cf"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""UseItem"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""24d4ac0c-f53d-46c5-b83f-0331dfc9afb5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Interaction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3a0f698e-9698-4986-8979-0b2ce22f34a6"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""35ec0c1f-8abe-4c95-b51f-a251d3599e28"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""0bd05991-6671-4138-b7e4-c428434e7169"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""d33582b6-8e33-4a24-b373-413ad2d35a95"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f3288b7c-16d1-4dcc-8419-925dfd887962"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""08ab30b6-4c06-4f6a-857f-ccbcb0a01135"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard"",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard"",
            ""bindingGroup"": ""Keyboard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // AllTime
            m_AllTime = asset.FindActionMap("AllTime", throwIfNotFound: true);
            m_AllTime_Pause = m_AllTime.FindAction("Pause", throwIfNotFound: true);
            // PlayerControls
            m_PlayerControls = asset.FindActionMap("PlayerControls", throwIfNotFound: true);
            m_PlayerControls_Movement = m_PlayerControls.FindAction("Movement", throwIfNotFound: true);
            m_PlayerControls_MeleeAttack = m_PlayerControls.FindAction("MeleeAttack", throwIfNotFound: true);
            m_PlayerControls_RangeAttack = m_PlayerControls.FindAction("RangeAttack", throwIfNotFound: true);
            m_PlayerControls_UseItem = m_PlayerControls.FindAction("UseItem", throwIfNotFound: true);
            m_PlayerControls_Interaction = m_PlayerControls.FindAction("Interaction", throwIfNotFound: true);
            m_PlayerControls_Dash = m_PlayerControls.FindAction("Dash", throwIfNotFound: true);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(asset);
        }

        public InputBinding? bindingMask
        {
            get => asset.bindingMask;
            set => asset.bindingMask = value;
        }

        public ReadOnlyArray<InputDevice>? devices
        {
            get => asset.devices;
            set => asset.devices = value;
        }

        public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

        public bool Contains(InputAction action)
        {
            return asset.Contains(action);
        }

        public IEnumerator<InputAction> GetEnumerator()
        {
            return asset.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Enable()
        {
            asset.Enable();
        }

        public void Disable()
        {
            asset.Disable();
        }

        public IEnumerable<InputBinding> bindings => asset.bindings;

        public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
        {
            return asset.FindAction(actionNameOrId, throwIfNotFound);
        }

        public int FindBinding(InputBinding bindingMask, out InputAction action)
        {
            return asset.FindBinding(bindingMask, out action);
        }

        // AllTime
        private readonly InputActionMap m_AllTime;
        private List<IAllTimeActions> m_AllTimeActionsCallbackInterfaces = new List<IAllTimeActions>();
        private readonly InputAction m_AllTime_Pause;
        public struct AllTimeActions
        {
            private @InputSystem m_Wrapper;
            public AllTimeActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
            public InputAction @Pause => m_Wrapper.m_AllTime_Pause;
            public InputActionMap Get() { return m_Wrapper.m_AllTime; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(AllTimeActions set) { return set.Get(); }
            public void AddCallbacks(IAllTimeActions instance)
            {
                if (instance == null || m_Wrapper.m_AllTimeActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_AllTimeActionsCallbackInterfaces.Add(instance);
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }

            private void UnregisterCallbacks(IAllTimeActions instance)
            {
                @Pause.started -= instance.OnPause;
                @Pause.performed -= instance.OnPause;
                @Pause.canceled -= instance.OnPause;
            }

            public void RemoveCallbacks(IAllTimeActions instance)
            {
                if (m_Wrapper.m_AllTimeActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IAllTimeActions instance)
            {
                foreach (var item in m_Wrapper.m_AllTimeActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_AllTimeActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public AllTimeActions @AllTime => new AllTimeActions(this);

        // PlayerControls
        private readonly InputActionMap m_PlayerControls;
        private List<IPlayerControlsActions> m_PlayerControlsActionsCallbackInterfaces = new List<IPlayerControlsActions>();
        private readonly InputAction m_PlayerControls_Movement;
        private readonly InputAction m_PlayerControls_MeleeAttack;
        private readonly InputAction m_PlayerControls_RangeAttack;
        private readonly InputAction m_PlayerControls_UseItem;
        private readonly InputAction m_PlayerControls_Interaction;
        private readonly InputAction m_PlayerControls_Dash;
        public struct PlayerControlsActions
        {
            private @InputSystem m_Wrapper;
            public PlayerControlsActions(@InputSystem wrapper) { m_Wrapper = wrapper; }
            public InputAction @Movement => m_Wrapper.m_PlayerControls_Movement;
            public InputAction @MeleeAttack => m_Wrapper.m_PlayerControls_MeleeAttack;
            public InputAction @RangeAttack => m_Wrapper.m_PlayerControls_RangeAttack;
            public InputAction @UseItem => m_Wrapper.m_PlayerControls_UseItem;
            public InputAction @Interaction => m_Wrapper.m_PlayerControls_Interaction;
            public InputAction @Dash => m_Wrapper.m_PlayerControls_Dash;
            public InputActionMap Get() { return m_Wrapper.m_PlayerControls; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(PlayerControlsActions set) { return set.Get(); }
            public void AddCallbacks(IPlayerControlsActions instance)
            {
                if (instance == null || m_Wrapper.m_PlayerControlsActionsCallbackInterfaces.Contains(instance)) return;
                m_Wrapper.m_PlayerControlsActionsCallbackInterfaces.Add(instance);
                @Movement.started += instance.OnMovement;
                @Movement.performed += instance.OnMovement;
                @Movement.canceled += instance.OnMovement;
                @MeleeAttack.started += instance.OnMeleeAttack;
                @MeleeAttack.performed += instance.OnMeleeAttack;
                @MeleeAttack.canceled += instance.OnMeleeAttack;
                @RangeAttack.started += instance.OnRangeAttack;
                @RangeAttack.performed += instance.OnRangeAttack;
                @RangeAttack.canceled += instance.OnRangeAttack;
                @UseItem.started += instance.OnUseItem;
                @UseItem.performed += instance.OnUseItem;
                @UseItem.canceled += instance.OnUseItem;
                @Interaction.started += instance.OnInteraction;
                @Interaction.performed += instance.OnInteraction;
                @Interaction.canceled += instance.OnInteraction;
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
            }

            private void UnregisterCallbacks(IPlayerControlsActions instance)
            {
                @Movement.started -= instance.OnMovement;
                @Movement.performed -= instance.OnMovement;
                @Movement.canceled -= instance.OnMovement;
                @MeleeAttack.started -= instance.OnMeleeAttack;
                @MeleeAttack.performed -= instance.OnMeleeAttack;
                @MeleeAttack.canceled -= instance.OnMeleeAttack;
                @RangeAttack.started -= instance.OnRangeAttack;
                @RangeAttack.performed -= instance.OnRangeAttack;
                @RangeAttack.canceled -= instance.OnRangeAttack;
                @UseItem.started -= instance.OnUseItem;
                @UseItem.performed -= instance.OnUseItem;
                @UseItem.canceled -= instance.OnUseItem;
                @Interaction.started -= instance.OnInteraction;
                @Interaction.performed -= instance.OnInteraction;
                @Interaction.canceled -= instance.OnInteraction;
                @Dash.started -= instance.OnDash;
                @Dash.performed -= instance.OnDash;
                @Dash.canceled -= instance.OnDash;
            }

            public void RemoveCallbacks(IPlayerControlsActions instance)
            {
                if (m_Wrapper.m_PlayerControlsActionsCallbackInterfaces.Remove(instance))
                    UnregisterCallbacks(instance);
            }

            public void SetCallbacks(IPlayerControlsActions instance)
            {
                foreach (var item in m_Wrapper.m_PlayerControlsActionsCallbackInterfaces)
                    UnregisterCallbacks(item);
                m_Wrapper.m_PlayerControlsActionsCallbackInterfaces.Clear();
                AddCallbacks(instance);
            }
        }
        public PlayerControlsActions @PlayerControls => new PlayerControlsActions(this);
        private int m_KeyboardSchemeIndex = -1;
        public InputControlScheme KeyboardScheme
        {
            get
            {
                if (m_KeyboardSchemeIndex == -1) m_KeyboardSchemeIndex = asset.FindControlSchemeIndex("Keyboard");
                return asset.controlSchemes[m_KeyboardSchemeIndex];
            }
        }
        public interface IAllTimeActions
        {
            void OnPause(InputAction.CallbackContext context);
        }
        public interface IPlayerControlsActions
        {
            void OnMovement(InputAction.CallbackContext context);
            void OnMeleeAttack(InputAction.CallbackContext context);
            void OnRangeAttack(InputAction.CallbackContext context);
            void OnUseItem(InputAction.CallbackContext context);
            void OnInteraction(InputAction.CallbackContext context);
            void OnDash(InputAction.CallbackContext context);
        }
    }
}
