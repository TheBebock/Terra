using UnityEditor;

namespace Terra.EventsSystem
{
    /// <summary>
    /// Contains methods and properties related to event buses and event types in the Unity application.
    /// </summary>
    public static class EventsUtil {
    
    
#if UNITY_EDITOR

    
        /// <summary>
        /// Initializes the Unity Editor related components of the EventBusUtil.
        /// The [InitializeOnLoadMethod] attribute causes this method to be called every time a script
        /// is loaded or when the game enters Play Mode in the Editor. This is useful to initialize
        /// fields or states of the class that are necessary during the editing state that also apply
        /// when the game enters Play Mode.
        /// The method sets up a subscriber to the playModeStateChanged event to allow
        /// actions to be performed when the Editor's play mode changes.
        /// </summary>    
        [InitializeOnLoadMethod]
        public static void InitializeEditor()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
    
        static void OnPlayModeStateChanged(PlayModeStateChange state) 
        {
        
            if (state == PlayModeStateChange.ExitingPlayMode) 
            {
                ClearEventsAPI();
            }
        }
#endif
    
        /// <summary>
        /// Clears (removes all listeners from) EventsAPI.
        /// </summary>
        private static void ClearEventsAPI() => EventsAPI.Clear();
    
    }
}