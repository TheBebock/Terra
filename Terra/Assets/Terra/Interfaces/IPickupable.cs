namespace Terra.Interfaces
{
    public interface IPickupable
    {
        public bool CanBePickedUp { get;}
        public void PickUp();
    }
}
