using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using XAct.Users;

namespace User.Api.Core
{
    public class MemoryDatabase : Database
    {
        static List<User> UsersList = new List<User>();

        public override void ExecuteNonQuery(User user) {
            var usuarioExiste = UsersList.Where(u => u.Username == user.Username).Any();
            if (!usuarioExiste) {
                UsersList.Add(user);
            }
        }

        public override string GetJson(string username = "")
        {
            string jsonTexto = "";
            if (string.IsNullOrEmpty(username))
            {
                jsonTexto = JsonSerializer.Serialize(UsersList).ToString();
            }
            else {
                var usuario = UsersList.Where(u => u.Username == username).FirstOrDefault();
                if (usuario!=null)
                {
                    jsonTexto = JsonSerializer.Serialize(usuario).ToString();
                }
            }
            return jsonTexto;
        }

        public override void ValidateUser(string userName, string password) {
            var usuarioExiste = UsersList.Where(u => u.Username == userName).FirstOrDefault();
            if (usuarioExiste==null)
            {
                throw new UserNotFoundException();//404
            }

            if (PasswordHash.Validate("", password, usuarioExiste.Password) == ServiceState.Rejected)
            {
                throw new InvalidCredentialsException();//404
            }

        }


    }

    public class UserNotFoundException : Exception { } // Excepción para 404
    public class InvalidCredentialsException : Exception { } // Excepción para 403
}
