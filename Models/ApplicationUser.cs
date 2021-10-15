using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductWebApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime DateStamp { get; set; }
    }
}
