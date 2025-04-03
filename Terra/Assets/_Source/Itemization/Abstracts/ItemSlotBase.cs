using Terra.Itemization.Items.Definitions;

namespace Terra.Itemization.Abstracts
{
    
    public abstract class ItemSlotBase
    {
        public abstract bool IsSlotTaken { get; set; }
        public abstract bool CanEquip();

        public abstract bool EquipNonGeneric(ItemBase item);
        public abstract bool SwapNonGeneric(ItemBase item);
    }
    
    /// <summary>
    /// Represents base class for item slots
    /// </summary>
    public abstract class ItemSlotBase<TItem> : ItemSlotBase
    where TItem : ItemBase
    {
        public abstract bool Equip(TItem item);
        public abstract bool Swap(TItem item);
        public abstract bool UnEquip();
        
        public override bool EquipNonGeneric(ItemBase item)
        {
            if (item is TItem typedItem)
            {
                return Equip(typedItem);
            }
            return false;
        }
    
        public override bool SwapNonGeneric(ItemBase item)
        {
            if (item is TItem typedItem)
            {
                return Swap(typedItem);
            }
            return false;
        }
    }
    

}