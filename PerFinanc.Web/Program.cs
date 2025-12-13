using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PerFinanc.Web.Auth;
using PerFinanc.Web.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<PerFinancDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<PerFinancIdentityDbContext>(options =>   
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews(options =>
{
    // Tudo exige login por padrão
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();

    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.User.RequireUniqueEmail = true;
        opt.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<PerFinancIdentityDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

builder.Services.ConfigureApplicationCookie(c =>
{
    // Se usar suas views (AccountController):
    c.LoginPath = "/Conta/Login";  
    c.AccessDeniedPath = "/Conta/AccessDenied";

    // Se usar a UI padrão do Identity (e mapeou Razor Pages), troque para:
    // c.LoginPath = "/Identity/Account/Login";
    // c.AccessDeniedPath = "/Identity/Account/AccessDenied";

    c.SlidingExpiration = true;
    c.Cookie.Name = ".PerFinanc.Web.Auth";
    c.Cookie.HttpOnly = true;
    c.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    c.Cookie.SameSite = SameSiteMode.Lax;
});

builder.Services.AddAuthorization(o =>
{
    o.AddPolicy("AdminOnly", p => p.RequireRole("Admin")); // <— só Admin
    o.AddPolicy("CanManageDevices", p => p.RequireRole("Admin", "Operator"));
    o.AddPolicy("CanManageSensors", p => p.RequireRole("Admin", "Operator"));
    o.AddPolicy("CanManageConfiguration", p => p.RequireRole("Admin")); // (se quiser manter o nome)
    o.AddPolicy("CanManageHistory", p => p.RequireRole("Admin", "Viewer"));
    o.AddPolicy("CanReadTelemetry", p => p.RequireRole("Admin", "Operator", "Viewer"));
});


var app = builder.Build();

//Injeta o Seed 
using (var scope = app.Services.CreateScope())
{
    await Seed.EnsureAsync(scope.ServiceProvider);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
