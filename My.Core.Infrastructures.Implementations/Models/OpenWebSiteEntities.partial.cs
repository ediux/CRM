using My.Core.Infrastructures.DAL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Core.Infrastructures.Implementations.Models
{
    public partial class OpenWebSiteEntities
    {
        private bool _requireuniqueEmail=false;
        public bool RequireUniqueEmail { get { return _requireuniqueEmail; } set { _requireuniqueEmail = value; } }

        private Hashtable _repositories;

        public OpenWebSiteEntities(string nameorconnectionstring)
            : base(nameorconnectionstring)
        {
            _requireuniqueEmail = false;
        }

        public static OpenWebSiteEntities Create()
        {
            return new OpenWebSiteEntities();
        }
    }
}
