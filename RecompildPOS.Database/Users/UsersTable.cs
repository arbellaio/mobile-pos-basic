using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RecompildPOS.Database.DatabaseHandler;
using RecompildPOS.Database.Helpers;
using RecompildPOS.Models.Businesses;
using RecompildPOS.Models.Users;

namespace RecompildPOS.Database.Users
{
    public class UsersTable : IUsersTable
    {
        public LocalDatabase Handler { get; private set; }
        public UsersTable(LocalDatabase database)
        {
            if (database == null)
                throw new ArgumentNullException("Database");
            this.Handler = database;
        }


        public async Task AddUpdateUsers(UserSync user)
        {
            var userInDb = await GetUserById(user.UserId);
            if (userInDb == null)
            {
                if (!user.IsDeleted)
                    await Handler.Database.InsertAsync(user);
            }
            else
            {
                if (!user.IsDeleted)
                    await Handler.Database.UpdateAsync(user);
                else
                    await Handler.Database.DeleteAsync(userInDb);
            }
        }

        public async Task AddUpdateUsers(IList<UserSync> users)
        {
            foreach (var user in users)
            {
                var userInDb = await GetUserById(user.UserId);
                if (userInDb == null)
                {
                    if (!user.IsDeleted)
                        await Handler.Database.InsertAsync(user);
                }
                else
                {
                    if (!user.IsDeleted)
                        await Handler.Database.UpdateAsync(user);
                    else
                        await Handler.Database.DeleteAsync(userInDb);
                }
            }
        }

        public async Task<bool> CheckUser(string username, string password)
        {
            return await Handler.Database.Table<UserSync>().Where(x => x.Username.ToLower().Equals(username.ToLower()) && x.Password.Equals(password)).FirstOrDefaultAsync() != null;
        }

        private async Task<UserSync> GetUserById(int id)
        {
            return await Handler.Database.Table<UserSync>().Where(x => x.UserId.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<List<UserSync>> GetAllUsers()
        {
            return await Handler.Database.Table<UserSync>().ToListAsync();
        }

        public async Task<List<UserSync>> GetAllUsersByBusinessId(int businessId)
        {
            return await Handler.Database.Table<UserSync>().Where(x => x.BusinessId.Equals(businessId)).ToListAsync();
        }

        public async Task<UserSync> LoginUser(string username, string password)
        {

            var user = await Handler.Database.Table<UserSync>().Where(x => x.Username.ToLower().Equals(username.ToLower())).FirstOrDefaultAsync();
            
            if (user == null)
                return null;

            var isPasswordVerified = Hasher.VerifyHashedPassword(user.Password, password);
            if (!isPasswordVerified)
                return null;

            return user;
        }

        public async Task<List<User>> GetAllUnSyncedUsers()
        {
            return await Handler.Database.Table<User>().Where(x => x.IsPost && !x.IsSynced).ToListAsync();
        }

        public async Task UpdateUser(User user)
        {
            if (user != null)
            {
                await Handler.Database.UpdateAsync(user);
            }
        }
    }

    public interface IUsersTable
    {
        Task AddUpdateUsers(UserSync userSync);
        Task AddUpdateUsers(IList<UserSync> users);
        Task<List<UserSync>> GetAllUsers();
        Task<List<UserSync>> GetAllUsersByBusinessId(int businessId);
        Task<bool> CheckUser(string username, string password);
        Task<UserSync> LoginUser(string username, string password);

        Task<List<User>> GetAllUnSyncedUsers();
        Task UpdateUser(User user);
    }
}
