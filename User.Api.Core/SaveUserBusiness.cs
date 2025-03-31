using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace User.Api.Core
{
    public class SaveUserBusiness
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public ServiceState Process() {

            var url = "https://randomuser.me/api/";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            var response = _httpClient.SendAsync(request).Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Error al tratar de obtener los datos randon del usuarios");
            }

            var respuesta = response.Content.ReadAsStringAsync().Result;
            var respuestaJson = JsonConvert.DeserializeObject<UserDto>(respuesta);
            var firstElement = respuestaJson.results.FirstOrDefault();
            var userName = firstElement.login.username;
            var claveEncriptada = PasswordHash.Generate(userName , firstElement.login.password);

            User usuario = new User()
            {
                FirstName = firstElement.name.first,
                LastName = firstElement.name.last,
                Email = firstElement.email,
                Password = claveEncriptada,
                RawPassword = userName + firstElement.login.password,
                Birthday = firstElement.dob.date,
                Username = userName
            };

            try
            {
                Database.Instance.ExecuteNonQuery(usuario);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al intentar crear el usuario {ServiceState.Rejected}: " + e.Message);
            }
            
            return ServiceState.Accepted;
        }
    }
}
