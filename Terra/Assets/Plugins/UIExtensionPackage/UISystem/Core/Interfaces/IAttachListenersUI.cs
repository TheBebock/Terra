namespace UIExtensionPackage.UISystem.Core.Interfaces
{
    public interface IAttachListenersUI
    {

        /// <summary>
        /// Attaches listeners, called during initialization pipeline
        /// </summary>
        public void AttachListeners();
        
        /// <summary>
        /// Detaches listeners, called during OnDestroy
        /// </summary>
        public void DetachListeners();
        
    }

}
