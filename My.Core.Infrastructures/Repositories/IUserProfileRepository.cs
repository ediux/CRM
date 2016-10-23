using System;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures
{
	public interface IUserProfileRepository<IUserProfile> : IRepositoryBase<IUserProfile> where IUserProfile : class
	{
		/// <summary>
		/// Finds the user identifier by email.
		/// </summary>
		/// <returns>The user identifier by email.</returns>
		/// <param name="email">Email.</param>
		int FindUserIdByEmail(string email);

		/// <summary>
		/// Ises the email confirmed by user identifier.
		/// </summary>
		/// <returns>The email confirmed by user identifier.</returns>
		/// <param name="UserId">User identifier.</param>
		bool IsEmailConfirmedByUserId(int UserId);

		/// <summary>
		/// Finds the user identifier by phone.
		/// </summary>
		/// <returns>The user identifier by phone.</returns>
		/// <param name="PhoneNumber">Phone number.</param>
		int FindUserIdByPhone(string PhoneNumber);

		/// <summary>
		/// Ises the phone confirme by user identifier.
		/// </summary>
		/// <returns>The phone confirme by user identifier.</returns>
		/// <param name="UserId">User identifier.</param>
		bool IsPhoneConfirmeByUserId(int UserId);

		/// <summary>
		/// Gets the custom field value.
		/// </summary>
		/// <returns>The custom field value.</returns>
		/// <param name="FieldName">Field name.</param>
		string GetCustomFieldValue(string FieldName);

		/// <summary>
		/// Sets the custom field value.
		/// </summary>
		/// <returns>The custom field value.</returns>
		/// <param name="FieldName">Field name.</param>
		void SetCustomFieldValue(string FieldName);

		/// <summary>
		/// Gets the empty custom field counts.
		/// </summary>
		/// <returns>The empty custom field counts.</returns>
		int GetEmptyCustomFieldCounts();

		/// <summary>
		/// Adds the custom field.
		/// </summary>
		/// <returns>The custom field.</returns>
		/// <param name="FieldName">Field name.</param>
		IUserProfile AddCustomField(string FieldName);

		/// <summary>
		/// Changes the name of the field.
		/// </summary>
		/// <returns>The field name.</returns>
		/// <param name="oldName">Old name.</param>
		/// <param name="newName">New name.</param>
		IUserProfile ChangeFieldName(string oldName, string newName);

		/// <summary>
		/// Removes the custom field.
		/// </summary>
		/// <returns>The custom field.</returns>
		/// <param name="FieldName">Field name.</param>
		void RemoveCustomField(string FieldName);
	}
}

