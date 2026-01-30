using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

public class AuthenticationController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ILogger<AuthenticationController> _logger;
    private readonly ApplicationDbContext _context;

    public AuthenticationController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, ILogger<AuthenticationController> logger, ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _logger = logger;
        _context = context;
    }

    // Log IN
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }



    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Model state is invalid.");
            return View(model);
        }

        var user = await _userManager.FindByNameAsync(model.EmailUsername);
        if (user == null)
        {
            _logger.LogWarning("Login attempt failed: User with username {EmailUsername} not found.", model.EmailUsername);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName,
            model.Password,
            model.RememberMe,
            lockoutOnFailure: false
        );

        if (result.Succeeded)
        {
            _logger.LogInformation("User {EmailUsername} logged in successfully.", model.EmailUsername);

            // ✅ NOVO: Se for mecânico e ainda tiver de mudar password, força o ecrã de alteração
            if (await _userManager.IsInRoleAsync(user, "Mecanico"))
            {
                var mustChange = await _context.Mecanicos
                    .Where(m => m.UserId == user.Id && m.IsActive)
                    .Select(m => m.MustChangePassword)
                    .FirstOrDefaultAsync();

                if (mustChange)
                    return RedirectToAction("ForceChangePassword", "Authentication");
            }

            return RedirectToAction("Index", "Home");
        }
        else if (result.IsLockedOut)
        {
            _logger.LogWarning("User {EmailUsername} account locked out.", model.EmailUsername);
            return RedirectToAction("Lockout");
        }
        else
        {
            _logger.LogWarning("Login attempt failed for user {EmailUsername}.", model.EmailUsername);
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
    }


// Register
[HttpGet]
    public async Task<IActionResult> Register()
    {
        // Fetch roles dynamically from the database
        var roles = await _roleManager.Roles.ToListAsync();

        var model = new RegisterViewModel
        {
            Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Name,
                Text = r.Name
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Add the user to the selected role
                if (!string.IsNullOrEmpty(model.SelectedRole))
                {
                    await _userManager.AddToRoleAsync(user, model.SelectedRole);
                }

                _logger.LogInformation("User {Email} registered successfully.", model.Email);
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            // Log errors if user creation failed
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
                _logger.LogError("Error registering user {Email}: {Error}", model.Email, error.Description);
            }
        }
        else
        {
            _logger.LogWarning("ModelState is invalid.");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                _logger.LogWarning("ModelState Error: {ErrorMessage}", error.ErrorMessage);
            }
        }

        // Re-fetch roles dynamically if registration fails
        var roles = await _roleManager.Roles.ToListAsync();
        model.Roles = roles.Select(r => new SelectListItem
        {
            Value = r.Name,
            Text = r.Name
        }).ToList();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction("Login", "Home");
    }

    [Authorize(Roles = "Mecanico")]
    [HttpGet]
    public IActionResult ForceChangePassword()
    {
        return View(new ForceChangePasswordVM());
    }

    [Authorize(Roles = "Mecanico")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForceChangePassword(ForceChangePasswordVM vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login");

        // muda password (exige a atual, que é a temporária do email)
        var res = await _userManager.ChangePasswordAsync(user, vm.CurrentPassword, vm.NewPassword);
        if (!res.Succeeded)
        {
            foreach (var e in res.Errors)
                ModelState.AddModelError("", e.Description);

            return View(vm);
        }

        // marcar que já não precisa alterar
        var mec = await _context.Mecanicos.FirstOrDefaultAsync(m => m.UserId == user.Id);
        if (mec != null)
        {
            mec.MustChangePassword = false;
            await _context.SaveChangesAsync();
        }

        await _signInManager.SignOutAsync(); // recomendado
        TempData["Success"] = "Password alterada com sucesso. Inicia sessão novamente.";
        return RedirectToAction("Login", "Authentication");


    }


}
