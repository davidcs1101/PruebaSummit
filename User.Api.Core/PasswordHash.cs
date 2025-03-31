using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace User.Api.Core
{
    public static class PasswordHash
    {
        public static string Generate(string userName, string password) {
            string clave = userName + password;
            SHA1CryptoServiceProvider SHA1 = new SHA1CryptoServiceProvider();
            byte[] vectoBytes = System.Text.Encoding.UTF8.GetBytes(clave);
            byte[] inArray = SHA1.ComputeHash(vectoBytes);
            SHA1.Clear();
            return Convert.ToBase64String(inArray);
        }

        public static ServiceState Validate(string userName, string password, string hash) {
            var claveEnviada = Generate(userName, password);
            if (claveEnviada == hash)
            {
                return ServiceState.Accepted;
            }
            else {
                return ServiceState.Rejected;
            }
        }
    }
}
