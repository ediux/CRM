using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationUserRepository : IApplicationUserRepository
    {
        // 此屬性為注入式相依性使用
        // 如無使用DI則會在第一次取用時初始化
        private IUserOperationLogRepository _userOperationLogRepository;
        public IUserOperationLogRepository UserOperationLogRepository
        {
            get
            {
                if (_userOperationLogRepository == null)
                {
                    _userOperationLogRepository = RepositoryHelper.GetUserOperationLogRepository();
                    _userOperationLogRepository.UnitOfWork = this.UnitOfWork; //設定使用相同的資料庫連線管理物件
                }

                return _userOperationLogRepository;
            }
            set { _userOperationLogRepository = value; }
        }

        public ApplicationUser ChangePassword(ApplicationUser UpdatedUserData)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_Start, currentLoginedUser));

                UnitOfWork.Context.Entry<ApplicationUser>(UpdatedUserData).State = System.Data.Entity.EntityState.Modified;

                UnitOfWork.Commit();

                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_End_Success, currentLoginedUser));

                return Reload(UpdatedUserData);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_End_Fail, currentLoginedUser));
                throw ex;
            }
        }

        public async Task<ApplicationUser> ChangePasswordAsync(ApplicationUser UpdateUserData)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                await WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_Start, currentLoginedUser);

                UnitOfWork.Context.Entry<ApplicationUser>(UpdateUserData).State = System.Data.Entity.EntityState.Modified;

                await UnitOfWork.CommitAsync();

                await WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_End_Success, currentLoginedUser);

                return await ReloadAsync(UpdateUserData);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_ChangePassword_End_Fail, currentLoginedUser));
                throw ex;
            }
        }

        public ApplicationUser FindByEmail(string email)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {

                IQueryable<ApplicationUser> queryset = ObjectSet.Include(i => i.ApplicationUserProfileRef);

                var result = from q in ApplicationUserProfileRefRepository.All()
                             where q.ApplicationUserProfile.EMail.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                             && q.Void == false
                             select q.ApplicationUser;

                ApplicationUser founduser = result.SingleOrDefault();

                return founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_FindByEmail_End_Fail, currentLoginedUser));
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {

                IQueryable<ApplicationUser> queryset = ObjectSet.Include(i => i.ApplicationUserProfileRef);

                var result = from q in queryset
                             from profilerefs in q.ApplicationUserProfileRef
                             where profilerefs.ApplicationUserProfile.EMail.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                             && q.Void == false
                             select q;

                ApplicationUser founduser = result.SingleOrDefault();

                return founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                Task.Run(() => WriteUserOperationLogAsync(OperationCodeEnum.Account_FindByEmail_End_Fail, currentLoginedUser));
                throw ex;
            }
        }

        public ApplicationUser FindUserById(int MemberId, bool isOnline)
        {
            try
            {
                ApplicationUser _founduser = Get(MemberId);

                if (_founduser != null)
                {
                    if (_founduser.LastActivityTime.HasValue)
                    {
                        if (_founduser.LastActivityTime.Value.AddMinutes(30) > DateTime.Now)
                        {
                            if (isOnline)
                            {
                                _founduser.LastActivityTime = DateTime.Now;
                                UnitOfWork.Commit();
                                return _founduser;
                            }
                        }
                    }
                }

                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindUserByIdAsync(int MemberId, bool isOnline)
        {
            try
            {
                ApplicationUser _founduser = await GetAsync(MemberId);

                if (_founduser != null)
                {
                    if (_founduser.LastActivityTime.HasValue)
                    {
                        if (_founduser.LastActivityTime.Value.AddMinutes(30) > DateTime.Now)
                        {
                            if (isOnline)
                            {
                                _founduser.LastActivityTime = DateTime.Now;
                                await UnitOfWork.CommitAsync();
                                return _founduser;
                            }
                        }
                    }
                }

                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public ApplicationUser FindUserByLoginAccount(string LoginAccount, bool IsOnline)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                ApplicationUser _founduser = null;


                var result = from q in ObjectSet
                             where q.UserName.Equals(LoginAccount, StringComparison.InvariantCultureIgnoreCase)
                             select q;

                _founduser = result.SingleOrDefault();

                if (IsOnline)
                {
                    //WriteUserOperationLogAsync(OperationCodeEnum.Account_FLAG_Online, currentLoginedUser);
                    _founduser.LastActivityTime = DateTime.Now;
                    UnitOfWork.Commit();
                }

                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public async Task<ApplicationUser> FindUserByLoginAccountAsync(string LoginAccount, bool IsOnline)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                ApplicationUser _founduser = null;


                var result = from q in ObjectSet
                             where q.UserName.Equals(LoginAccount, StringComparison.InvariantCultureIgnoreCase)
                             select q;

                _founduser = result.SingleOrDefault();

                if (IsOnline)
                {
                    //WriteUserOperationLogAsync(OperationCodeEnum.Account_FLAG_Online, currentLoginedUser);
                    _founduser.LastActivityTime = DateTime.Now;
                    await UnitOfWork.CommitAsync();
                }

                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public int FindUserIdFromPasswordResetToken(string Token, out ApplicationUser user)
        {
            try
            {
                var result = from q in ObjectSet
                             where q.ResetPasswordToken.Equals(Token, StringComparison.InvariantCulture)
                             select q;

                user = result.SingleOrDefault();

                return user.Id;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }



        public bool IsConfirmed(int MemberId)
        {
            try
            {

                //IUserProfile _profiledata = _userprofilerepository.Find(w => w.MemberId == MemberId &&
                //                                                        (w.EmailConfirmed || w.PhoneNumberConfirmed));
                ApplicationUser _founduser = Get(MemberId);

                if (_founduser != null)
                {
                    ApplicationUserProfileRef _profile = _founduser.ApplicationUserProfileRef.Single();
                    return (_profile.ApplicationUserProfile.EMailConfirmed
                        || _profile.ApplicationUserProfile.PhoneConfirmed);
                }

                return false;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public async Task<bool> IsConfirmedAsync(int MemberId)
        {
            try
            {

                //IUserProfile _profiledata = _userprofilerepository.Find(w => w.MemberId == MemberId &&
                //                                                        (w.EmailConfirmed || w.PhoneNumberConfirmed));
                ApplicationUser _founduser = await GetAsync(MemberId);

                if (_founduser != null)
                {
                    ApplicationUserProfileRef _profile = _founduser.ApplicationUserProfileRef.Single();
                    return (_profile.ApplicationUserProfile.EMailConfirmed
                        || _profile.ApplicationUserProfile.PhoneConfirmed);
                }

                return false;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public int ResetPasswordWithToken(string Token, string newPassword)
        {
            try
            {
                ApplicationUser _founduser = null;
                int _memberid = FindUserIdFromPasswordResetToken(Token, out _founduser);

                if (_founduser != null)
                {
                    _founduser.Password = newPassword;
                    UnitOfWork.Commit();
                    return (int)OperationCodeEnum.Account_ChangePassword_End_Success;
                }
                return (int)OperationCodeEnum.Account_ChangePassword_End_Fail;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public async Task<int> ResetPasswordWithTokenAsync(string Token, string newPassword)
        {
            try
            {
                ApplicationUser _founduser = null;
                int _memberid = FindUserIdFromPasswordResetToken(Token, out _founduser);

                if (_founduser != null)
                {
                    _founduser.Password = newPassword;
                    await UnitOfWork.CommitAsync();
                    return (int)OperationCodeEnum.Account_ChangePassword_End_Success;
                }
                return (int)OperationCodeEnum.Account_ChangePassword_End_Fail;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public bool ValidateUser(ApplicationUser UserDataToValidated)
        {
            try
            {
                ApplicationUser _validatedUser = null;

                var result = from w in ObjectSet
                             where w.UserName == UserDataToValidated.UserName
                             select w;

                _validatedUser = result.SingleOrDefault();

                if (_validatedUser != null)
                    return (UserDataToValidated.Password == _validatedUser.Password ||
                        UserDataToValidated.PasswordHash == _validatedUser.PasswordHash);

                return false;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }

        public Task<bool> ValidateUserAsync(ApplicationUser UserDataToValidated)
        {
            return Task<bool>.Run(() => ValidateUser(UserDataToValidated));
        }

        #region Helper Function
        protected virtual ApplicationUser getCurrentLoginedUser()
        {
            var rtn = base.GetCurrentLoginedUserId();

            if (rtn == -1)
            {
                var s = from q in ObjectSet
                        where q.UserName == "System"
                        select q;

                if (s.Any())
                {
                    return s.Single();
                }

                var usr = ApplicationUser.CreateKernelUser();
                base.Add(usr);
                usr = Reload(usr);
                UnitOfWork.Commit();

                return usr;
            }

            return base.Get(rtn);

        }
        /// <summary>
        /// Writes the user operation log.
        /// </summary>
        /// <returns>The user operation log.</returns>
        /// <param name="code">Code.</param>
        /// <param name="User">User.</param>
        protected async virtual Task WriteUserOperationLogAsync(OperationCodeEnum code, ApplicationUser User)
        {
            try
            {
                if (User == null)
                {
                    return;
                }

                string _url = string.Empty;
                string _body = string.Empty;

                if (System.Web.HttpContext.Current != null)
                {
                    _url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                    _body = Newtonsoft.Json.JsonConvert.SerializeObject(System.Web.HttpContext.Current.Request.Form);
                }

                _userOperationLogRepository.Add(new UserOperationLog()
                {
                    Body = _body,
                    UserId = User.Id,
                    LogTime = DateTime.Now,
                    OpreationCode = (int)code,
                    URL = _url
                });

                await UnitOfWork.CommitAsync();

                //_unitofwork.GetEntry<ApplicationUser>(User).State = EntityState.Modified;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }

        }

        protected virtual bool GetIsOnline(int memberid)
        {
            try
            {
                ApplicationUser _founduser = Get(memberid);

                if (_founduser != null)
                {
                    DateTime ExpireTime = DateTime.Now.AddMinutes(-30);

                    if (_founduser.LastActivityTime < ExpireTime)
                    {
                        return false;
                    }

                    return true;
                }

                throw new Exception("Not found.");
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }
        }
        #endregion

        private IApplicationUserProfileRefRepository _applicationUserProfileRefRepository;
        public IApplicationUserProfileRefRepository ApplicationUserProfileRefRepository
        {
            get
            {
                if (_applicationUserProfileRefRepository == null)
                {
                    _applicationUserProfileRefRepository = RepositoryHelper.GetApplicationUserProfileRefRepository();
                    _applicationUserProfileRefRepository.UnitOfWork = UnitOfWork;
                }

                return _applicationUserProfileRefRepository;
            }
            set
            {
                _applicationUserProfileRefRepository = value;
                _applicationUserProfileRefRepository.UnitOfWork = UnitOfWork;
            }
        }
    }

    public partial interface IApplicationUserRepository : IRepositoryBase<ApplicationUser>
    {
        IApplicationUserProfileRefRepository ApplicationUserProfileRefRepository { get; set; }
        /// <summary>
        /// 變更密碼
        /// </summary>
        /// <param name="UpdatedUserData"></param>
        /// <returns></returns>
        ApplicationUser ChangePassword(ApplicationUser UpdatedUserData);

        /// <summary>
        /// 變更密碼的非同步版本。
        /// </summary>
        /// <param name="UpdateUserData">欲變更密碼的使用者資訊。</param>
        /// <returns></returns>
        Task<ApplicationUser> ChangePasswordAsync(ApplicationUser UpdateUserData);

        /// <summary>
        /// 依據登入帳號尋找使用者資訊
        /// </summary>
        /// <param name="LoginAccount"></param>
        /// <param name="IsOnline"></param>
        /// <returns></returns>
        ApplicationUser FindUserByLoginAccount(string LoginAccount, bool IsOnline);

        Task<ApplicationUser> FindUserByLoginAccountAsync(string LoginAccount, bool IsOnline);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="isOnline"></param>
        /// <returns></returns>
        ApplicationUser FindUserById(int MemberId, bool isOnline);

        Task<ApplicationUser> FindUserByIdAsync(int MemberId, bool isOnline);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        int FindUserIdFromPasswordResetToken(string Token, out ApplicationUser user);

        /// <summary>
        /// 傳回使用者是否已被確認是本人建立？？
        /// </summary>
        /// <param name="SysUserId"></param>
        /// <returns></returns>
        bool IsConfirmed(int MemberId);

        Task<bool> IsConfirmedAsync(int MemberId);

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="UserDataToValidated"></param>
        /// <returns></returns>
        bool ValidateUser(ApplicationUser UserDataToValidated);

        Task<bool> ValidateUserAsync(ApplicationUser UserDataToValidated);

        /// <summary>
        /// 依據Token重設密碼。
        /// </summary>
        /// <returns>傳回執行結果。</returns>
        /// <param name="Token">Token.</param>
        /// <param name="newPassword">New password.</param>
        int ResetPasswordWithToken(string Token, string newPassword);

        Task<int> ResetPasswordWithTokenAsync(string Token, string newPassword);



        IUserOperationLogRepository UserOperationLogRepository { get; set; }
    }
}
