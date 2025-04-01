using Microsoft.AspNetCore.Mvc;
using User.Api.Core;
using User.Api.Core.ExceptionsHandlers;

namespace SummitApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiSummitController : Controller
    {
        private readonly GetUserBusiness _getUserBusiness;
        private readonly SaveUserBusiness _saveUserBusiness;
        private readonly ValidateUserBusiness _validateUserBusiness;

        public ApiSummitController(GetUserBusiness getUserBusiness, SaveUserBusiness saveUserBusiness, ValidateUserBusiness validateUserBusiness)
        {
            _getUserBusiness = getUserBusiness;
            _saveUserBusiness = saveUserBusiness;
            _validateUserBusiness = validateUserBusiness;
        }

        [HttpGet("GetUsers")]
        public string Get(string userName = "") {
            return _getUserBusiness.Process(userName);
        }

        [HttpPost("SaveUser")]
        public ServiceState Save() {
            return _saveUserBusiness.Process();
        }

        [HttpPost("ValidateUser")]
        public IActionResult Validate(string userName, string password)
        {
            try
            {
                _validateUserBusiness.Process(userName, password);
                return Ok();
            }
            catch (UserNotFoundException)
            {
                return NotFound("Usuario no encontrado"); // 404 Not Found
            }
            catch (InvalidCredentialsException)
            {
                return StatusCode(403,"Datos incorrectos"); // 403 Forbidden
            }
        }
    }
}
