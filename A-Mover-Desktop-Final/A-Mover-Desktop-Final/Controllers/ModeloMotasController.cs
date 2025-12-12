using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            ViewData["ActiveMenu"] = "GestaoModelos";
            return View(await _context.ModelosMota.ToListAsync());
        }

        // GET: ModeloMotas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["ActiveMenu"] = "GestaoModelos";

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

            // Get associated parts with their details
            var pecasFixas = await _context.ModeloPecasFixas
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == id)
                .ToListAsync();

            var pecasSN = await _context.ModeloPecasSN
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == id)
                .ToListAsync();

            ViewBag.PecasFixas = pecasFixas;
            ViewBag.PecasSN = pecasSN;

            // Buscar checklists associados a este modelo usando a tabela de junção
            var checklistsAssociados = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                .Where(c => c.ChecklistModelos.Any(cm => cm.IDModelo == id))
                .ToListAsync();
            
            // Buscar checklists não associados (disponíveis para associação)
            var checklistsDisponiveis = await _context.Checklist
                .Include(c => c.ChecklistModelos)
                .Where(c => !c.ChecklistModelos.Any(cm => cm.IDModelo == id))
                .ToListAsync();

            ViewData["ChecklistsAssociados"] = checklistsAssociados;
            ViewData["ChecklistsDisponiveis"] = checklistsDisponiveis;
    
            return View(modeloMota);
        }

        // Método para associar checklist a um modelo
        [HttpPost]
        public async Task<IActionResult> AssociarChecklist(int idModelo, int idChecklist)
        {
            // Verificar se já existe associação
            var associacaoExistente = await _context.Set<ChecklistModelo>()
                .FirstOrDefaultAsync(cm => cm.IDChecklist == idChecklist && cm.IDModelo == idModelo);

            if (associacaoExistente == null)
            {
                // Criar nova associação na tabela de junção
                _context.Add(new ChecklistModelo
                {
                    IDChecklist = idChecklist,
                    IDModelo = idModelo
                });
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Details), new { id = idModelo });
        }

        // Método para remover associação entre checklist e modelo
        [HttpPost]
        public async Task<IActionResult> RemoverAssociacaoChecklist(int idModelo, int idChecklist)
        {
            // Encontrar a associação na tabela de junção
            var associacao = await _context.Set<ChecklistModelo>()
                .FirstOrDefaultAsync(cm => cm.IDChecklist == idChecklist && cm.IDModelo == idModelo);
                
            if (associacao != null)
            {
                // Remover a associação
                _context.Remove(associacao);
                
                // Verificar se o checklist está sendo usado em alguma ordem de produção
                bool usadoEmMontagem = await _context.ChecklistMontagem.AnyAsync(cm => cm.IDChecklist == idChecklist);
                bool usadoEmControlo = await _context.ChecklistControlo.AnyAsync(cc => cc.IDChecklist == idChecklist);
                bool usadoEmEmbalagem = await _context.ChecklistEmbalagem.AnyAsync(ce => ce.IDChecklist == idChecklist);
                
                // Verificar se o checklist está associado a outros modelos
                bool associadoOutrosModelos = await _context.Set<ChecklistModelo>()
                    .AnyAsync(cm => cm.IDChecklist == idChecklist && cm.IDModelo != idModelo);
                
                // Se o checklist não estiver em uso e não estiver associado a outros modelos, pode ser eliminado
                if (!usadoEmMontagem && !usadoEmControlo && !usadoEmEmbalagem && !associadoOutrosModelos)
                {
                    var checklist = await _context.Checklist.FindAsync(idChecklist);
                    if (checklist != null)
                    {
                        _context.Checklist.Remove(checklist);
                        TempData["Success"] = "Checklist eliminado com sucesso.";
                    }
                }
                else
                {
                    TempData["Warning"] = "O checklist não pôde ser eliminado porque está sendo utilizado em ordens de produção ou está associado a outros modelos. A associação foi removida.";
                }
                
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Details), new { id = idModelo });
        }
        
        // GET: ModeloMotas/Create
        public IActionResult Create()
        {
            ViewData["ActiveMenu"] = "GestaoModelos";
            ViewBag.Pecas = _context.Pecas.OrderBy(p => p.PartNumber).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModeloMota modeloMota, string pecasFixasIds, string pecasSNIds)
        {
            ViewData["ActiveMenu"] = "GestaoModelos";

            var pecasFixasList = pecasFixasIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();
            var pecasSNList = pecasSNIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();

            if (ModelState.IsValid)
            {
                _context.Add(modeloMota);
                await _context.SaveChangesAsync();

                foreach (var idPeca in pecasFixasList)
                {
                    _context.ModeloPecasFixas.Add(new ModeloPecasFixas
                    {
                        IDModelo = modeloMota.IDModelo,
                        IDPeca = idPeca
                    });
                }

                foreach (var idPeca in pecasSNList)
                {
                    _context.ModeloPecasSN.Add(new ModeloPecasSN
                    {
                        IDModelo = modeloMota.IDModelo,
                        IDPeca = idPeca
                    });
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = "Modelo criado com sucesso! Configure agora as especificações padrão.";
                return RedirectToAction(nameof(ConfigurarEspecificacoesPadrao), new { id = modeloMota.IDModelo });
            }

            ViewBag.Pecas = await _context.Pecas.OrderBy(p => p.PartNumber).ToListAsync();
            return View(modeloMota);
        }

        // Novo método para configurar especificações padrão
        [HttpGet]
        public async Task<IActionResult> ConfigurarEspecificacoesPadrao(int id)
        {
            var modelo = await _context.ModelosMota
                .Include(m => m.PecasFixas).ThenInclude(p => p.Pecas)
                .Include(m => m.PecasSN).ThenInclude(p => p.Pecas)
                .FirstOrDefaultAsync(m => m.IDModelo == id);
                
            if (modelo == null)
                return NotFound();
                
            return View(modelo);
        }

        // Método para salvar as especificações padrão
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarEspecificacoesPadrao(int id, Dictionary<string, string> especificacoes)
        {
            var modelo = await _context.ModelosMota
                .Include(m => m.PecasFixas)
                .Include(m => m.PecasSN)
                .FirstOrDefaultAsync(m => m.IDModelo == id);
                
            if (modelo == null)
                return NotFound();
            
            // Processar peças fixas
            foreach (var pecaFixa in modelo.PecasFixas)
            {
                string key = $"fixa_{pecaFixa.IDPeca}";
                if (especificacoes.ContainsKey(key))
                {
                    pecaFixa.EspecificacaoPadrao = especificacoes[key];
                }
            }
            
            // Processar peças com número de série
            foreach (var pecaSN in modelo.PecasSN)
            {
                string key = $"sn_{pecaSN.IDPeca}";
                if (especificacoes.ContainsKey(key))
                {
                    pecaSN.EspecificacaoPadrao = especificacoes[key];
                }
            }
            
            await _context.SaveChangesAsync();
            
            TempData["Success"] = "Especificações padrão salvas com sucesso!";
            return RedirectToAction(nameof(Details), new { id });
        }



        // GET: ModeloMotas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["ActiveMenu"] = "GestaoModelos";

            if (id == null)
            {
                return NotFound();
            }

            var modeloMota = await _context.ModelosMota.FindAsync(id);
            if (modeloMota == null)
            {
                return NotFound();
            }

            // Get existing relationships
            var pecasFixas = await _context.ModeloPecasFixas
                .Where(pf => pf.IDModelo == id)
                .Select(pf => pf.IDPeca)
                .ToListAsync() ?? new List<int>();

            var pecasSN = await _context.ModeloPecasSN
                .Where(psn => psn.IDModelo == id)
                .Select(psn => psn.IDPeca)
                .ToListAsync() ?? new List<int>();

            ViewBag.PecasFixasIds = pecasFixas;
            ViewBag.PecasSNIds = pecasSN;
            ViewBag.Pecas = await _context.Pecas.OrderBy(p => p.PartNumber).ToListAsync();

            return View(modeloMota);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
    [Bind("IDModelo,CodigoProduto,Nome,DataInicioProducao,DataLancamento,DataDescontinuacao,Estado")] ModeloMota modeloMota,
    string pecasFixasIds,
    string pecasSNIds)
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

                    var pecasFixas = pecasFixasIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();
                    var pecasSN = pecasSNIds?.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList() ?? new List<int>();

                    var existingPecasFixas = _context.ModeloPecasFixas.Where(mpf => mpf.IDModelo == modeloMota.IDModelo);
                    _context.ModeloPecasFixas.RemoveRange(existingPecasFixas);

                    foreach (var idPeca in pecasFixas)
                    {
                        _context.ModeloPecasFixas.Add(new ModeloPecasFixas { IDModelo = modeloMota.IDModelo, IDPeca = idPeca });
                    }

                    var existingPecasSN = _context.ModeloPecasSN.Where(mps => mps.IDModelo == modeloMota.IDModelo);
                    _context.ModeloPecasSN.RemoveRange(existingPecasSN);

                    foreach (var idPeca in pecasSN)
                    {
                        _context.ModeloPecasSN.Add(new ModeloPecasSN { IDModelo = modeloMota.IDModelo, IDPeca = idPeca });
                    }

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ModelosMota.Any(e => e.IDModelo == modeloMota.IDModelo))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            // Se falhar, recarrega as listas
            var pecasFixasRecarregadas = await _context.ModeloPecasFixas
                .Where(pf => pf.IDModelo == id)
                .Select(pf => pf.IDPeca)
                .ToListAsync();

            var pecasSNRecarregadas = await _context.ModeloPecasSN
                .Where(psn => psn.IDModelo == id)
                .Select(psn => psn.IDPeca)
                .ToListAsync();

            ViewBag.PecasFixasIds = pecasFixasRecarregadas;
            ViewBag.PecasSNIds = pecasSNRecarregadas;
            ViewBag.Pecas = await _context.Pecas.OrderBy(p => p.PartNumber).ToListAsync();

            return View(modeloMota);
        }


        // GET: ModeloMotas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["ActiveMenu"] = "GestaoModelos";

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

            // Get associated parts with their details
            var pecasFixas = await _context.ModeloPecasFixas
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == id)
                .ToListAsync();

            var pecasSN = await _context.ModeloPecasSN
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == id)
                .ToListAsync();

            ViewBag.PecasFixas = pecasFixas;
            ViewBag.PecasSN = pecasSN;

            return View(modeloMota);
        }

        // POST: ModeloMotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Delete related records first
            var pecasFixas = await _context.ModeloPecasFixas
                .Where(pf => pf.IDModelo == id)
                .ToListAsync();
            _context.ModeloPecasFixas.RemoveRange(pecasFixas);

            var pecasSN = await _context.ModeloPecasSN
                .Where(psn => psn.IDModelo == id)
                .ToListAsync();
            _context.ModeloPecasSN.RemoveRange(pecasSN);

            // Then delete the model
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