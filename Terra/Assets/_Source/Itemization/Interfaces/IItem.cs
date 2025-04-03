using Terra.Itemization.Items.Definitions;

public interface IItem<out TData>
    where TData : ItemData
{
    TData Data { get; }
}
