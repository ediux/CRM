using System;
namespace My.Core.Infrastructures.Implementations
{
	public enum OperationCodeEnum
	{
		Undefined = 0,
		Account_Create_Start = 1,
		Account_Create_End_Success = 2,
		Account_Create_End_Fail = 3,
		Account_Create_Rollback = 4,
		Account_BatchCreate_Start = 5,
		Account_BatchCreate_End_Success = 6,
		Account_BatchCreate_End_Fail = 7,
		Account_BatchCreate_Rollback = 8,
		Account_Update_Start = 9,
		Account_Update_End_Success = 10,
		Account_Update_End_Fail = 11,
		Account_Update_Rollback = 12,
		Account_Delete_Start = 13,
		Account_Delete_End_Success = 14,
		Account_Delete_End_Fail = 15,
		Account_Delete_Rollback = 16,
		Account_ChangePassword_Start = 17,
		Account_ChangePassword_End_Success = 18,
		Account_ChangePassword_End_Fail = 19,
		Account_ChangePassword_Rollback = 20,
		Account_Find_Start = 21,
		Account_Find_End_Success = 22,
		Accpimt_Find_End_Fail = 23,
		Account_FLAG_Online = 24,
		Account_FLAG_Offline = 25,
		Account_FindByEmail_Start = 26,
		Account_FindByEmail_End_Success = 27,
		Account_FindByEmail_End_Fail = 28,
		Account_FindById_Start = 29,
		Account_FindById_End_Success = 30,
		Account_FindById_End_Fail = 31,
		Account_FindByLoginAccount_Start = 32,
		Account_FindByLoginAccount_End_Success = 33,
		Account_FindByLoginAccount_End_Fail = 34,
		UserDefined = 65535
	}
}

