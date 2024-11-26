
public interface IEquipable
{
    public bool CanBeRemoved { get; }
    public void Equip();
    public void UnEquip();
    
}
