using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .ThenInclude(m => m.ModeloMota)
                .ToListAsync();
            return View(servicos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico
                .Include(s => s.Mota)
                    .ThenInclude(m => m.ModeloMota)
                .Include(s => s.PecasAlteradas)
                    .ThenInclude(p => p.MotasPecasSN)
                        .ThenInclude(mp => mp.Pecas)
                .FirstOrDefaultAsync(s => s.IDServico == id);

            if (servico == null) return NotFound();

            return View(servico);
        }
        public async Task<IActionResult> CalendarioIntervencoes()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .ToListAsync();

            return View(servicos);
        }
        [HttpGet]
        public async Task<IActionResult> GetIntervencoesAgendadas()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .Where(s => s.Estado != EstadoServico.Concluido)
                .ToListAsync();

            var eventos = servicos.Select(s => new
            {
                title = $"{s.Mota?.NumeroIdentificacao} - {s.Tipo}",
                start = s.DataServico.ToString("s"), // formato ISO 8601
                extendedProps = new
                {
                    vin = s.Mota?.NumeroIdentificacao,
                    tipoManutencao = s.Tipo.ToString(),
                    description = string.IsNullOrEmpty(s.Descricao) ? "Sem descrição" : s.Descricao
                }
            });

            return Json(eventos);
        }

        // GET: Servicos/AgendarIntervencao
        public IActionResult AgendarIntervencao()
        {
            ViewData["Motas"] = _context.Motas
                .Select(m => new SelectListItem
                {
                    Value = m.IDMota.ToString(),
                    Text = m.NumeroIdentificacao
                }).ToList();

            ViewData["TiposServico"] = Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().ToList();

            var servico = new Servico
            {
                DataServico = DateTime.Now.AddDays(1)
            };

            return View(servico);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgendarIntervencao(Servico servico)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao", servico.IDMota);
                return View(servico);
            }

            servico.Estado = EstadoServico.Agendado;
            servico.DataConclusao = DateTime.MinValue;

            _context.Servico.Add(servico);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Intervenção agendada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RegistarIntervencao()
        {
            ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistarIntervencao(Servico servico)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao", servico.IDMota);
                return View(servico);
            }

            servico.Estado = EstadoServico.EmCurso;
            servico.DataConclusao = DateTime.MinValue;
            servico.DataServico = DateTime.Now;

            _context.Servico.Add(servico);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Intervenção iniciada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico.FindAsync(id);
            if (servico == null) return NotFound();

            ViewData["IDMota"] = new SelectList(_context.Motas, "IDMota", "NumeroIdentificacao", servico.IDMota);
            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDServico,IDMota,Tipo,Descricao,Estado,DataServico,DataConclusao")] Servico servico)
        {
            if (id != servico.IDServico) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.IDServico)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDMota"] = new SelectList(_context.Motas, "IDMota", "NumeroIdentificacao", servico.IDMota);
            return View(servico);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico
                .Include(s => s.Mota)
                .FirstOrDefaultAsync(s => s.IDServico == id);

            if (servico == null) return NotFound();

            return View(servico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servico.FindAsync(id);
            if (servico != null)
            {
                _context.Servico.Remove(servico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServicoExists(int id)
        {
            return _context.Servico.Any(e => e.IDServico == id);
        }

        [HttpGet]
        public async Task<IActionResult> ObterMotasPorIdentificacao(string termo)
        {
            var motas = await _context.Motas
                .Where(m => m.NumeroIdentificacao.Contains(termo))
                .Select(m => new
                {
                    id = m.IDMota,
                    texto = m.NumeroIdentificacao
                })
                .ToListAsync();

            return Json(motas);
        }

    }
}