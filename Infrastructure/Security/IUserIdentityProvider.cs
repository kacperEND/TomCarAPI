using System.Security.Principal;

namespace Infrastructure.Security
{
    public interface IUserIdentityProvider
    {
        IIdentity Identity { get; }
    }
}