namespace Terra.Itemization.Interfaces
{

    /// <summary>
    /// Represents an item that can be equipped
    /// </summary>
    public interface IEquipable
    {
        public bool CanBeRemoved { get; }
        public void OnEquip();
        public void OnUnEquip();

    }
}
