using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class MotasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MotasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Motas
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Motas.Include(m => m.ModeloMota).Include(m => m.OrdemProducao);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Motas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas
                .Include(m => m.ModeloMota)
                .Include(m => m.OrdemProducao)
                .Include(m => m.MotasPecasSN)
                    .ThenInclude(mp => mp.Pecas)
                .FirstOrDefaultAsync(m => m.IDMota == id);

            if (mota == null)
            {
                return NotFound();
            }

            return View(mota);
        }


        // GET: Motas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas.FindAsync(id);
            if (mota == null)
            {
                return NotFound();
            }
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "CodigoProduto", mota.IDModelo);
            ViewData["IDOrdemProducao"] = new SelectList(_context.OrdemProducao, "IDOrdemProducao", "NumeroOrdem", mota.IDOrdemProducao);
            return View(mota);
        }

        // POST: Motas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDMota,IDModelo,IDOrdemProducao,NumeroIdentificacao,DataRegisto,Cor,Quilometragem,Estado")] Mota mota)
        {
            if (id != mota.IDMota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MotaExists(mota.IDMota))
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
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "CodigoProduto", mota.IDModelo);
            ViewData["IDOrdemProducao"] = new SelectList(_context.OrdemProducao, "IDOrdemProducao", "NumeroOrdem", mota.IDOrdemProducao);
            return View(mota);
        }

        // GET: Motas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mota = await _context.Motas
                .Include(m => m.ModeloMota)
                .Include(m => m.OrdemProducao)
                .FirstOrDefaultAsync(m => m.IDMota == id);
            if (mota == null)
            {
                return NotFound();
            }

            return View(mota);
        }

        // POST: Motas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mota = await _context.Motas.FindAsync(id);
            if (mota != null)
            {
                _context.Motas.Remove(mota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MotaExists(int id)
        {
            return _context.Motas.Any(e => e.IDMota == id);
        }
    }
}
