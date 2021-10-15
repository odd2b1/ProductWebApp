using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductWebApp.Models;
using ProductWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProductWebApp.Repository
{
    public class AccountRepo : IAccountRepo
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountRepo(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<string> Login(LoginVM model)
        {
            var userobj = await _userManager.FindByEmailAsync(model.Email);
            if (userobj == null)
            {
                return null;
            }
            var result = await _signInManager.PasswordSignInAsync(userobj, model.Password, true, false);
            if (!result.Succeeded)
            {
                return null;
            }
            var authclaims = new Claim[]
            {
                new Claim(ClaimTypes.Name,model.Email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Id",userobj.Id)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var signincred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authclaims,
                signingCredentials: signincred);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<IdentityResult> SignUp(SignUpVM model)
        {
            var userobj = new ApplicationUser()
            {
                DateStamp = DateTime.Now,
                UserName = model.Email,
                Email = model.Email
            };
            var result = await _userManager.CreateAsync(userobj, model.Password);
            return result;
        }
    }

    public class Response
    {
        public bool isSuccess { get; set; }
        public string Error { get; set; }

    }
}


