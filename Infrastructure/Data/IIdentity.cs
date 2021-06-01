namespace Infrastructure.Data
{
    public interface IIdentity<TKey>
    {
        TKey Id { get; set; }

        bool IsTransient();
    }
}