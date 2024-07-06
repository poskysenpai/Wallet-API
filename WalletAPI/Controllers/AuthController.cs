using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using WalletAPI.Models;
using WalletAPI.Models.DTO;
using WalletAPI.Models.DTO.Request;
using WalletAPI.Service;
using WalletAPI.Service.Interfaces;

namespace WalletAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost]
        [Route("Login")]
       
        public async Task<IActionResult> Login([FromBody] LoginDTO requestDTO) // takes a view model as a parameter from the view
        {
            if (!ModelState.IsValid) // checks if the view model properties are valid
            {
                return BadRequest();

            }

            var response = await _authService.Login(requestDTO);
            return Ok(response);
        }


        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            
            if (!ModelState.IsValid) // checks if the view model properties are valid
            {
                return BadRequest();

            }
            var response = await _authService.Register(request);
            if (response.StatusCode != 200)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}
