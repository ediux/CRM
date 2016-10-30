using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;
using My.Core.Infrastructures.Implementations.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNet.Identity;

namespace My.Core.Infrastructures.Implementations
{
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository<ApplicationUser>
    {

        private IUserOperationCodeDefineRepository<UserOperationCodeDefine> logdefinerepo;
        private IUserOperationLogRepository<UserOperationLog> logrepo;

        public ApplicationUserRepository(IUnitofWork unitofwork)
            : base(unitofwork)
        {
            logdefinerepo = _unitofwork.GetRepository<UserOperationCodeDefineRepository, UserOperationCodeDefine>() as IUserOperationCodeDefineRepository<UserOperationCodeDefine>;
            logrepo = _unitofwork.GetRepository<UserOperationLogRepository, UserOperationLog>() as IUserOperationLogRepository<UserOperationLog>;
        }

        #region Helper Functions
        /// <summary>
        /// Writes the user operation log.
        /// </summary>
        /// <returns>The user operation log.</returns>
        /// <param name="code">Code.</param>
        /// <param name="User">User.</param>
        protected virtual void WriteUserOperationLog(OperationCodeEnum code, ApplicationUser User)
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

                logrepo.Create(new UserOperationLog()
                {
                    Body = _body,
                    UserId = User.Id,
                    LogTime = DateTime.Now,
                    OpreationCode = (int)code,
                    URL = _url
                });

                SaveChanges();

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
                ApplicationUser _founduser = Find(memberid);

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

        public override IList<ApplicationUser> BatchCreate(IEnumerable<ApplicationUser> entities)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Start, currentLoginedUser);
                var result = base.BatchCreate(entities);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Success, currentLoginedUser);

                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Rollback, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public ApplicationUser ChangePassword(ApplicationUser UpdatedUserData)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_Start, currentLoginedUser);

                ApplicationUser _updateduser = Update(UpdatedUserData);

                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Success, currentLoginedUser);
                return _updateduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public override ApplicationUser Create(ApplicationUser entity)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                var isexists = FindUserByLoginAccount(entity.UserName, false);

                if (isexists != null && isexists.Id > 0)
                {
                    throw new Exception("User is existed.");
                }

                WriteUserOperationLog(OperationCodeEnum.Account_Create_Start, currentLoginedUser);
                entity.LastUpdateTime = entity.CreateTime = DateTime.Now.ToUniversalTime();
                entity.LastUpdateUserId = entity.CreateUserId = currentLoginedUser.Id;
                entity = base.Create(entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Success, currentLoginedUser);

                return entity;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Fail, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_Rollback, currentLoginedUser);
                throw ex;
            }

        }

        public override void Delete(ApplicationUser entity)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Start, entity);
                base.Delete(entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Success, entity);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Fail, entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Rollback, entity);
                throw ex;
            }

        }

        public override IQueryable<ApplicationUser> FindAll()
        {
            try
            {
                return _datatable
                    .Include(w => w.ApplicationUserGroup)
                    .Include(w => w.ApplicationUserProfileRef)
                    .Include(w => w.ApplicationUserRole).AsQueryable();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public IList<ApplicationUser> FindAllAccounts()
        {
            try
            {
                return FindAll().ToList();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public ApplicationUser FindByEmail(string email)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                IQueryable<ApplicationUser> queryset = _datatable.Include(i => i.ApplicationUserProfileRef);

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
                WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public override ApplicationUser Find(params object[] values)
        {
            var result = base.Find(values);
            if (result == null)
                result = ApplicationUser.Create();

            return result;
        }

        public ApplicationUser FindUserById(int MemberId, bool isOnline)
        {
            try
            {

                ApplicationUser _founduser = Find(MemberId);

                if (_founduser != null)
                {
                    if (_founduser.LastActivityTime.HasValue)
                    {
                        if (_founduser.LastActivityTime.Value.AddMinutes(30) > DateTime.Now)
                        {
                            if (isOnline)
                            {
                                _founduser.LastActivityTime = DateTime.Now;
                                Update(_founduser);
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

        protected virtual ApplicationUser getCurrentLoginedUser()
        {
            var rtn = base.GetCurrentLoginedUserId();

            if (rtn == -1)
            {
                var s = from q in _datatable
                        where q.UserName == "System"
                        select q;

                if (s.Any())
                {
                    return s.Single();
                }

                var usr = ApplicationUser.CreateKernelUser();
                usr = base.Create(usr);
                SaveChanges();

                return usr;
            }

            return base.Find(rtn);

        }
        protected override int GetCurrentLoginedUserId()
        {
            var rtn = base.GetCurrentLoginedUserId();

            if (rtn == -1)
            {
                var s = from q in _datatable
                        where q.UserName == "System"
                        select q.Id;

                if (s.Any())
                {
                    return s.Single();
                }

                return -1;
            }

            return rtn;
        }

        public ApplicationUser FindUserByLoginAccount(string LoginAccount, bool IsOnline)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                ApplicationUser _founduser = null;


                var result = from q in _datatable
                             where q.UserName.Equals(LoginAccount, StringComparison.InvariantCultureIgnoreCase)
                             select q;

                _founduser = result.SingleOrDefault();

                if (IsOnline)
                {
                    //WriteUserOperationLogAsync(OperationCodeEnum.Account_FLAG_Online, currentLoginedUser);
                    _founduser.LastActivityTime = DateTime.Now;
                    base.Update(_founduser);
                    SaveChanges();
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


                var result = from q in _datatable
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
                ApplicationUser _founduser = Find(MemberId);

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
                    Update(_founduser);
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



        public override ApplicationUser Update(ApplicationUser entity)
        {
            var currentLoginedUser = getCurrentLoginedUser();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Update_Start, currentLoginedUser);
                entity.LastUpdateTime = DateTime.Now.ToUniversalTime();
                entity.LastUpdateUserId = currentLoginedUser.Id;
                var entry = _unitofwork.GetEntry<ApplicationUser>(entity);

                if (entry != null && entry.State == EntityState.Modified)
                    base.Update(entity);

                WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Success, currentLoginedUser);
                return entity;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Update_Rollback, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public bool ValidateUser(ApplicationUser UserDataToValidated)
        {
            try
            {
                ApplicationUser _validatedUser = null;

                var result = from w in _datatable
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
    }
}

