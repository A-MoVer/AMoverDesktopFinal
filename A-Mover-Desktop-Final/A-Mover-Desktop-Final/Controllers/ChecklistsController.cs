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
    public class ChecklistsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChecklistsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Checklists
        public async Task<IActionResult> Index()
        {
            var checklists = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                    .ThenInclude(cm => cm.ModeloMota)
                .ToListAsync();
            
            return View(checklists);
        }

        // GET: Checklists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                    .ThenInclude(cm => cm.ModeloMota)
                .FirstOrDefaultAsync(m => m.IDChecklist == id);
                
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // GET: Checklists/Create
        public IActionResult Create(int? modeloId)
        {
            // Preencher ViewBag com a lista de modelos
            ViewData["ModeloMotas"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome");
            
            var checklist = new Checklist();
            
            // Se foi passado um ID de modelo, vamos pré-selecioná-lo
            if (modeloId.HasValue)
            {
                // Opcionalmente, busque o nome do modelo para exibir na view
                var nomeModelo = _context.ModelosMota
                    .Where(m => m.IDModelo == modeloId.Value)
                    .Select(m => m.Nome)
                    .FirstOrDefault();
                    
                ViewData["NomeModeloSelecionado"] = nomeModelo;
                ViewData["SelectedModelId"] = modeloId.Value;
            }
            
            return View(checklist);
        }

        // POST: Checklists/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDChecklist,Nome,Descricao,Tipo")] Checklist checklist, int[] selectedModelos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(checklist);
                await _context.SaveChangesAsync();
                
                // Add associated models
                if (selectedModelos != null && selectedModelos.Length > 0)
                {
                    foreach (var modeloId in selectedModelos)
                    {
                        _context.Add(new ChecklistModelo
                        {
                            IDChecklist = checklist.IDChecklist,
                            IDModelo = modeloId
                        });
                    }
                    await _context.SaveChangesAsync();
                }
                
                return RedirectToAction(nameof(Index));
            }
            
            ViewData["ModeloMotas"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome");
            return View(checklist);
        }

        // GET: Checklists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                .FirstOrDefaultAsync(m => m.IDChecklist == id);
                
            if (checklist == null)
            {
                return NotFound();
            }
            
            // Get IDs of models already associated with this checklist
            var selectedModeloIds = checklist.ChecklistModelos.Select(cm => cm.IDModelo).ToList();
            
            ViewData["ModeloMotas"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome");
            ViewData["SelectedModeloIds"] = selectedModeloIds;
            
            return View(checklist);
        }

        // POST: Checklists/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDChecklist,Nome,Descricao,Tipo")] Checklist checklist, int[] selectedModelos)
        {
            if (id != checklist.IDChecklist)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update the checklist entity itself
                    _context.Update(checklist);
                    
                    // Get current model associations
                    var existingAssociations = await _context.Set<ChecklistModelo>()
                        .Where(cm => cm.IDChecklist == id)
                        .ToListAsync();
                    
                    // Remove all existing associations
                    _context.RemoveRange(existingAssociations);
                    
                    // Add new associations based on selected models
                    if (selectedModelos != null)
                    {
                        foreach (var modeloId in selectedModelos)
                        {
                            _context.Add(new ChecklistModelo 
                            { 
                                IDChecklist = checklist.IDChecklist, 
                                IDModelo = modeloId 
                            });
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChecklistExists(checklist.IDChecklist))
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
            
            ViewData["ModeloMotas"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome");
            return View(checklist);
        }

        // GET: Checklists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var checklist = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                    .ThenInclude(cm => cm.ModeloMota)
                .FirstOrDefaultAsync(m => m.IDChecklist == id);
                
            if (checklist == null)
            {
                return NotFound();
            }

            return View(checklist);
        }

        // POST: Checklists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // First, delete all associations
            var associations = await _context.Set<ChecklistModelo>()
                .Where(cm => cm.IDChecklist == id)
                .ToListAsync();
                
            _context.RemoveRange(associations);
            
            // Then, delete the checklist
            var checklist = await _context.Checklist.FindAsync(id);
            if (checklist != null)
            {
                _context.Checklist.Remove(checklist);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChecklistExists(int id)
        {
            return _context.Checklist.Any(e => e.IDChecklist == id);
        }
    }
}
