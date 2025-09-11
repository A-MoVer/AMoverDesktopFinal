using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class EncomendasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EncomendasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Encomendas
        public async Task<IActionResult> Index()
        {
            var encomendas = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.ModeloMota)
                .OrderByDescending(e => e.DateCriacao)  // Ordenar por data de criação (mais recentes primeiro)
                .ToListAsync();
            
            return View(encomendas);
        }

        // GET: Encomendas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Buscar a encomenda (sem incluir ordens de produção)
            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.ModeloMota)
                .FirstOrDefaultAsync(m => m.IDEncomenda == id);

            if (encomenda == null)
            {
                return NotFound();
            }

            // Buscar as ordens de produção separadamente
            var ordensProducao = await _context.OrdemProducao
                .Include(op => op.Mota)
                .Where(op => op.IDEncomenda == id)
                .ToListAsync();

            // Adicionar mensagem para depuração
            TempData["Debug"] = $"Buscando encomenda com ID {id} | Ordens encontradas: {ordensProducao.Count}";

            // Passar as ordens para a view usando ViewBag
            ViewBag.OrdensProducao = ordensProducao;

            return View(encomenda);
        }

        // GET: Encomendas/Create
        public IActionResult Create()
        {
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Nome");
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome");
            return View();
        }

        // POST: Encomendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDEncomenda,IDModelo,IDCliente,Quantidade")] Encomenda encomenda)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Definir valores padrão
                    encomenda.Estado = EstadoEncomenda.Pendente;
                    encomenda.DateCriacao = DateTime.Now;
                    encomenda.DataEntrega = null; // Garantir que a data de entrega seja nula
                    encomenda.OrdemProducao = new List<OrdemProducao>(); // Inicializar a coleção

                    _context.Add(encomenda);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    // Adicionar mensagem de erro específica para depuração
                    ModelState.AddModelError("", $"Erro ao criar encomenda: {ex.Message}");
                }
            }

            // Recarregar as SelectLists em caso de erro
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Nome", encomenda.IDCliente);
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome", encomenda.IDModelo);
            return View(encomenda);
        }

        // GET: Encomendas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda == null)
            {
                return NotFound();
            }
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Nome", encomenda.IDCliente);
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "CodigoProduto", encomenda.IDModelo);
            return View(encomenda);
        }

        // POST: Encomendas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDEncomenda,IDModelo,IDCliente,Quantidade,Estado,DateCriacao,DataEntrega")] Encomenda encomenda)
        {
            if (id != encomenda.IDEncomenda)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(encomenda);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EncomendaExists(encomenda.IDEncomenda))
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
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Nome", encomenda.IDCliente);
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "CodigoProduto", encomenda.IDModelo);
            return View(encomenda);
        }

        // GET: Encomendas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.ModeloMota)
                .FirstOrDefaultAsync(m => m.IDEncomenda == id);
            if (encomenda == null)
            {
                return NotFound();
            }

            return View(encomenda);
        }

        // POST: Encomendas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var encomenda = await _context.Encomendas.FindAsync(id);
            if (encomenda != null)
            {
                _context.Encomendas.Remove(encomenda);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EncomendaExists(int id)
        {
            return _context.Encomendas.Any(e => e.IDEncomenda == id);
        }

        // GET: Clientes/BuscarPorNome
        [HttpGet]
        public async Task<IActionResult> BuscarPorNome(string termo)
        {
            if (string.IsNullOrEmpty(termo) || termo.Length < 2)
                return Json(new List<object>());

            var clientes = await _context.Clientes
                .Where(c => c.Nome.Contains(termo))
                .Take(10)
                .Select(c => new
                {
                    id = c.IDCliente,
                    nome = c.Nome,
                })
                .ToListAsync();

            return Json(clientes);
        }
    }
}
