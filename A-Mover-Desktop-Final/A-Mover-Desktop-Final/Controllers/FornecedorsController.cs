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
    public class FornecedorsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FornecedorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fornecedors
        public async Task<IActionResult> Index()
        {
            ViewData["ActiveMenu"] = "GestaoFornecedores";

            var fornecedores = await _context.Fornecedores
                .AsNoTracking()
                .Include(f => f.Pecas)
                .ToListAsync();


            return View(fornecedores);
        }

        // GET: Fornecedors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                 .Include(f => f.Pecas)
                .FirstOrDefaultAsync(m => m.IDFornecedor == id);
                
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // GET: Fornecedors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fornecedors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDFornecedor,Nome,Email")] Fornecedor fornecedor)
        {
            // Validar se já existe fornecedor com o mesmo nome
            if (await _context.Fornecedores.AnyAsync(f => f.Nome.ToLower() == fornecedor.Nome.ToLower()))
            {
                ModelState.AddModelError("Nome", "Já existe um fornecedor com este nome.");
            }

            // Validar se já existe fornecedor com o mesmo email (se email foi fornecido)
            if (!string.IsNullOrWhiteSpace(fornecedor.Email) && 
                await _context.Fornecedores.AnyAsync(f => f.Email.ToLower() == fornecedor.Email.ToLower()))
            {
                ModelState.AddModelError("Email", "Já existe um fornecedor com este email.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(fornecedor);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fornecedor criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return View(fornecedor);
        }

        // GET: Fornecedors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor == null)
            {
                return NotFound();
            }
            return View(fornecedor);
        }

        // POST: Fornecedors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDFornecedor,Nome,Email")] Fornecedor fornecedor)
        {
            if (id != fornecedor.IDFornecedor)
            {
                return NotFound();
            }

            // Validar se já existe outro fornecedor com o mesmo nome
            if (await _context.Fornecedores.AnyAsync(f => f.Nome.ToLower() == fornecedor.Nome.ToLower() && f.IDFornecedor != id))
            {
                ModelState.AddModelError("Nome", "Já existe outro fornecedor com este nome.");
            }

            // Validar se já existe outro fornecedor com o mesmo email (se email foi fornecido)
            if (!string.IsNullOrWhiteSpace(fornecedor.Email) && 
                await _context.Fornecedores.AnyAsync(f => f.Email.ToLower() == fornecedor.Email.ToLower() && f.IDFornecedor != id))
            {
                ModelState.AddModelError("Email", "Já existe outro fornecedor com este email.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fornecedor);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Fornecedor atualizado com sucesso!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FornecedorExists(fornecedor.IDFornecedor))
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
            return View(fornecedor);
        }

        // GET: Fornecedors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fornecedor = await _context.Fornecedores
                 .Include(f => f.Pecas)
                .FirstOrDefaultAsync(m => m.IDFornecedor == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            return View(fornecedor);
        }

        // POST: Fornecedors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fornecedor = await _context.Fornecedores.FindAsync(id);
            if (fornecedor != null)
            {
                _context.Fornecedores.Remove(fornecedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FornecedorExists(int id)
        {
            return _context.Fornecedores.Any(e => e.IDFornecedor == id);
        }
    }
}
