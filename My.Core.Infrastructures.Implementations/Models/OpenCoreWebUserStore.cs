using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Linq;
using My.Core.Infrastructures.Implementations.Models;
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Implementations;
using My.Core.Infrastructures;

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

        IApplicationUserRoleRepository userrolerepo;


        public OpenCoreWebUserStore(DbContext context)
        {
            userrolerepo = RepositoryHelper.GetApplicationUserRoleRepository();
            userrolerepo.UnitOfWork.Context = context;
            userrolerepo.UnitOfWork.ConnectionString = context.Database.Connection.ConnectionString;
        }

        #region 使用者
        public async Task CreateAsync(ApplicationUser user)
        {
            userrolerepo.ApplicationUserRepository.Add(user);
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            user.Void = true;
            userrolerepo.UnitOfWork.Context.Entry<ApplicationUser>(user).State = EntityState.Modified;
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public async Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return await userrolerepo.ApplicationUserRepository.FindUserByIdAsync(userId, false);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await userrolerepo.ApplicationUserRepository.FindUserByLoginAccountAsync(userName, false);
        }

        public async Task UpdateAsync(ApplicationUser user)
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
            int roleid = userrolerepo.ApplicationRoleRepository.FindByName(roleName).Id;
            userrolerepo.Add(new ApplicationUserRole() { UserId = user.Id, RoleId = roleid, Void = false });
            await userrolerepo.UnitOfWork.CommitAsync();
        }

        public Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return Task<System.Collections.Generic.IList<string>>.Run(() =>
            {
                var roles = from q in user.ApplicationUserRole
                            select q.ApplicationRole.Name;

                return (System.Collections.Generic.IList<string>)roles.ToList();
            });
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return Task<bool>.Run(() =>
            {
                var userinroles = from q in user.ApplicationUserRole
                                  where q.ApplicationRole.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                                  select q;
                return userinroles.Any();
            });
        }

        public async Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            var role = userrolerepo.ApplicationRoleRepository.FindByName(roleName);

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
        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return Task<ApplicationUser>.Run(() =>
            {
                var findemail = userrolerepo.ApplicationUserRepository.FindByEmail(email);
                return findemail;
            });
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task<string>.Run(() =>
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
            return Task<bool>.Run(() =>
            {
                var userinroles = (from q in user.ApplicationUserProfileRef
                                   select q.ApplicationUserProfile.EMailConfirmed);
                return userinroles.Single();
            });
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return Task.Run(() =>
            {
                var useremail = user.ApplicationUserProfileRef.Single();
                useremail.ApplicationUserProfile.EMail = email;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.Run(() =>
            {
                var useremail = user.ApplicationUserProfileRef.Single();
                useremail.ApplicationUserProfile.EMailConfirmed = confirmed;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Lockout Store
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task<int>.Run(() =>
            {
                return user.AccessFailedCount;
            });
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task<bool>.Run(() =>
            {
                return user.LockoutEnabled.HasValue ? user.LockoutEnabled.Value : false;
            });
        }

        public Task<System.DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task<System.DateTimeOffset>.Run(() =>
            {
                return user.LockoutEndDate.HasValue ? user.LockoutEndDate.Value : new System.DateTimeOffset(new DateTime(1754, 1, 1).ToUniversalTime());
            });
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return Task<int>.Run(() =>
            {
                user.AccessFailedCount += 1;
                user.LastActivityTime = DateTime.Now;

                user = accountrepo.Update(user);
                accountrepo.SaveChanges();
                return user.AccessFailedCount;
            });
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime = DateTime.Now;

                user.LockoutEnabled = false;
                user.LockoutEndDate = DateTime.Now;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime = DateTime.Now;
                user.LastUpdateUserId = user.Id;
                user.LockoutEnabled = enabled;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd)
        {
            return Task.Run(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime = DateTime.Now;

                user.LockoutEndDate = new DateTime(lockoutEnd.Ticks);
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Login Store
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return Task.Run(() =>
            {
                user.ApplicationUserLogin.Add(new ApplicationUserLogin()
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return Task<ApplicationUser>.Run(() =>
            {
                var founduser = from q in accountrepo.FindAll()
                                from l in q.ApplicationUserLogin
                                where l.LoginProvider == login.LoginProvider
                                && l.ProviderKey == login.ProviderKey
                                select q;

                return founduser.SingleOrDefault();
            });
        }

        public Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return Task<System.Collections.Generic.IList<UserLoginInfo>>.Run(() =>
            {
                var founduser = from q in accountrepo.FindAll()
                                from l in q.ApplicationUserLogin
                                select l;

                return (System.Collections.Generic.IList<UserLoginInfo>)(founduser.ToList()
                    .ConvertAll(c => new UserLoginInfo(c.LoginProvider, c.ProviderKey)));
            });
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return Task.Run(() =>
            {
                var foundlogininfo = (from q in user.ApplicationUserLogin
                                      where q.LoginProvider == login.LoginProvider
                                      && q.ProviderKey == login.ProviderKey
                                      select q).Single();

                user.ApplicationUserLogin.Remove(foundlogininfo);
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Password Store
        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task<string>.Run(() =>
            {
                return user.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task<bool>.Run(() =>
            {
                if (!string.IsNullOrEmpty(user.Password))
                    return true;
                if (!string.IsNullOrEmpty(user.PasswordHash))
                    return true;
                return false;
            });
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.Run(() =>
            {
                user.PasswordHash = passwordHash;
                user.LastActivityTime = user.LastUpdateTime = DateTime.Now.ToUniversalTime();
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Phone Number Store
        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return Task<string>.Run(() =>
            {
                var result = (from q in user.ApplicationUserProfileRef
                              select q.ApplicationUserProfile.PhoneNumber).SingleOrDefault();

                return result;
            });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return Task<bool>.Run(() =>
            {
                var result = (from q in user.ApplicationUserProfileRef
                              select q.ApplicationUserProfile.PhoneConfirmed).Single();
                return result;
            });
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return Task.Run(() =>
            {
                var profile = (from q in user.ApplicationUserProfileRef
                               select q.ApplicationUserProfile).Single();

                profile.PhoneNumber = phoneNumber;
                profile.PhoneConfirmed = true;

                if (GetTwoFactorEnabledAsync(user).Result)
                    profile.PhoneConfirmed = false;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.Run(() =>
            {
                var profile = (from q in user.ApplicationUserProfileRef
                               select q.ApplicationUserProfile).Single();

                profile.PhoneConfirmed = confirmed;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Security Stamp Store
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return Task<string>.Run(() =>
            {
                return user.SecurityStamp;
            });
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return Task.Run(() =>
            {
                user.SecurityStamp = stamp;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region TwoFactor
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task<bool>.Run(() =>
            {
                return user.TwoFactorEnabled;
            });
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() =>
            {
                user.TwoFactorEnabled = enabled;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Role Store
        public Task CreateAsync(ApplicationRole role)
        {
            return Task.Run(() =>
            {
                rolerepo.Create(role);
                rolerepo.SaveChanges();
            });
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            return Task.Run(() =>
            {
                role.Void = true;
                UpdateAsync(role);
            });
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByIdAsync(int roleId)
        {
            return Task<ApplicationRole>.Run(() =>
            {
                return rolerepo.FindById(roleId);
            });
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByNameAsync(string roleName)
        {
            return Task<ApplicationRole>.Run(() =>
            {
                return rolerepo.FindByName(roleName);
            });
        }

        public Task UpdateAsync(ApplicationRole role)
        {
            return Task.Run(() =>
            {
                rolerepo.Update(role);
                rolerepo.SaveChanges();
            });
        }
        #endregion

        #region 可用來查詢的使用者清單屬性
        public System.Linq.IQueryable<ApplicationUser> Users
        {
            get { return uow.GetEntity<ApplicationUser>(); }
        }
        #endregion

        #region 可用來查詢的角色清單
        public System.Linq.IQueryable<ApplicationRole> Roles
        {
            get { return uow.GetEntity<ApplicationRole>(); }
        }
        #endregion

        public Task AddClaimAsync(ApplicationUser user, Claim claim)
        {
            return Task.Run(() =>
            {
                user.ApplicationUserClaim.Add(new ApplicationUserClaim()
                {
                    ClaimType = claim.ValueType,
                    ClaimValue = claim.Value,
                    UserId = user.Id
                });

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task<System.Collections.Generic.IList<Claim>> GetClaimsAsync(ApplicationUser user)
        {
            return Task<System.Collections.Generic.IList<Claim>>.Run(() =>
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