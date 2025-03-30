using Blog.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blog.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AuthDbContext authContext;

        public UserRepository(AuthDbContext authContext)
        {
            this.authContext = authContext;
        }
        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            var users = await authContext.Users.ToListAsync();
            var superAdmin = await authContext.Users.FirstOrDefaultAsync(u => u.Email == "superAdmin@vpost.com");
            if (superAdmin != null)
            {
                users.Remove(superAdmin);
            }
            return users;
        }
        public async Task<IEnumerable<IdentityUser>> GetAllUsersWithSuperAdmin()
        {
            var users = await authContext.Users.ToListAsync();
            var superAdmin = await authContext.Users.FirstOrDefaultAsync(u => u.Email == "superAdmin@vpost.com");
            //if (superAdmin != null)
            //{
            //    users.Remove(superAdmin);
            //}
            return users;
        }
        public async Task<IdentityUser> GetUserAsync(Guid Id)
        {
            var user = await authContext.Users.FirstAsync(u => Guid.Parse(u.Id) == Id);
           
            return user;
        }

        public async Task<IdentityUser> UpdateUserInfo(Guid Id)
        {

            return null;
        }
    }
}
