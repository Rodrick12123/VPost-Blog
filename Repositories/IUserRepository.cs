using Microsoft.AspNetCore.Identity;

namespace Blog.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<IdentityUser>> GetAllUsers();
        Task<IEnumerable<IdentityUser>> GetAllUsersWithSuperAdmin();
        Task<IdentityUser> GetUserAsync(Guid Id);
        Task<IdentityUser> UpdateUserInfo(Guid Id);
    }
}
