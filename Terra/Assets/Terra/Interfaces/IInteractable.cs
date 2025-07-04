namespace Terra.Interfaces
{
    public interface IInteractable
    {
        /// <summary>
        /// Checks if available for interaction
        /// </summary>
        public bool CanBeInteractedWith { get; }
        
        /// <summary>
        /// Called on interaction
        /// </summary>
        public void Interact();
    
        /// <summary>
        /// Called when is ready to be interacted with by Player
        /// </summary>
        public void ShowVisualization();


        /// <summary>
        /// Stop all visualisation
        /// </summary>
        public void StopVisualization(bool force = false);

    }
}
