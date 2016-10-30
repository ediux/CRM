using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class ApplicationUserProfileRepository
    {
        public ApplicationUser FindByEmail(string email)
        {
            var user = ( from e in ObjectSet
                       from u in e.ApplicationUserProfileRef
                       where e.EMail.Equals(email)
                       select u.ApplicationUser).SingleOrDefault();

            return user;
                     
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return Task.Run(() => FindByEmail(email));
        }

        public async Task SetEmailAsync(ApplicationUser user, string email)
        {
            var profiles = from q in ObjectSet
                           from u in q.ApplicationUserProfileRef
                           where u.ApplicationUser.Id == user.Id
                           select q;

            if (profiles != null && profiles.Any())
            {
                var profile = profiles.Single();

                if (profile.EMail != email)
                {
                    profile.EMail = email;
                    profile.LastUpdateTime = DateTime.Now.ToUniversalTime();
                }
            }
            else
            {
                var profile = new ApplicationUserProfile();
                profile.LastUpdateTime = profile.CreateTime = DateTime.Now.ToUniversalTime();
                profile.EMail = email;
                if (((OpenWebSiteEntities)UnitOfWork.Context).RequireUniqueEmail == false)
                {
                    profile.EMailConfirmed = true;
                }
                profile = Add(profile);
 
                user.ApplicationUserProfileRef.Add(new ApplicationUserProfileRef()
                {
                    CreateTime = DateTime.Now.ToUniversalTime(),
                    LastUpdateTime = DateTime.Now.ToUniversalTime(),
                    ProfileId = profile.Id,
                    UserId = user.Id,
                    Void = false
                });
                UnitOfWork.Commit();
            }
            await UnitOfWork.CommitAsync();
        }
    }

    public partial interface IApplicationUserProfileRepository
    {
        /// <summary>
        /// Finds the by email.
        /// </summary>
        /// <returns>The by email.</returns>
        /// <param name="email">Email.</param>
        ApplicationUser FindByEmail(string email);

        Task<ApplicationUser> FindByEmailAsync(string email);

        Task SetEmailAsync(ApplicationUser user, string email);
    }
}
