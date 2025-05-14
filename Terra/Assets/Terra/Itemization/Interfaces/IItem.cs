using Terra.Itemization.Abstracts.Definitions;

namespace Terra.Itemization.Interfaces
{
    public interface IItem<out TData>
        where TData : ItemData
    {
        TData Data { get; }
    }
}
