using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalletAPI.Data;
using WalletAPI.Models.DTO;
using WalletAPI.Models.DTO.Request;
using WalletAPI.Models.Entities;
using WalletAPI.Service;

namespace WalletAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private static List<Wallets> wallets = new List<Wallets>();
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _userManager;

        public WalletsController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // POST: api/wallets
        //[HttpPost]
        //[Route("Promote/Demote-User")]
        //[Authorize()]
        //public IActionResult PromoteDemote(string UserId)
        //{
        //    var user = _userManager.FindByIdAsync(UserId);  

        //    if (user == null) { 


        //    }

        //    var roles = _userManager.GetRolesAsync()
        //    var existingRoles =  _userManager.RemoveFromRoleAsync()
        //    foreach (var role in existingRoles)
        //    {
        //        await _userManager.RemoveFromRoleAsync(userId, role);
        //    }

        //    // Add new role
        //    await _userManager.AddToRoleAsync(userId, model.NewRole);
        //    //var userClaims = User.Claims;

        //    //// Check if the user has a role claim
        //    //var roleClaim = userClaims.FirstOrDefault(c => c.Type == "role");

        //    //if (roleClaim != null && roleClaim.Value == "admin")
        //    //{
        //    //    return BadRequest("An admin should not have any wallet");
        //    //}





        //    // Generate a unique wallet ID (you can use any method you prefer)
        //    var walletId =  Guid.NewGuid().ToString();

        //    // Create the wallet object
        //    var wallet = new Wallets
        //    {
        //        Id = walletId,
        //        UsersId = request.UserId,
        //        Currency = request.Currency,
        //        Balance = 0

        //    }; 

        //    // Add the wallet to the list

        //    _context.Wallets.Add(wallet);
        //    _context.SaveChanges();

        //    return CreatedAtAction(nameof(GetWallet), new { id = walletId }, wallet);

        //}

        // GET: api/wallets/{id}
        [HttpGet]
        [Route("GetWallet")]
        public async Task<Wallets> GetWallet(string id)
        {
            //var wallet = _context.Wallets.FirstOrDefault(w => w.Id == id );
            //if (wallet == null)
            //{
            //    return NotFound();
            //}

            var wallet = await _context.Wallets.Where(x => x.Id == id).Select(x => new Wallets()
            {
                Id = id,
                Balance = x.Balance,
                Currency = x.Currency,
                UsersId = x.UsersId,
            }).FirstOrDefaultAsync();

            return wallet;
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]
        [HttpPost("Promote/Demote")]
        public async Task<bool> PromoteOrDemoteUser(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            if (userRoles.Contains("admin"))
            {
                return false;
            }

            if (roleName.ToLower() == "admin")
            {
                if (userRoles.Contains("noob") || userRoles.Contains("elite"))
                {
                    return false;
                }

            }

            var removeResult = await _userManager.RemoveFromRolesAsync(user, userRoles);

            if (!removeResult.Succeeded)
            {
                return false;
            }

            var result = await _userManager.AddToRoleAsync(user, roleName.ToLower());

            return result.Succeeded;
        }
    }

    // Request model for creating a wallet
    
}
