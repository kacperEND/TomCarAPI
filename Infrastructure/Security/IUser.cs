namespace Infrastructure.Security
{
    public interface IUser
    {
        int Id { get; }
        string UserName { get; }
    }
}