//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/00 Own/06 - Technical/Input/HIDInputComputer.inputactions
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

public partial class @HIDInputComputer: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @HIDInputComputer()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""HIDInputComputer"",
    ""maps"": [
        {
            ""name"": ""Pilot"",
            ""id"": ""fa5954d5-eaa6-4cee-ac2b-17343a8d989d"",
            ""actions"": [
                {
                    ""name"": ""LMB-Interact"",
                    ""type"": ""Button"",
                    ""id"": ""52b884d1-ce6c-480b-8c5c-b99f245c567e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""07ac28f7-d9e4-4f88-98c3-cc9ac457f6c1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LMB-Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Aircraftcontrols"",
            ""id"": ""0807af68-4b0b-4334-a655-7ea7861ec78c"",
            ""actions"": [
                {
                    ""name"": ""aControlsurfaces.Pitchcontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""716e8c14-0c00-4c50-8e27-cf9159300580"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aControlsurfaces.Rollcontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""2ae5b73e-95da-480d-a5b1-326d92de6051"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aControlsurfaces.Yawcontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""69399770-f406-4ef7-843b-ee0a996ded4a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aControlsurfaces.Airbrakecontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""319e1d17-b0c3-4d55-aa26-8ef5fe925c28"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aControlsurfaces.Flapcontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""5fa0c456-244b-4295-af53-180cf17e81c0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aAircraftsystems.Landinggearcontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""10163f29-f91a-440a-aba1-7d501c473841"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""aAircraftsystems.Throttlecontrol.Override"",
                    ""type"": ""Button"",
                    ""id"": ""08ffccce-3f26-447a-8b9a-4bd8ef040d16"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""aControlsurfaces.Pitchcontrol.Keyboard"",
                    ""id"": ""a922d62f-2836-4b85-b353-e48afe021c9f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Pitchcontrol.Override"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5318a183-ae37-4b62-859c-825a6c1ea060"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Pitchcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8b592f9b-05f1-4ad9-8b0f-630141193819"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Pitchcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""aControlsurfaces.Rollcontrol.Keyboard"",
                    ""id"": ""9d5bf0a9-f83f-49f0-bb39-d9db1b8d8ca3"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Rollcontrol.Override"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""56a1c5e5-6d62-41da-99db-b0fa76150b76"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Rollcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7d8b8cec-dfcb-456d-a167-29fd1a902c08"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Rollcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""aControlsurfaces.Yawcontrol.Keyboard"",
                    ""id"": ""e99fe34c-4883-4b0f-bd01-031e1ce5b28b"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Yawcontrol.Override"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""b593a648-91eb-44e9-b42d-dde8717962ad"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Yawcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""5708e674-4b6d-4fb1-baed-1b03fe41090d"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Yawcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""69fa7d1c-46e0-4fbe-a5be-212112cc2717"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Airbrakecontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""515aa64b-5602-48b0-9548-56df27d4cf6a"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aControlsurfaces.Flapcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5900b3ea-44b9-4e99-8d8c-cefb9095ebe0"",
                    ""path"": ""<Keyboard>/g"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aAircraftsystems.Landinggearcontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""aAircraftsystems.Throttlecontrol.Keyboard"",
                    ""id"": ""ef941308-9983-4927-b648-6ae8e0681bd7"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aAircraftsystems.Throttlecontrol.Override"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""4fd4ae83-0e63-4156-bc77-2ce922947621"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aAircraftsystems.Throttlecontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""005487e4-bff2-42b8-aebd-f649df7af983"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""aAircraftsystems.Throttlecontrol.Override"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Debugcontrols"",
            ""id"": ""a4a0ba91-d68e-48ce-ba3f-4ee1e33c7d70"",
            ""actions"": [
                {
                    ""name"": ""pDebug.Debugcontrols.Togglecursor"",
                    ""type"": ""Button"",
                    ""id"": ""251aa365-dc28-4c1a-8704-c5efb58aab43"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""pDebug.Debugcontrols.Cursorleftclick"",
                    ""type"": ""Button"",
                    ""id"": ""0c71a814-dd11-409c-8443-35619c519bea"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""2384e315-b02f-48dc-88fd-36e63253cf0c"",
                    ""path"": ""<Keyboard>/period"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""pDebug.Debugcontrols.Togglecursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4c08e26-b6ed-488b-a0b7-1a65bda55326"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""pDebug.Debugcontrols.Cursorleftclick"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Pilot
        m_Pilot = asset.FindActionMap("Pilot", throwIfNotFound: true);
        m_Pilot_LMBInteract = m_Pilot.FindAction("LMB-Interact", throwIfNotFound: true);
        // Aircraftcontrols
        m_Aircraftcontrols = asset.FindActionMap("Aircraftcontrols", throwIfNotFound: true);
        m_Aircraftcontrols_aControlsurfacesPitchcontrolOverride = m_Aircraftcontrols.FindAction("aControlsurfaces.Pitchcontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aControlsurfacesRollcontrolOverride = m_Aircraftcontrols.FindAction("aControlsurfaces.Rollcontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aControlsurfacesYawcontrolOverride = m_Aircraftcontrols.FindAction("aControlsurfaces.Yawcontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aControlsurfacesAirbrakecontrolOverride = m_Aircraftcontrols.FindAction("aControlsurfaces.Airbrakecontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aControlsurfacesFlapcontrolOverride = m_Aircraftcontrols.FindAction("aControlsurfaces.Flapcontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aAircraftsystemsLandinggearcontrolOverride = m_Aircraftcontrols.FindAction("aAircraftsystems.Landinggearcontrol.Override", throwIfNotFound: true);
        m_Aircraftcontrols_aAircraftsystemsThrottlecontrolOverride = m_Aircraftcontrols.FindAction("aAircraftsystems.Throttlecontrol.Override", throwIfNotFound: true);
        // Debugcontrols
        m_Debugcontrols = asset.FindActionMap("Debugcontrols", throwIfNotFound: true);
        m_Debugcontrols_pDebugDebugcontrolsTogglecursor = m_Debugcontrols.FindAction("pDebug.Debugcontrols.Togglecursor", throwIfNotFound: true);
        m_Debugcontrols_pDebugDebugcontrolsCursorleftclick = m_Debugcontrols.FindAction("pDebug.Debugcontrols.Cursorleftclick", throwIfNotFound: true);
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

    // Pilot
    private readonly InputActionMap m_Pilot;
    private List<IPilotActions> m_PilotActionsCallbackInterfaces = new List<IPilotActions>();
    private readonly InputAction m_Pilot_LMBInteract;
    public struct PilotActions
    {
        private @HIDInputComputer m_Wrapper;
        public PilotActions(@HIDInputComputer wrapper) { m_Wrapper = wrapper; }
        public InputAction @LMBInteract => m_Wrapper.m_Pilot_LMBInteract;
        public InputActionMap Get() { return m_Wrapper.m_Pilot; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PilotActions set) { return set.Get(); }
        public void AddCallbacks(IPilotActions instance)
        {
            if (instance == null || m_Wrapper.m_PilotActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PilotActionsCallbackInterfaces.Add(instance);
            @LMBInteract.started += instance.OnLMBInteract;
            @LMBInteract.performed += instance.OnLMBInteract;
            @LMBInteract.canceled += instance.OnLMBInteract;
        }

        private void UnregisterCallbacks(IPilotActions instance)
        {
            @LMBInteract.started -= instance.OnLMBInteract;
            @LMBInteract.performed -= instance.OnLMBInteract;
            @LMBInteract.canceled -= instance.OnLMBInteract;
        }

        public void RemoveCallbacks(IPilotActions instance)
        {
            if (m_Wrapper.m_PilotActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPilotActions instance)
        {
            foreach (var item in m_Wrapper.m_PilotActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PilotActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PilotActions @Pilot => new PilotActions(this);

    // Aircraftcontrols
    private readonly InputActionMap m_Aircraftcontrols;
    private List<IAircraftcontrolsActions> m_AircraftcontrolsActionsCallbackInterfaces = new List<IAircraftcontrolsActions>();
    private readonly InputAction m_Aircraftcontrols_aControlsurfacesPitchcontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aControlsurfacesRollcontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aControlsurfacesYawcontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aControlsurfacesAirbrakecontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aControlsurfacesFlapcontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aAircraftsystemsLandinggearcontrolOverride;
    private readonly InputAction m_Aircraftcontrols_aAircraftsystemsThrottlecontrolOverride;
    public struct AircraftcontrolsActions
    {
        private @HIDInputComputer m_Wrapper;
        public AircraftcontrolsActions(@HIDInputComputer wrapper) { m_Wrapper = wrapper; }
        public InputAction @aControlsurfacesPitchcontrolOverride => m_Wrapper.m_Aircraftcontrols_aControlsurfacesPitchcontrolOverride;
        public InputAction @aControlsurfacesRollcontrolOverride => m_Wrapper.m_Aircraftcontrols_aControlsurfacesRollcontrolOverride;
        public InputAction @aControlsurfacesYawcontrolOverride => m_Wrapper.m_Aircraftcontrols_aControlsurfacesYawcontrolOverride;
        public InputAction @aControlsurfacesAirbrakecontrolOverride => m_Wrapper.m_Aircraftcontrols_aControlsurfacesAirbrakecontrolOverride;
        public InputAction @aControlsurfacesFlapcontrolOverride => m_Wrapper.m_Aircraftcontrols_aControlsurfacesFlapcontrolOverride;
        public InputAction @aAircraftsystemsLandinggearcontrolOverride => m_Wrapper.m_Aircraftcontrols_aAircraftsystemsLandinggearcontrolOverride;
        public InputAction @aAircraftsystemsThrottlecontrolOverride => m_Wrapper.m_Aircraftcontrols_aAircraftsystemsThrottlecontrolOverride;
        public InputActionMap Get() { return m_Wrapper.m_Aircraftcontrols; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(AircraftcontrolsActions set) { return set.Get(); }
        public void AddCallbacks(IAircraftcontrolsActions instance)
        {
            if (instance == null || m_Wrapper.m_AircraftcontrolsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_AircraftcontrolsActionsCallbackInterfaces.Add(instance);
            @aControlsurfacesPitchcontrolOverride.started += instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesPitchcontrolOverride.performed += instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesPitchcontrolOverride.canceled += instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesRollcontrolOverride.started += instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesRollcontrolOverride.performed += instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesRollcontrolOverride.canceled += instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesYawcontrolOverride.started += instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesYawcontrolOverride.performed += instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesYawcontrolOverride.canceled += instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.started += instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.performed += instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.canceled += instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesFlapcontrolOverride.started += instance.OnAControlsurfacesFlapcontrolOverride;
            @aControlsurfacesFlapcontrolOverride.performed += instance.OnAControlsurfacesFlapcontrolOverride;
            @aControlsurfacesFlapcontrolOverride.canceled += instance.OnAControlsurfacesFlapcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.started += instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.performed += instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.canceled += instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.started += instance.OnAAircraftsystemsThrottlecontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.performed += instance.OnAAircraftsystemsThrottlecontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.canceled += instance.OnAAircraftsystemsThrottlecontrolOverride;
        }

        private void UnregisterCallbacks(IAircraftcontrolsActions instance)
        {
            @aControlsurfacesPitchcontrolOverride.started -= instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesPitchcontrolOverride.performed -= instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesPitchcontrolOverride.canceled -= instance.OnAControlsurfacesPitchcontrolOverride;
            @aControlsurfacesRollcontrolOverride.started -= instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesRollcontrolOverride.performed -= instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesRollcontrolOverride.canceled -= instance.OnAControlsurfacesRollcontrolOverride;
            @aControlsurfacesYawcontrolOverride.started -= instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesYawcontrolOverride.performed -= instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesYawcontrolOverride.canceled -= instance.OnAControlsurfacesYawcontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.started -= instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.performed -= instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesAirbrakecontrolOverride.canceled -= instance.OnAControlsurfacesAirbrakecontrolOverride;
            @aControlsurfacesFlapcontrolOverride.started -= instance.OnAControlsurfacesFlapcontrolOverride;
            @aControlsurfacesFlapcontrolOverride.performed -= instance.OnAControlsurfacesFlapcontrolOverride;
            @aControlsurfacesFlapcontrolOverride.canceled -= instance.OnAControlsurfacesFlapcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.started -= instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.performed -= instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsLandinggearcontrolOverride.canceled -= instance.OnAAircraftsystemsLandinggearcontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.started -= instance.OnAAircraftsystemsThrottlecontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.performed -= instance.OnAAircraftsystemsThrottlecontrolOverride;
            @aAircraftsystemsThrottlecontrolOverride.canceled -= instance.OnAAircraftsystemsThrottlecontrolOverride;
        }

        public void RemoveCallbacks(IAircraftcontrolsActions instance)
        {
            if (m_Wrapper.m_AircraftcontrolsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IAircraftcontrolsActions instance)
        {
            foreach (var item in m_Wrapper.m_AircraftcontrolsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_AircraftcontrolsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public AircraftcontrolsActions @Aircraftcontrols => new AircraftcontrolsActions(this);

    // Debugcontrols
    private readonly InputActionMap m_Debugcontrols;
    private List<IDebugcontrolsActions> m_DebugcontrolsActionsCallbackInterfaces = new List<IDebugcontrolsActions>();
    private readonly InputAction m_Debugcontrols_pDebugDebugcontrolsTogglecursor;
    private readonly InputAction m_Debugcontrols_pDebugDebugcontrolsCursorleftclick;
    public struct DebugcontrolsActions
    {
        private @HIDInputComputer m_Wrapper;
        public DebugcontrolsActions(@HIDInputComputer wrapper) { m_Wrapper = wrapper; }
        public InputAction @pDebugDebugcontrolsTogglecursor => m_Wrapper.m_Debugcontrols_pDebugDebugcontrolsTogglecursor;
        public InputAction @pDebugDebugcontrolsCursorleftclick => m_Wrapper.m_Debugcontrols_pDebugDebugcontrolsCursorleftclick;
        public InputActionMap Get() { return m_Wrapper.m_Debugcontrols; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugcontrolsActions set) { return set.Get(); }
        public void AddCallbacks(IDebugcontrolsActions instance)
        {
            if (instance == null || m_Wrapper.m_DebugcontrolsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DebugcontrolsActionsCallbackInterfaces.Add(instance);
            @pDebugDebugcontrolsTogglecursor.started += instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsTogglecursor.performed += instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsTogglecursor.canceled += instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsCursorleftclick.started += instance.OnPDebugDebugcontrolsCursorleftclick;
            @pDebugDebugcontrolsCursorleftclick.performed += instance.OnPDebugDebugcontrolsCursorleftclick;
            @pDebugDebugcontrolsCursorleftclick.canceled += instance.OnPDebugDebugcontrolsCursorleftclick;
        }

        private void UnregisterCallbacks(IDebugcontrolsActions instance)
        {
            @pDebugDebugcontrolsTogglecursor.started -= instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsTogglecursor.performed -= instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsTogglecursor.canceled -= instance.OnPDebugDebugcontrolsTogglecursor;
            @pDebugDebugcontrolsCursorleftclick.started -= instance.OnPDebugDebugcontrolsCursorleftclick;
            @pDebugDebugcontrolsCursorleftclick.performed -= instance.OnPDebugDebugcontrolsCursorleftclick;
            @pDebugDebugcontrolsCursorleftclick.canceled -= instance.OnPDebugDebugcontrolsCursorleftclick;
        }

        public void RemoveCallbacks(IDebugcontrolsActions instance)
        {
            if (m_Wrapper.m_DebugcontrolsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDebugcontrolsActions instance)
        {
            foreach (var item in m_Wrapper.m_DebugcontrolsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DebugcontrolsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DebugcontrolsActions @Debugcontrols => new DebugcontrolsActions(this);
    public interface IPilotActions
    {
        void OnLMBInteract(InputAction.CallbackContext context);
    }
    public interface IAircraftcontrolsActions
    {
        void OnAControlsurfacesPitchcontrolOverride(InputAction.CallbackContext context);
        void OnAControlsurfacesRollcontrolOverride(InputAction.CallbackContext context);
        void OnAControlsurfacesYawcontrolOverride(InputAction.CallbackContext context);
        void OnAControlsurfacesAirbrakecontrolOverride(InputAction.CallbackContext context);
        void OnAControlsurfacesFlapcontrolOverride(InputAction.CallbackContext context);
        void OnAAircraftsystemsLandinggearcontrolOverride(InputAction.CallbackContext context);
        void OnAAircraftsystemsThrottlecontrolOverride(InputAction.CallbackContext context);
    }
    public interface IDebugcontrolsActions
    {
        void OnPDebugDebugcontrolsTogglecursor(InputAction.CallbackContext context);
        void OnPDebugDebugcontrolsCursorleftclick(InputAction.CallbackContext context);
    }
}
