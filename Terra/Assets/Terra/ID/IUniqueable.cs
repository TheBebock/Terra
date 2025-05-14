namespace Terra.ID
{
    public interface IUniqueable
    {
        /// <summary>
        /// Unique ID of an object
        /// </summary>
        int Identity { get; }

        /// <summary>
        /// Used when registering object's ID, if object doesn't have a unique ID, it will get a new one as well.
        /// </summary>
        public void RegisterID();

        /// <summary>
        /// Used when destroying an object, makes the ID available
        /// </summary>
        public void ReturnID();
        
        /// <summary>
        /// Used by <see cref="IDFactory"/>
        /// </summary>
        public void SetID(int newID);
    }
}