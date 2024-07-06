using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WalletAPI.Data;
using WalletAPI.Migrations;
using WalletAPI.Models;
using WalletAPI.Models.DTO;
using WalletAPI.Models.DTO.Request;
using WalletAPI.Models.Entities;
using WalletAPI.Models.Response;
using WalletAPI.Service.Interfaces;

namespace WalletAPI.Service
{
    public class AuthService: IAuthService
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        
        private readonly AppDbContext _context;
        public AuthService(IConfiguration config, UserManager<Users> userManager, SignInManager<Users> signInManager, AppDbContext context)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
           
            _context = context;
        }

        //public string GenerateJWT(Users user, List<string> roles, List<Claim> claims)
        //{
        //    var myClaims = new List<Claim>();

        //    myClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
        //    myClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
        //    myClaims.Add(new Claim(ClaimTypes.Email, user.Email));

        //    foreach (var role in roles)
        //    {
        //        myClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    foreach (var claim in claims)
        //    {
        //        myClaims.Add(new Claim(claim.Type, claim.Value));
        //    }
        //    var key = Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value);
        //    var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
        //        SecurityAlgorithms.HmacSha256);

        //    var securityToken = new JwtSecurityToken(
        //        claims: myClaims,
        //        expires: DateTime.UtcNow.AddDays(1),
        //        signingCredentials: signingCredentials
        //    );

        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var token = tokenHandler.WriteToken(securityToken);
        //    return token;
        //}

        //public async Task<LoginResultDTO> Login(string email, string password)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user != null)
        //    {
        //        if (await _userManager.CheckPasswordAsync(user, password))
        //        {
        //            var roles = await _userManager.GetRolesAsync(user);
        //            var claims = new List<Claim>();
        //            return new LoginResultDTO
        //            {
        //                IsSuccess = true,
        //                Message = "Login sucessfully!",
        //                UserId = user.Id,
        //                Roles = roles.ToList(),
        //                Token = this.GenerateJWT(user, roles.ToList(), claims)
        //            };
        //        }
        //    }
        //    return new LoginResultDTO
        //    {
        //        IsSuccess = false,
        //        Message = "Invalid credential"
        //    };

        //}

        //public async Task<Dictionary<string, string>> ValidateLoggedInUser(ClaimsPrincipal user, string userId)
        //{
        //    var loggedInUser = await _userManager.GetUserAsync(user);
        //    if (loggedInUser == null || loggedInUser.Id != userId)
        //    {
        //        return new Dictionary<string, string> {
        //            { "Code", "400" },
        //            { "Message", "Access denied! Id provided does not match loggedIn user." }
        //        };
        //    }

        //    return new Dictionary<string, string> {
        //            { "Code", "200" },
        //            { "Message", "ok" }
        //        };
        //}

        /*public async Task<APIresponse<string>> LoginAsync(LoginDTO requestDTO)
        {
            var user = await _userManager.FindByEmailAsync(requestDTO.Email);

            var password = await _signInManager.CheckPasswordSignInAsync(user, requestDTO.Password, false); ;// checks if the email inputted exist
            if (!password.Succeeded)
            {
                return new APIresponse<string>() { Message = "Login Failed", StatusCode = 401 };
            }
            if (user != null)  // email exist
            {
                var result = await _signInManager.PasswordSignInAsync(user, requestDTO.Password, false, false); // checks if inputted password matches the email
                if (!result.Succeeded)
                {
                    return new APIresponse<string>() { Message = "Login Failed", StatusCode = 401 };
                }
                // If successfull generate jwt for user
                var jwt = await Generate(requestDTO);
                return new APIresponse<string>() { Message = "Login Successful", StatusCode = 200, Data = jwt };
            }
            return new APIresponse<string>() { Message = "Login Failed", StatusCode = 401 };
        }*/

        /*public async Task<string> Generate(LoginDTO user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var users = await _userManager.FindByEmailAsync(user.Email);

            var roles = await _userManager.GetRolesAsync(users);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Email),
                new Claim(ClaimTypes.Email, user.Email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            foreach (var claim in claims)
            {
                claims.Add(new Claim(claim.Type, claim.Value));
            }
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }*/
        public async Task<LoginResult> Login(LoginDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var claims = await _userManager.GetClaimsAsync(user);
                return new LoginResult
                {
                    IsSuccess = true,
                    Message = "Login successful",
                    Token = Generate(user, roles.ToList(), claims.ToList()),
                    UserId = user.Id,
                    Roles = roles.ToList()
                };

            }

            return new LoginResult
            {
                IsSuccess = false,
                Message = "Invalid login details"
            };
        }

        public string Generate(Users user, List<string> roles, List<Claim> claims)
        {
            var myClaims = new List<Claim>();

            myClaims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            myClaims.Add(new Claim(ClaimTypes.Name, user.UserName));
            myClaims.Add(new Claim(ClaimTypes.Email, user.Email));

            foreach (var role in roles)
            {
                myClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            foreach (var claim in claims)
            {
                myClaims.Add(new Claim(claim.Type, claim.Value));
            }
            var key = Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value);
            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256);

            var securityToken = new JwtSecurityToken(
                claims: myClaims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: signingCredentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.WriteToken(securityToken);
            return token;
        }
        public async Task<APIresponse<RegisterResponse>> Register(RegisterRequestDTO request)

        {
            var role = request.UserType.ToLower();
            if (role != "noob" && role != "elite")
            {
                return new APIresponse<RegisterResponse>() { Message = "UserType is Unable to register", StatusCode = 400 };
            }
           

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user != null)
            {
                return new APIresponse<RegisterResponse>() { Message = "Email Exist", StatusCode = 400 };
            }
            
             
            var userToAdd = new Users
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                
                
            };

            var identityResult = await _userManager.CreateAsync(userToAdd, request.Password);
            if (!identityResult.Succeeded)
            {
                return new APIresponse<RegisterResponse>() { Message = " Registration Failed", StatusCode = 400 };
            }

            if (request.UserType == "noob" && request.Currencies.Count > 1)
            {
                return new APIresponse<RegisterResponse>() { Message = "Your Usertype can only have one currency", StatusCode = 400 };
            }

            
            

            await _userManager.AddToRoleAsync(userToAdd, request.UserType);
            RegisterResponse responseDTO = new RegisterResponse();
            responseDTO.Email = userToAdd.Email;
            responseDTO.UserName = userToAdd.Email;
            responseDTO.FirstName = userToAdd.FirstName;
            responseDTO.LastName = userToAdd.LastName;
            responseDTO.PhoneNumber = userToAdd.PhoneNumber;
            responseDTO.UserId = userToAdd.Id;

            



            foreach (var currency in request.Currencies)
            {
                // Check if the currency is valid or exists in your system
                if(!Enum.IsDefined(typeof(Currency), currency.Type))
                {
                    return new APIresponse<RegisterResponse>() { Message = "Invalid Currency code", StatusCode = 400 };
                }
                // Perform any additional validation if necessary

                var walletId = Guid.NewGuid().ToString();
                // Create a wallet for the current currency
                var wallet = new Wallets
                {
                    Id = walletId,
                  //IsMain = true,
                    Currency = currency.Type,
                    Balance = 0,
                    UsersId = userToAdd.Id,
                    // Other properties initialization if needed
                };
                
               

                // Add the wallet to your database or any storage mechanism
                _context.Wallets.Add(wallet);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            
        
           





            return new APIresponse<RegisterResponse>() { Message = "Registeration Succeded", StatusCode = 200, Data = responseDTO };
        }

      
    }
}

