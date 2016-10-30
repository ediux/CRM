using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class EFRepository<T> where T : class
    {
        protected virtual int GetCurrentLoginedUserId()
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            }

            return -1;
        }

        #region Helper Functions

        /// <summary>
        /// Writes the error log.
        /// </summary>
        /// <returns>The error log.</returns>
        /// <param name="ex">Ex.</param>
        protected virtual void WriteErrorLog(Exception ex)
        {
            System.Diagnostics.Debug.Write(string.Format("{0},{1}", ex.Message, ex.StackTrace));
        }
        #endregion
    }
}
