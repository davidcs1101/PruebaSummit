using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Api.Core
{
    public class GetUserBusiness
    {
        public string Process(string userName = "") {
            return Database.Instance.GetJson(userName);
        }
    }
}
