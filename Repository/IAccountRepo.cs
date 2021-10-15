using ProductWebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Repository
{
    public interface IAccountRepo
    {
        Task<string> Login(LoginVM model);
        Task<Microsoft.AspNetCore.Identity.IdentityResult> SignUp(SignUpVM model);
    }
}
