using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using User.Api.Core.ExceptionsHandlers;
using XAct.Users;

namespace User.Api.Core
{
    public class FileDatabase : Database
    {
        static List<User> UsersList = new List<User>();

        public override void ExecuteNonQuery(User user) {
            var usuarioExiste = UsersList.Where(u => u.Username == user.Username).FirstOrDefault();
            if (usuarioExiste != null)
            {
                string fileName = usuarioExiste.Username + ".txt";
                string filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                string jsonUsuario = JsonSerializer.Serialize(usuarioExiste);

                using StreamWriter writer = new StreamWriter(filePath, false);
                writer.WriteLine(jsonUsuario);
            }
        }

        public override string GetJson(string username = "")
        {
            if (!string.IsNullOrEmpty(username))
            {
                // Leer un archivo específico
                string fileName = username + ".txt";
                string filePath = Path.Combine(AppContext.BaseDirectory, fileName);

                if (File.Exists(filePath))
                {
                    string jsonString = File.ReadAllText(filePath);
                    User user = JsonSerializer.Deserialize<User>(jsonString);
                    return JsonSerializer.Serialize(user);
                }
                else
                {
                    throw new UserNotFoundException();//404
                }
            }
            else
            {
                // Leer todos los archivos
                List<User> allUsers = new List<User>();
                string[] files = Directory.GetFiles(AppContext.BaseDirectory, "*.txt");

                foreach (string file in files)
                {
                    try
                    {
                        string jsonString = File.ReadAllText(file);
                        User user = JsonSerializer.Deserialize<User>(jsonString);
                        allUsers.Add(user);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Error al tratar de deserializar los archivos.");
                    }
                }

                return JsonSerializer.Serialize(allUsers);
            }
        }

        public override void ValidateUser(string userName, string password) {

        }
    }
}
