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
    public class MaterialRecebidoesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MaterialRecebidoesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MaterialRecebidoes
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MateriaisRecebidos.Include(m => m.Fornecedor).Include(m => m.Peca);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: MaterialRecebidoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRecebido = await _context.MateriaisRecebidos
                .Include(m => m.Fornecedor)
                .Include(m => m.Peca)
                .FirstOrDefaultAsync(m => m.IDMaterialRecebido == id);
            if (materialRecebido == null)
            {
                return NotFound();
            }

            return View(materialRecebido);
        }

        // GET: MaterialRecebidoes/Create
        public IActionResult Create()
        {
            ViewBag.Fornecedores = new SelectList(
                _context.Fornecedores.OrderBy(f => f.Nome), "IDFornecedor", "Nome"
            );
            ViewBag.Pecas = new SelectList(
                _context.Pecas.OrderBy(p => p.PartNumber), "IDPeca", "PartNumber"
            );
            return View();
        }

        // POST: MaterialRecebidoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDMaterialRecebido,PecaId,FornecedorId,DataRececao,Quantidade,Lote,Documento,Observacoes")] MaterialRecebido materialRecebido)
        {
            if (ModelState.IsValid)
            {
                _context.Add(materialRecebido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Fornecedores = new SelectList(
                _context.Fornecedores.OrderBy(f => f.Nome), "IDFornecedor", "Nome", materialRecebido.FornecedorId
            );
            ViewBag.Pecas = new SelectList(
                _context.Pecas.OrderBy(p => p.PartNumber), "IDPeca", "PartNumber", materialRecebido.PecaId
            );
            return View(materialRecebido);
        }


        // GET: MaterialRecebidoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRecebido = await _context.MateriaisRecebidos.FindAsync(id);
            if (materialRecebido == null)
            {
                return NotFound();
            }
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "IDFornecedor", "Nome", materialRecebido.FornecedorId);
            ViewData["PecaId"] = new SelectList(_context.Pecas, "IDPeca", "Descricao", materialRecebido.PecaId);
            return View(materialRecebido);
        }

        // POST: MaterialRecebidoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDMaterialRecebido,PecaId,FornecedorId,DataRececao,Quantidade,Lote,Documento,Observacoes")] MaterialRecebido materialRecebido)
        {
            if (id != materialRecebido.IDMaterialRecebido)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materialRecebido);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialRecebidoExists(materialRecebido.IDMaterialRecebido))
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
            ViewData["FornecedorId"] = new SelectList(_context.Fornecedores, "IDFornecedor", "Nome", materialRecebido.FornecedorId);
            ViewData["PecaId"] = new SelectList(_context.Pecas, "IDPeca", "Descricao", materialRecebido.PecaId);
            return View(materialRecebido);
        }

        // GET: MaterialRecebidoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materialRecebido = await _context.MateriaisRecebidos
                .Include(m => m.Fornecedor)
                .Include(m => m.Peca)
                .FirstOrDefaultAsync(m => m.IDMaterialRecebido == id);
            if (materialRecebido == null)
            {
                return NotFound();
            }

            return View(materialRecebido);
        }

        // POST: MaterialRecebidoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var materialRecebido = await _context.MateriaisRecebidos.FindAsync(id);
            if (materialRecebido != null)
            {
                _context.MateriaisRecebidos.Remove(materialRecebido);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialRecebidoExists(int id)
        {
            return _context.MateriaisRecebidos.Any(e => e.IDMaterialRecebido == id);
        }
    }
}
