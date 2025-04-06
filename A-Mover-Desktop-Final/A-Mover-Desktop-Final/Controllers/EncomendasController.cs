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
            var applicationDbContext = _context.Encomendas.Include(e => e.Cliente).Include(e => e.ModeloMota);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Encomendas/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public async Task<IActionResult> Create([Bind("IDEncomenda,IDModelo,IDCliente,Quantidade,Estado,DateCriacao,DataEntrega")] Encomenda encomenda)
        {
            if (ModelState.IsValid)
            {
                _context.Add(encomenda);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Cidade", encomenda.IDCliente);
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "CodigoProduto", encomenda.IDModelo);
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
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Cidade", encomenda.IDCliente);
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
            ViewData["IDCliente"] = new SelectList(_context.Clientes, "IDCliente", "Cidade", encomenda.IDCliente);
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
    }
}
