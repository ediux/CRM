using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Base
{
    public static class OpenCoreWebExtensions
    {
        [DebuggerStepThrough]
        public async static Task<bool> TwoFactorBrowserRememberedAsync(this IAuthenticationManager manager, int userId)
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }
            var result = await manager.AuthenticateAsync(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
            return (result != null && result.Identity != null && result.Identity.GetUserId<int>() == userId);
        }
    }
}
