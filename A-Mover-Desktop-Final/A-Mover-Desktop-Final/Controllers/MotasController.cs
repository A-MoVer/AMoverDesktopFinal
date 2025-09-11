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
            var motas = await _context.Motas
                .Include(m => m.ModeloMota)
                .Include(m => m.OrdemProducao)
                .OrderByDescending(m => m.DataRegisto)  // Ordenar pela data de registro (mais recentes primeiro)
                .ToListAsync();
            
            return View(motas);
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

            var mota = await _context.Motas
                .Include(m => m.ModeloMota)
                .Include(m => m.MotasPecasSN)
                    .ThenInclude(p => p.Pecas)
                .FirstOrDefaultAsync(m => m.IDMota == id);
                
            if (mota == null)
            {
                return NotFound();
            }
            
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome", mota.IDModelo);
            ViewData["IDOrdemProducao"] = new SelectList(_context.OrdemProducao, "IDOrdemProducao", "IDOrdemProducao", mota.IDOrdemProducao);
            
            // Preparar lista de peças disponíveis para adicionar
            //ViewBag.PecasDisponiveis = await _context.Pecas
             //   .Where(p => p.ControlarSN)
             //   .Select(p => new { 
              //      Value = p.IDPeca, 
                //PartNumber = p.PartNumber, 
                 //   Descricao = p.Descricao 
               // })
              //  .ToListAsync();
            
            return View(mota);
        }

        // POST: Motas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id, 
            Mota mota, 
            List<int> PecasID, 
            List<int> PecasIDPeca, 
            List<string> PecasNumeroSerie,
            List<int> NovasPecasIDPeca, 
            List<string> NovasPecasNumeroSerie,
            List<int> PecasRemovidas)
        {
            if (id != mota.IDMota)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Atualizar dados da mota
                    _context.Update(mota);
                    
                    // Processar peças existentes (atualizar número de série)
                    if (PecasID != null)
                    {
                        for (int i = 0; i < PecasID.Count; i++)
                        {
                            var pecaId = PecasID[i];
                            var numeroSerie = PecasNumeroSerie[i];
                            
                            var pecaExistente = await _context.MotasPecasSN.FindAsync(pecaId);
                            if (pecaExistente != null)
                            {
                                pecaExistente.NumeroSerie = numeroSerie;
                                _context.Update(pecaExistente);
                            }
                        }
                    }
                    
                    // Adicionar novas peças
                    if (NovasPecasIDPeca != null)
                    {
                        for (int i = 0; i < NovasPecasIDPeca.Count; i++)
                        {
                            var idPeca = NovasPecasIDPeca[i];
                            var numeroSerie = NovasPecasNumeroSerie[i];
                            
                            var novaPeca = new MotasPecasSN
                            {
                                IDMota = mota.IDMota,
                                IDPeca = idPeca,
                                NumeroSerie = numeroSerie
                            };
                            
                            _context.MotasPecasSN.Add(novaPeca);
                        }
                    }
                    
                    // Remover peças
                    if (PecasRemovidas != null)
                    {
                        foreach (var pecaId in PecasRemovidas)
                        {
                            var pecaRemover = await _context.MotasPecasSN.FindAsync(pecaId);
                            if (pecaRemover != null)
                            {
                                _context.MotasPecasSN.Remove(pecaRemover);
                            }
                        }
                    }
                    
                    await _context.SaveChangesAsync();
                    
                    TempData["Sucesso"] = "Mota atualizada com sucesso!";
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
            
            ViewData["IDModelo"] = new SelectList(_context.ModelosMota, "IDModelo", "Nome", mota.IDModelo);
            ViewData["IDOrdemProducao"] = new SelectList(_context.OrdemProducao, "IDOrdemProducao", "IDOrdemProducao", mota.IDOrdemProducao);
            
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

        [HttpGet]
        public IActionResult GetListaIdentificacoes()
        {
            var lista = _context.Motas
                .Select(m => new
                {
                    id = m.IDMota,
                    numeroIdentificacao = m.NumeroIdentificacao
                }).ToList();

            return Json(lista);
        }
    }
}
