using System;

namespace My.Core.Infrastructures.Implementations.Models
{
	public static class RepositoryHelper
	{
		public static IUnitOfWork GetUnitOfWork()
		{
			return new EFUnitOfWork();
		}

		public static TUnitOfWork GetUnitOfWork<TUnitOfWork>() where TUnitOfWork : IUnitOfWork
        {
            return Activator.CreateInstance<TUnitOfWork>();
        }
		
		
		public static ApplicationGroupRepository GetApplicationGroupRepository()
		{
			var repository = new ApplicationGroupRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationGroupRepository GetApplicationGroupRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationGroupRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationGroupTreeRepository GetApplicationGroupTreeRepository()
		{
			var repository = new ApplicationGroupTreeRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationGroupTreeRepository GetApplicationGroupTreeRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationGroupTreeRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationRoleRepository GetApplicationRoleRepository()
		{
			var repository = new ApplicationRoleRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationRoleRepository GetApplicationRoleRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationRoleRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserRepository GetApplicationUserRepository()
		{
			var repository = new ApplicationUserRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserRepository GetApplicationUserRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserClaimRepository GetApplicationUserClaimRepository()
		{
			var repository = new ApplicationUserClaimRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserClaimRepository GetApplicationUserClaimRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserClaimRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserGroupRepository GetApplicationUserGroupRepository()
		{
			var repository = new ApplicationUserGroupRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserGroupRepository GetApplicationUserGroupRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserGroupRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserLoginRepository GetApplicationUserLoginRepository()
		{
			var repository = new ApplicationUserLoginRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserLoginRepository GetApplicationUserLoginRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserLoginRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserProfileRepository GetApplicationUserProfileRepository()
		{
			var repository = new ApplicationUserProfileRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserProfileRepository GetApplicationUserProfileRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserProfileRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserProfileRefRepository GetApplicationUserProfileRefRepository()
		{
			var repository = new ApplicationUserProfileRefRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserProfileRefRepository GetApplicationUserProfileRefRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserProfileRefRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static ApplicationUserRoleRepository GetApplicationUserRoleRepository()
		{
			var repository = new ApplicationUserRoleRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static ApplicationUserRoleRepository GetApplicationUserRoleRepository(IUnitOfWork unitOfWork)
		{
			var repository = new ApplicationUserRoleRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static UserOperationCodeDefineRepository GetUserOperationCodeDefineRepository()
		{
			var repository = new UserOperationCodeDefineRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static UserOperationCodeDefineRepository GetUserOperationCodeDefineRepository(IUnitOfWork unitOfWork)
		{
			var repository = new UserOperationCodeDefineRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		

		public static UserOperationLogRepository GetUserOperationLogRepository()
		{
			var repository = new UserOperationLogRepository();
			repository.UnitOfWork = GetUnitOfWork();
			return repository;
		}

		public static UserOperationLogRepository GetUserOperationLogRepository(IUnitOfWork unitOfWork)
		{
			var repository = new UserOperationLogRepository();
			repository.UnitOfWork = unitOfWork;
			return repository;
		}		
	}
}