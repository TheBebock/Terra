namespace Terra.ID
{
    public interface IUniqueable
    {
        /// <summary>
        /// Unique ID of an object
        /// </summary>
        int Identity { get; }

        /// <summary>
        /// Used on load to register ID to factory
        /// </summary>
        public void RegisterID();

        /// <summary>
        /// Used when destroying an object, to make the ID available
        /// </summary>
        public void ReturnID();
        
        /// <summary>
        /// Used by <see cref="IDFactory"/>
        /// </summary>
        public void SetID(int newID);
    }
}