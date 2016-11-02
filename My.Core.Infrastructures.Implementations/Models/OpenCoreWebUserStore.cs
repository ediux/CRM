using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace My.Core.Infrastructures.Implementations.Models
{
    // 您可以在 ApplicationUser 類別新增更多屬性，為使用者新增設定檔資料，請造訪 http://go.microsoft.com/fwlink/?LinkID=317594 以深入了解。

    public class OpenCoreWebUserStore : IUserStore<ApplicationUser, int>
        , IUserRoleStore<ApplicationUser, int>, IRoleStore<ApplicationRole, int>
        , IUserEmailStore<ApplicationUser, int>, IUserLockoutStore<ApplicationUser, int>
        , IUserLoginStore<ApplicationUser, int>, IUserPasswordStore<ApplicationUser, int>
        , IUserPhoneNumberStore<ApplicationUser, int>, IUserSecurityStampStore<ApplicationUser, int>
        , IUserTwoFactorStore<ApplicationUser, int>, IUserClaimStore<ApplicationUser, int>
        , IQueryableUserStore<ApplicationUser, int>, IQueryableRoleStore<ApplicationRole, int>
    {

        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        IApplicationUserRepository userrolerepo;
        IApplicationRoleRepository rolerepo;
        IApplicationUserProfileRepository profilerepo;
        IApplicationUserLoginRepository userloginrepo;

        public OpenCoreWebUserStore(DbContext context)
        {
            userrolerepo = RepositoryHelper.GetApplicationUserRepository();
            userrolerepo.UnitOfWork.Context = context;

            rolerepo = RepositoryHelper.GetApplicationRoleRepository();
            profilerepo = RepositoryHelper.GetApplicationUserProfileRepository();
            userloginrepo = RepositoryHelper.GetApplicationUserLoginRepository();
            userloginrepo.UnitOfWork = profilerepo.UnitOfWork = rolerepo.UnitOfWork = userrolerepo.UnitOfWork;

        }

        #region 使用者
        public virtual async Task CreateAsync(ApplicationUser user)
        {
            userrolerepo.Add(user);
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            user.Void = true;
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return await userrolerepo.FindUserByIdAsync(userId, false);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await userrolerepo.FindUserByLoginAccountAsync(userName, false);
        }

        public virtual async Task UpdateAsync(ApplicationUser user)
        {
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion

        #region User Role Store
        public async Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            var roleinstance = rolerepo.FindByName(roleName);

            if (roleinstance == null)
                throw new ArgumentException(string.Format("Role {0} not existed.", roleName), "roleName");

            int roleid = roleinstance.Id;
            user.ApplicationUserRole.Add(new ApplicationUserRole() { UserId = user.Id, RoleId = roleid, Void = false });
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var roles = from q in user.ApplicationUserRole
                            select q.ApplicationRole.Name;

                return (System.Collections.Generic.IList<string>)roles.ToList();
            });
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return Task.Run(() =>
            {
                var userinroles = from q in user.ApplicationUserRole
                                  where q.ApplicationRole.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                                  select q;
                return userinroles.Any();
            });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            var role = rolerepo.FindByName(roleName);

            if (role == null)
            {
                throw new ArgumentException(string.Format("Role {0} not existed.", roleName), "roleName");
            }

            if (user == null)
                throw new ArgumentNullException("user");

            var r = userrolerepo.Get(user.Id, role.Id);
            userrolerepo.Delete(r);
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region EMail Stroe
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var findemail = await profilerepo.FindByEmailAsync(email);
            return findemail;
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                try
                {
                    var userinroles = (from q in user.ApplicationUserProfileRef
                                       select q.ApplicationUserProfile.EMail).SingleOrDefault();
                    return userinroles;
                }
                catch (Exception)
                {
                    return string.Empty;
                }

            });
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var userinroles = (from q in user.ApplicationUserProfileRef
                                   select q.ApplicationUserProfile.EMailConfirmed);
                return userinroles.Single();
            });
        }

        public async Task SetEmailAsync(ApplicationUser user, string email)
        {
            //userrolerepo.ApplicationUserRepository.ApplicationUserProfileRefRepository.UserProfileRepository.
            var useremail = user.ApplicationUserProfileRef.Single();
            useremail.ApplicationUserProfile.EMail = email;
            useremail.ApplicationUserProfile.LastUpdateTime = DateTime.Now.ToUniversalTime();
            userrolerepo.UnitOfWork.Context.Entry(useremail).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            var useremail = user.ApplicationUserProfileRef.Single();
            useremail.ApplicationUserProfile.EMailConfirmed = confirmed;
            useremail.ApplicationUserProfile.LastUpdateTime = DateTime.Now.ToUniversalTime();
            userrolerepo.UnitOfWork.Context.Entry(useremail).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region Lockout Store
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.AccessFailedCount;
            });
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.LockoutEnabled.HasValue ? user.LockoutEnabled.Value : false;
            });
        }

        public Task<System.DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.LockoutEndDate.HasValue ? user.LockoutEndDate.Value : new System.DateTimeOffset(new DateTime(1754, 1, 1).ToUniversalTime());
            });
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount += 1;
            user.LastActivityTime = DateTime.Now;
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
            user = userrolerepo.Reload(user);
            return user.AccessFailedCount;

        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount = 0;
            user.LastActivityTime = DateTime.Now;

            user.LockoutEnabled = false;
            user.LockoutEndDate = DateTime.Now;
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.AccessFailedCount = 0;
            user.LastActivityTime = DateTime.Now;
            user.LastUpdateUserId = user.Id;
            user.LockoutEnabled = enabled;

            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd)
        {
            user.AccessFailedCount = 0;
            user.LastActivityTime = DateTime.Now;

            user.LockoutEndDate = new DateTime(lockoutEnd.Ticks);
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region Login Store
        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            user.ApplicationUserLogin.Add(new ApplicationUserLogin()
            {
                LoginProvider = login.LoginProvider,
                ProviderKey = login.ProviderKey,
                UserId = user.Id
            });

            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();

        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            var founduser = await userloginrepo.GetAsync(login.LoginProvider, login.ProviderKey);
            if (founduser == null)
                throw new ArgumentException("User is not found.", "login");
            return founduser.ApplicationUser;          
        }

        public Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var userlogins = userloginrepo.Where(w => w.UserId == user.Id);

                return (System.Collections.Generic.IList<UserLoginInfo>)(userlogins.ToList()
                    .ConvertAll(c => new UserLoginInfo(c.LoginProvider, c.ProviderKey)));
            });
        }

        public async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            var foundlogininfo = (from q in user.ApplicationUserLogin
                                  where q.LoginProvider == login.LoginProvider
                                  && q.ProviderKey == login.ProviderKey
                                  select q).Single();

            user.ApplicationUserLogin.Remove(foundlogininfo);
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();

        }
        #endregion

        #region Password Store
        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(user.Password))
                    return true;
                if (!string.IsNullOrEmpty(user.PasswordHash))
                    return true;
                return false;
            });
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            user.Password = "";
            user.LastActivityTime = user.LastUpdateTime = DateTime.Now.ToUniversalTime();
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();

        }
        #endregion

        #region Phone Number Store
        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var result = (from q in user.ApplicationUserProfileRef
                              select q.ApplicationUserProfile.PhoneNumber).SingleOrDefault();

                return result;
            });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                var result = (from q in user.ApplicationUserProfileRef
                              select q.ApplicationUserProfile.PhoneConfirmed).Single();
                return result;
            });
        }

        public async Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            var profile = (from q in user.ApplicationUserProfileRef
                           select q.ApplicationUserProfile).Single();

            profile.PhoneNumber = phoneNumber;
            profile.PhoneConfirmed = true;

            if (GetTwoFactorEnabledAsync(user).Result)
                profile.PhoneConfirmed = false;

            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();

        }

        public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            var profile = (from q in user.ApplicationUserProfileRef
                           select q.ApplicationUserProfile).Single();

            profile.PhoneConfirmed = confirmed;

            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();


        }
        #endregion

        #region Security Stamp Store
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.SecurityStamp;
            });
        }

        public async Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region TwoFactor
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.TwoFactorEnabled;
            });
        }

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region Role Store
        public async Task CreateAsync(ApplicationRole role)
        {
            rolerepo.Add(role);
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(ApplicationRole role)
        {
            role.Void = true;
            await UpdateAsync(role);
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByIdAsync(int roleId)
        {
            return Task.Run(() =>
            {
                return rolerepo.FindById(roleId);
            });
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByNameAsync(string roleName)
        {
            return Task.Run(() =>
            {
                return rolerepo.FindByName(roleName);
            });
        }

        public async Task UpdateAsync(ApplicationRole role)
        {
            userrolerepo.UnitOfWork.Context.Entry(role).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }
        #endregion

        #region 可用來查詢的使用者清單屬性
        public System.Linq.IQueryable<ApplicationUser> Users
        {
            get { return userrolerepo.All(); }
        }
        #endregion

        #region 可用來查詢的角色清單
        public System.Linq.IQueryable<ApplicationRole> Roles
        {
            get { return rolerepo.All(); }
        }
        #endregion

        public async Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            user.ApplicationUserClaim.Add(new ApplicationUserClaim()
            {
                ClaimType = claim.ValueType,
                ClaimValue = claim.Value,
                UserId = user.Id
            });

            userrolerepo.UnitOfWork.Context.Entry(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.ApplicationUserClaim.ToList().ConvertAll<Claim>(c => new Claim(c.ClaimType, c.ClaimValue)) as System.Collections.Generic.IList<Claim>;
            });
        }

        public Task RemoveClaimAsync(ApplicationUser user, Claim claim)
        {
            return Task.Run(() =>
            {

            });
        }
    }
}