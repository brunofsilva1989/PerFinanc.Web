using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Data;

namespace PerFinanc.Web.Auth
{
    public class PerFinancIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public PerFinancIdentityDbContext(DbContextOptions<PerFinancIdentityDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
