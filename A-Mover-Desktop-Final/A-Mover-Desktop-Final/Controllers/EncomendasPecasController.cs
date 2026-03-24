using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class EncomendasPecasController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public EncomendasPecasController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: EncomendasPecas

        public EncomendasPecasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "EncomendasPecas";

            var encomendas = await _context.EncomendasPecas
                .Include(e => e.Peca)
                .ThenInclude(p => p.Fornecedor)
                .OrderByDescending(e => e.DataCriacao)
                .ToListAsync();

            return View(encomendas);
        }

        // GET: EncomendasPecas/Create
        public IActionResult Create()
        {
            ViewData["IDPeca"] = new SelectList(_context.Pecas.OrderBy(p => p.Descricao), "IDPeca", "Descricao");
            return View(new EncomendaPeca { DataNecessaria = DateTime.Today.AddDays(1) });
        }

        // GET: EncomendasPecas/Details/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDEncomendaPeca,IDPeca,Quantidade,DataNecessaria,Observacoes")] EncomendaPeca encomenda)
        {
            if (encomenda.DataNecessaria.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(encomenda.DataNecessaria), "A data necessária não pode ser anterior à data atual.");
            }

            if (ModelState.IsValid)
            {
                encomenda.DataCriacao = DateTime.Now;
                encomenda.Estado = EstadoEncomendaPeca.Pendente;

                _context.Add(encomenda);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Encomenda registada com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDPeca"] = new SelectList(_context.Pecas.OrderBy(p => p.Descricao), "IDPeca", "Descricao", encomenda.IDPeca);
            return View(encomenda);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.EncomendasPecas
                .Include(e => e.Peca)
                .ThenInclude(p => p.Fornecedor)
                .FirstOrDefaultAsync(e => e.IDEncomendaPeca == id);

            if (encomenda == null)
            {
                return NotFound();
            }

            return View(encomenda);
        }

        // POST: EncomendasPecas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDEncomendaPeca,IDPeca,Quantidade,DataNecessaria,Observacoes")] EncomendaPeca encomenda)
        {
            if (encomenda.DataNecessaria.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(encomenda.DataNecessaria), "A data necessária não pode ser anterior à data atual.");
            }

            if (ModelState.IsValid)
            {
                encomenda.DataCriacao = DateTime.Now;
                encomenda.Estado = EstadoEncomendaPeca.Pendente;

                _context.Add(encomenda);
                await _context.SaveChangesAsync();

                await EnviarEmailFabricanteAsync(encomenda);

                TempData["Success"] = "Encomenda de peças registada e enviada ao fabricante.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDPeca"] = new SelectList(_context.Pecas.OrderBy(p => p.Descricao), "IDPeca", "Descricao", encomenda.IDPeca);
            return View(encomenda);
        }

        // GET: EncomendasPecas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.EncomendasPecas.FindAsync(id);
            if (encomenda == null)
            {
                return NotFound();
            }

            ViewData["IDPeca"] = new SelectList(_context.Pecas.OrderBy(p => p.Descricao), "IDPeca", "Descricao", encomenda.IDPeca);
            ViewData["EstadosEncomenda"] = Enum.GetValues(typeof(EstadoEncomendaPeca)).Cast<EstadoEncomendaPeca>().ToList();
            return View(encomenda);
        }

        // POST: EncomendasPecas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDEncomendaPeca,IDPeca,Quantidade,DataNecessaria,Observacoes,Estado,DataCriacao")] EncomendaPeca encomenda)
        {
            if (id != encomenda.IDEncomendaPeca)
            {
                return NotFound();
            }

            if (encomenda.DataNecessaria.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(encomenda.DataNecessaria), "A data necessária não pode ser anterior à data atual.");
            }

            if (ModelState.IsValid)
            {
                _context.Update(encomenda);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Encomenda atualizada com sucesso.";
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDPeca"] = new SelectList(_context.Pecas.OrderBy(p => p.Descricao), "IDPeca", "Descricao", encomenda.IDPeca);
            ViewData["EstadosEncomenda"] = Enum.GetValues(typeof(EstadoEncomendaPeca)).Cast<EstadoEncomendaPeca>().ToList();
            return View(encomenda);
        }

        private async Task EnviarEmailFabricanteAsync(EncomendaPeca encomenda)
        {
            var host = _configuration["Smtp2Go:Host"]!;
            var port = int.Parse(_configuration["Smtp2Go:Port"] ?? "2525");
            var enableSsl = bool.Parse(_configuration["Smtp2Go:EnableSsl"] ?? "true");
            var smtpUser = _configuration["Smtp2Go:Username"]!;
            var smtpPass = _configuration["Smtp2Go:Password"]!;
            var fromEmail = _configuration["Smtp2Go:FromEmail"]!;
            var fromName = _configuration["Smtp2Go:FromName"] ?? "A-MoVeR";
            var fabricanteEmail = _configuration["Fabricante:Email"];

            if (string.IsNullOrWhiteSpace(fabricanteEmail))
            {
                return;
            }

            var peca = await _context.Pecas
                .Include(p => p.Fornecedor)
                .FirstOrDefaultAsync(p => p.IDPeca == encomenda.IDPeca);

            var subject = $"Nova encomenda de peças - {peca?.Descricao}";
            var body = $@"Foi criada uma nova encomenda de peças pela concessionária.

Peça: {peca?.Descricao}
Part Number: {peca?.PartNumber}
Fornecedor: {peca?.Fornecedor?.Nome}
Quantidade: {encomenda.Quantidade}
Data Necessária: {encomenda.DataNecessaria:dd/MM/yyyy}
Observações: {encomenda.Observacoes ?? "(sem observações)"}

Data de criação: {encomenda.DataCriacao:dd/MM/yyyy HH:mm}";

            await EmailHelper.SendAsync(
                host, port, enableSsl,
                smtpUser, smtpPass,
                fromEmail, fromName,
                fabricanteEmail, subject, body
            );
        }
    }
}
