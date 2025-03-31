using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Api.Core
{
    public class ValidateUserBusiness
    {
        public void Process(string userName, string password) { 
            Database.Instance.ValidateUser(userName, password);
        }
    }
}
