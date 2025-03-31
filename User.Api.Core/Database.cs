using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Api.Core
{
    public abstract class Database
    {
        public abstract void ExecuteNonQuery(User user);
        public abstract string GetJson(string username = "");

        public abstract void ValidateUser(string userName, string password);
        public static Database Instance { get; set; }
    }
}
