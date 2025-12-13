using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PerFinanc.Web.Auth;

namespace PerFinanc.Web.Controllers
{
    public class ContaController : Controller
    {

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<ContaController> _logger;

        public ContaController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<ContaController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        // GET: /Conta/
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };
            return View(model);
        }

        // POST: /Conta/Login
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var input = model.UserNameOrEmail;

            var user = await _userManager.FindByNameAsync(input)
                    ?? await _userManager.FindByEmailAsync(input);

            if (user == null)
            {
                ModelState.AddModelError("", "Usuário/senha inválidos.");
                return View(model);
            }

            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}
            var result = await _signInManager.PasswordSignInAsync(
                model.UserNameOrEmail,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: true);
            if (result.Succeeded)
            {
                _logger.LogInformation("Usuário {UserName} logado com sucesso.", model.UserNameOrEmail);
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning("Conta de usuário {UserName} bloqueada.", model.UserNameOrEmail);
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tentativa de login inválida.");
                return View(model);
            }
        }


        // POST: /Conta/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Usuário deslogado.");
            return RedirectToAction("Index", "Home");
        }

        // GET: /Conta/Register
        [AllowAnonymous]
        [HttpGet]        
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new RegisterViewModel());
        }

        // POST: /Conta/Register
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(model);

            var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("O usuário criou uma nova conta com senha.");

                const string defaultRole = "Viewer"; 

                // garante que a role existe
                if (!await _roleManager.RoleExistsAsync(defaultRole))
                    await _roleManager.CreateAsync(new IdentityRole(defaultRole));

                // atribui Viewer SEM depender do model
                var addRole = await _userManager.AddToRoleAsync(user, defaultRole);
                if (!addRole.Succeeded)
                {
                    foreach (var error in addRole.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return View(model);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Usuário conectado após o registro.");

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }


        // GET: /Conta/AccessDenied
        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
    }
}
