using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace A_Mover_Desktop_Final.Controllers
{
    public class MecanicosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MecanicosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Mecanicos
        public async Task<IActionResult> Index()
        {
            string oficinaUserId = _userManager.GetUserId(User)!;

            var list = await _context.Mecanicos
                .Where(m => m.OficinaId == oficinaUserId)
                .OrderByDescending(m => m.IsActive)
                .ThenBy(m => m.Nome)
                .ToListAsync();

            return View(list);
        }

        // GET: Mecanicos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecanico = await _context.Mecanicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mecanico == null)
            {
                return NotFound();
            }

            return View(mecanico);
        }

        // GET: Mecanicos/Create
        [HttpGet]
        public IActionResult Create() => View(new MecanicoCreate());


        // POST: Mecanicos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MecanicoCreate vm)
        {
            if (!ModelState.IsValid) return View(vm);

            // ✅ a oficina autenticada é o próprio utilizador (UserId)
            string oficinaUserId = _userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(oficinaUserId))
                throw new InvalidOperationException("Utilizador oficina não autenticado.");

            string email = vm.Email.Trim().ToLower();

            // ✅ impedir duplicados por oficina (por UserId da oficina)
            bool exists = await _context.Mecanicos
                .AnyAsync(m => m.OficinaId == oficinaUserId && m.Email == email);

            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Email), "Já existe um mecânico com este email nesta oficina.");
                return View(vm);
            }

            // ✅ password default (tua regra)
            string defaultPassword = BuildDefaultPassword(vm.Nome);
            string username = await EnsureUniqueUsernameAsync(vm.Nome);

            // ✅ criar conta Identity do mecânico
            var user = new IdentityUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true
            };

            var createUser = await _userManager.CreateAsync(user, defaultPassword);
            if (!createUser.Succeeded)
            {
                foreach (var e in createUser.Errors)
                    ModelState.AddModelError("", e.Description);

                return View(vm);
            }

            // ✅ role mecânico
            await _userManager.AddToRoleAsync(user, "Mecanico");

            // ✅ criar registo Mecanico ligado à oficina (UserId) + UserId do mecânico
            var mecanico = new Mecanico
            {
                Nome = vm.Nome.Trim(),
                Email = email,
                Telemovel = vm.Telemovel?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                OficinaId = oficinaUserId,  // ✅ aqui está a associação correta
                UserId = user.Id                // ✅ user do mecânico
            };

            _context.Mecanicos.Add(mecanico);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Mecânico criado. Password por defeito: {defaultPassword}";
            return RedirectToAction(nameof(Index));
        }

        // GET: Mecanicos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecanico = await _context.Mecanicos.FindAsync(id);
            if (mecanico == null)
            {
                return NotFound();
            }
            return View(mecanico);
        }

        // POST: Mecanicos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Email,Telemovel,IsActive,CreatedAt,OficinaId,UserId")] Mecanico mecanico)
        {
            if (id != mecanico.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mecanico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MecanicoExists(mecanico.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mecanico);
        }

        // GET: Mecanicos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mecanico = await _context.Mecanicos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (mecanico == null)
            {
                return NotFound();
            }

            return View(mecanico);
        }

        // POST: Mecanicos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mecanico = await _context.Mecanicos.FindAsync(id);
            if (mecanico != null)
            {
                _context.Mecanicos.Remove(mecanico);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MecanicoExists(int id)
        {
            return _context.Mecanicos.Any(e => e.Id == id);
        }

     


        private static string BuildDefaultPassword(string nome)
        {
            var token = (nome ?? "").Trim();
            token = token.Split(' ', StringSplitOptions.RemoveEmptyEntries).FirstOrDefault() ?? "User";

            // mantém só letras (evita espaços, hífens, etc.)
            token = new string(token.Where(char.IsLetter).ToArray());
            if (string.IsNullOrWhiteSpace(token)) token = "User";

            token = char.ToUpper(token[0]) + token.Substring(1).ToLower();

            return $"mecanico{token}.";
        }


        private static string BuildUsernameFromName(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return "mecanico";

            // "João Silva" -> "joao-silva"
            var trimmed = nome.Trim().ToLower();

            // mantém letras/dígitos e troca espaços por hífen
            var cleaned = new string(trimmed.Select(c =>
                char.IsLetterOrDigit(c) ? c :
                char.IsWhiteSpace(c) ? '-' : '\0'
            ).Where(c => c != '\0').ToArray());

            // remove hífens repetidos
            while (cleaned.Contains("--")) cleaned = cleaned.Replace("--", "-");
            cleaned = cleaned.Trim('-');

            return string.IsNullOrWhiteSpace(cleaned) ? "mecanico" : cleaned;
        }

        private async Task<string> EnsureUniqueUsernameAsync(string baseUsername)
        {
            var candidate = baseUsername;
            int i = 1;

            while (await _userManager.FindByNameAsync(candidate) != null)
            {
                i++;
                candidate = $"{baseUsername}{i}"; // ex: "joao-silva2", "joao-silva3"
            }

            return candidate;
        }
    }
}
