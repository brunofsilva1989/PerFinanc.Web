using Microsoft.AspNetCore.Identity;

namespace PerFinanc.Web.Auth
{
    public class Seed
    {
        public static async Task EnsureAsync(IServiceProvider sp)
        {
            using var scope = sp.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Seed>>();

            var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var users = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            foreach (var role in new[] { "Admin", "Operator", "Viewer" })
            {
                if (!await roles.RoleExistsAsync(role))
                {
                    var r = await roles.CreateAsync(new IdentityRole(role));
                    if (!r.Succeeded) logger.LogError("Erro criando role {Role}: {Errors}",
                        role, string.Join(" | ", r.Errors.Select(e => e.Description)));
                }
            }

            var adminEmail = "suporte.bfs@gmail.com";
            var admin = await users.FindByEmailAsync(adminEmail);
            if (admin is null)
            {
                admin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var u = await users.CreateAsync(admin, "Admin@12345");
                if (!u.Succeeded) logger.LogError("Erro criando admin: {Errors}",
                    string.Join(" | ", u.Errors.Select(e => e.Description)));

                var ar = await users.AddToRoleAsync(admin, "Admin");
                if (!ar.Succeeded) logger.LogError("Erro adicionando admin à role: {Errors}",
                    string.Join(" | ", ar.Errors.Select(e => e.Description)));
            }
        }

    }
}
