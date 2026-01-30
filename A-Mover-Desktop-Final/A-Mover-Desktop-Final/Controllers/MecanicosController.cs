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
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace A_Mover_Desktop_Final.Controllers
{
    public class MecanicosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        public MecanicosController(ApplicationDbContext context, UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _context = context;
            _userManager = userManager;
            _config = config;
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

            // Oficina autenticada é o próprio utilizador (UserId)
            string oficinaUserId = _userManager.GetUserId(User)!;
            if (string.IsNullOrWhiteSpace(oficinaUserId))
                return Unauthorized();

            string email = vm.Email.Trim().ToLower();

            // Evitar duplicados por oficina
            bool exists = await _context.Mecanicos
                .AnyAsync(m => m.OficinaId == oficinaUserId && m.Email == email);

            if (exists)
            {
                ModelState.AddModelError(nameof(vm.Email), "Já existe um mecânico com este email nesta oficina.");
                return View(vm);
            }

            // Password temporária + username único
            string tempPassword = GenerateTemporaryPassword();
            string usernameBase = BuildUsernameFromName(vm.Nome);
            string username = await EnsureUniqueUsernameAsync(usernameBase);

            IdentityUser? user = null;
            Mecanico? mecanico = null;

            try
            {
                user = new IdentityUser
                {
                    UserName = username,
                    Email = email,
                    EmailConfirmed = true
                };

                // Criar conta Identity
                var createUser = await _userManager.CreateAsync(user, tempPassword);
                if (!createUser.Succeeded)
                {
                    foreach (var e in createUser.Errors)
                        ModelState.AddModelError("", e.Description);

                    return View(vm);
                }

                // Atribuir role
                var roleRes = await _userManager.AddToRoleAsync(user, "Mecanico");
                if (!roleRes.Succeeded)
                {
                    await _userManager.DeleteAsync(user);

                    foreach (var e in roleRes.Errors)
                        ModelState.AddModelError("", e.Description);

                    return View(vm);
                }

                // Criar registo Mecanico
                mecanico = new Mecanico
                {
                    Nome = vm.Nome.Trim(),
                    Email = email,
                    Telemovel = vm.Telemovel?.Trim(),
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,

                    OficinaId = oficinaUserId,
                    UserId = user.Id,
                    MustChangePassword = true
                };

                _context.Mecanicos.Add(mecanico);
                await _context.SaveChangesAsync();

                // Enviar email com credenciais (SMTP2GO)
                await SendMechanicCredentialsEmailAsync(
                    toEmail: email,
                    nome: vm.Nome.Trim(),
                    username: username,
                    tempPassword: tempPassword
                );

                TempData["Success"] = "Mecânico criado. As credenciais foram enviadas por email.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // rollback do registo do mecânico (se já tiver sido gravado)
                try
                {
                    if (mecanico != null)
                    {
                        _context.Mecanicos.Remove(mecanico);
                        await _context.SaveChangesAsync();
                    }
                }
                catch { /* opcional: log */ }

                // rollback da conta Identity
                try
                {
                    if (user != null)
                        await _userManager.DeleteAsync(user);
                }
                catch { /* opcional: log */ }

                ModelState.AddModelError("", "Não foi possível concluir a criação do mecânico (envio de email incluído). Tenta novamente.");
                // Em DEV podes manter este detalhe, em PROD é melhor registar em log e não mostrar.
                ModelState.AddModelError("", $"Detalhe: {ex.Message}");

                return View(vm);
            }
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




        private static string GenerateTemporaryPassword(int length = 12)
        {
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string digits = "0123456789";
            const string symbols = "!@#$%^&*()-_=+.";

            if (length < 8) length = 8;

            char Pick(string s) => s[RandomNumberGenerator.GetInt32(s.Length)];

            var chars = new List<char>
            {
                Pick(lower),
                Pick(upper),
                Pick(digits),
                Pick(symbols)
            };

            string all = lower + upper + digits + symbols;
            while (chars.Count < length) chars.Add(Pick(all));

            // baralhar (Fisher–Yates)
            for (int i = chars.Count - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (chars[i], chars[j]) = (chars[j], chars[i]);
            }

            return new string(chars.ToArray());
        }


        private static string BuildUsernameFromName(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome)) return "mecanico";

            var trimmed = nome.Trim().ToLower();

            // letras/dígitos e espaços -> hífen
            var cleaned = new string(trimmed.Select(c =>
                char.IsLetterOrDigit(c) ? c :
                char.IsWhiteSpace(c) ? '-' : '\0'
            ).Where(c => c != '\0').ToArray());

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
                candidate = $"{baseUsername}{i}";
            }

            return candidate;
        }


        private async Task SendMechanicCredentialsEmailAsync(string toEmail, string nome, string username, string tempPassword)
        {
            var host = _config["Smtp2Go:Host"]!;
            var port = int.Parse(_config["Smtp2Go:Port"] ?? "2525");
            var enableSsl = bool.Parse(_config["Smtp2Go:EnableSsl"] ?? "true");
            var smtpUser = _config["Smtp2Go:Username"]!;
            var smtpPass = _config["Smtp2Go:Password"]!;
            var fromEmail = _config["Smtp2Go:FromEmail"]!;
            var fromName = _config["Smtp2Go:FromName"] ?? "A-MoVeR";



            if (string.IsNullOrWhiteSpace(host) || string.IsNullOrWhiteSpace(smtpUser) ||
                string.IsNullOrWhiteSpace(smtpPass) || string.IsNullOrWhiteSpace(fromEmail))
                throw new InvalidOperationException("Configuração SMTP2GO incompleta no appsettings.json.");

            var subject = "Acesso criado - Password temporária";
            var body =
                    $@"Olá {nome},

            A tua conta de mecânico foi criada.

            Username: {username}
            Password temporária: {tempPassword}

            No primeiro login vais ser obrigado(a) a alterar a password.";

            Console.WriteLine($"SMTP Host={host} Port={port} User={smtpUser} From={fromEmail}");


            await EmailHelper.SendAsync(
                host, port, enableSsl,
                smtpUser, smtpPass,
                fromEmail, fromName,
                toEmail, subject, body
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deactivate(int id)
        {
            string oficinaUserId = _userManager.GetUserId(User)!;

            // só mexe no mecânico se pertencer a esta oficina
            var mecanico = await _context.Mecanicos
                .FirstOrDefaultAsync(m => m.Id == id && m.OficinaId == oficinaUserId);

            if (mecanico == null) return NotFound();

            mecanico.IsActive = false;

            // bloquear login do mecânico (se houver conta Identity associada)
            if (!string.IsNullOrWhiteSpace(mecanico.UserId))
            {
                var user = await _userManager.FindByIdAsync(mecanico.UserId);
                if (user != null)
                {
                    await _userManager.SetLockoutEnabledAsync(user, true);
                    await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Mecânico desativado com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reactivate(int id)
        {
            string oficinaUserId = _userManager.GetUserId(User)!;

            var mecanico = await _context.Mecanicos
                .FirstOrDefaultAsync(m => m.Id == id && m.OficinaId == oficinaUserId);

            if (mecanico == null) return NotFound();

            mecanico.IsActive = true;

            // desbloquear login
            if (!string.IsNullOrWhiteSpace(mecanico.UserId))
            {
                var user = await _userManager.FindByIdAsync(mecanico.UserId);
                if (user != null)
                {
                    // remover lockout
                    await _userManager.SetLockoutEndDateAsync(user, null);
                }
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Mecânico reativado com sucesso.";
            return RedirectToAction(nameof(Index));
        }


        private async Task<int> GetMecanicoIdAsync()
        {
            var userId = _userManager.GetUserId(User);

            var mecanicoId = await _context.Mecanicos
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                .Select(m => m.Id)
                .FirstOrDefaultAsync();

            if (mecanicoId == 0)
                throw new Exception("Mecânico não encontrado para o utilizador autenticado.");

            return mecanicoId;
        }

        // LISTA: só serviços atribuídos ao mecânico autenticado
        public async Task<IActionResult> MeusAgendamentos()
        {
            int mecanicoId = await GetMecanicoIdAsync();

            var lista = await _context.Servico
                .AsNoTracking()
                .Where(s => s.IDMecanico == mecanicoId)
                .Include(s => s.Mota)                // ajusta conforme o teu model
                .ThenInclude(m => m.ModeloMota)      // se existir
                .OrderByDescending(s => s.DataServico)
                .ToListAsync();

            return View(lista);
        }



    }
}
