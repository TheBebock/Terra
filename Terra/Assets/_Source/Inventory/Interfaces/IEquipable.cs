
public interface IEquipable
{
    public bool CanBeRemoved { get; }
    public void OnEquip();
    public void OnUnEquip();
    
}
