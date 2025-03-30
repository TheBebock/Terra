
namespace Terra.Itemization.Abstracts
{
    public abstract class ItemSlotBase
    {
        public abstract bool CanEquip();
        public abstract bool Equip(Item item);
        public abstract bool Swap(Item item);
        public abstract bool UnEquip();
        public abstract bool IsSlotTaken { get; set; }
    }
}