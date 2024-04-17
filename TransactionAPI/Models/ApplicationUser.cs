using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TransactionAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }

   
}
