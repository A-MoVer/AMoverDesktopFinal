using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;

namespace A_Mover_Desktop_Final.Controllers
{
    public class ModeloMotasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ModeloMotasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ModeloMotas
        public async Task<IActionResult> Index()
        {
            return View(await _context.ModelosMota.ToListAsync());
        }

        // GET: ModeloMotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeloMota = await _context.ModelosMota
                .FirstOrDefaultAsync(m => m.IDModelo == id);
            if (modeloMota == null)
            {
                return NotFound();
            }

            return View(modeloMota);
        }

        // GET: ModeloMotas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ModeloMotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDModelo,CodigoProduto,Nome,DataInicioProducao,DataLancamento,DataDescontinuacao,Estado")] ModeloMota modeloMota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(modeloMota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(modeloMota);
        }

        // GET: ModeloMotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeloMota = await _context.ModelosMota.FindAsync(id);
            if (modeloMota == null)
            {
                return NotFound();
            }
            return View(modeloMota);
        }

        // POST: ModeloMotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDModelo,CodigoProduto,Nome,DataInicioProducao,DataLancamento,DataDescontinuacao,Estado")] ModeloMota modeloMota)
        {
            if (id != modeloMota.IDModelo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(modeloMota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ModeloMotaExists(modeloMota.IDModelo))
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
            return View(modeloMota);
        }

        // GET: ModeloMotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var modeloMota = await _context.ModelosMota
                .FirstOrDefaultAsync(m => m.IDModelo == id);
            if (modeloMota == null)
            {
                return NotFound();
            }

            return View(modeloMota);
        }

        // POST: ModeloMotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var modeloMota = await _context.ModelosMota.FindAsync(id);
            if (modeloMota != null)
            {
                _context.ModelosMota.Remove(modeloMota);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ModeloMotaExists(int id)
        {
            return _context.ModelosMota.Any(e => e.IDModelo == id);
        }
    }
}
