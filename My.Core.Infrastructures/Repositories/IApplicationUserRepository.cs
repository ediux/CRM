using System;
using System.Collections.Generic;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures
{
    public interface IApplicationUserRepository<IAccount> : IRepositoryBase<IAccount> where IAccount : class
	{
		/// <summary>
		/// 取得所有系統內帳號！
		/// </summary>
		/// <returns>The p get all accounts.</returns>
		IList<IAccount> FindAllAccounts();

		/// <summary>
		/// 變更密碼
		/// </summary>
		/// <param name="UpdatedUserData"></param>
		/// <returns></returns>
		IAccount ChangePassword(IAccount UpdatedUserData);

		/// <summary>
		/// 依據登入帳號尋找使用者資訊
		/// </summary>
		/// <param name="LoginAccount"></param>
		/// <param name="IsOnline"></param>
		/// <returns></returns>
		IAccount FindUserByLoginAccount(string LoginAccount, bool IsOnline);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="MemberId"></param>
		/// <param name="isOnline"></param>
		/// <returns></returns>
		IAccount FindUserById(int MemberId, bool isOnline);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="Token"></param>
		/// <returns></returns>
		int FindUserIdFromPasswordResetToken(string Token, out IAccount user);

		/// <summary>
		/// 傳回使用者是否已被確認是本人建立？？
		/// </summary>
		/// <param name="SysUserId"></param>
		/// <returns></returns>
		bool IsConfirmed(int MemberId);

		/// <summary>
		/// 驗證使用者
		/// </summary>
		/// <param name="UserDataToValidated"></param>
		/// <returns></returns>
		bool ValidateUser(IAccount UserDataToValidated);

		/// <summary>
		/// 依據Token重設密碼。
		/// </summary>
		/// <returns>傳回執行結果。</returns>
		/// <param name="Token">Token.</param>
		/// <param name="newPassword">New password.</param>
		int ResetPasswordWithToken(string Token, string newPassword);

		/// <summary>
		/// Finds the by email.
		/// </summary>
		/// <returns>The by email.</returns>
		/// <param name="email">Email.</param>
		IAccount FindByEmail(string email);
	}
}

