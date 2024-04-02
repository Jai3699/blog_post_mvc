using Microsoft.AspNetCore.Identity;

namespace Blogpost.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAll();
    }
}
