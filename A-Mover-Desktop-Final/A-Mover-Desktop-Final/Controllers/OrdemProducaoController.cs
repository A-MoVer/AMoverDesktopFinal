using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class OrdemProducaoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdemProducaoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordens = await _context.OrdemProducao
                .Include(op => op.Encomenda)
                    .ThenInclude(e => e.Cliente)
                .Include(op => op.Encomenda.ModeloMota)
                .ToListAsync();

            return View(ordens);
        }

        public async Task<IActionResult> Details(int? id)
{
    if (id == null) return NotFound();

    var ordemProducao = await _context.OrdemProducao
        .Include(op => op.Encomenda)
            .ThenInclude(e => e.Cliente)
        .Include(op => op.Encomenda.ModeloMota)
        .Include(op => op.ChecklistMontagem)
            .ThenInclude(cm => cm.Checklist)
        .Include(op => op.ChecklistControlo)
            .ThenInclude(cc => cc.Checklist)
        .Include(op => op.ChecklistEmbalagem)
            .ThenInclude(ce => ce.Checklist)
        .FirstOrDefaultAsync(op => op.IDOrdemProducao == id);

    if (ordemProducao == null) return NotFound();

    // Return the FichaOP view instead of Details view
    return View("FichaOP", ordemProducao);
}

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IDOrdemProducao,IDEncomenda,NumeroOrdem,Estado,PaisDestino,DataCriacao,DataConclusao")] OrdemProducao ordemProducao)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ordemProducao);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ordemProducao);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var ordemProducao = await _context.OrdemProducao.FindAsync(id);
            if (ordemProducao == null) return NotFound();

            return View(ordemProducao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDOrdemProducao,IDEncomenda,NumeroOrdem,Estado,PaisDestino,DataCriacao,DataConclusao")] OrdemProducao ordemProducao)
        {
            if (id != ordemProducao.IDOrdemProducao) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ordemProducao);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdemProducaoExists(ordemProducao.IDOrdemProducao)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(ordemProducao);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var ordemProducao = await _context.OrdemProducao.FindAsync(id);
            if (ordemProducao == null) return NotFound();

            return View(ordemProducao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ordemProducao = await _context.OrdemProducao.FindAsync(id);
            if (ordemProducao != null)
            {
                _context.OrdemProducao.Remove(ordemProducao);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CriarPorEncomenda(int id)
        {
            var encomenda = await _context.Encomendas
                .Include(e => e.Cliente)
                .Include(e => e.ModeloMota)
                .FirstOrDefaultAsync(e => e.IDEncomenda == id);

            if (encomenda == null || encomenda.Estado.ToString() != "Pendente") return NotFound();

            var ordens = new List<OrdemProducao>();
            for (int i = 1; i <= encomenda.Quantidade; i++)
            {
                var numero = $"OP-{encomenda.IDEncomenda}-{i:D3}";
                ordens.Add(new OrdemProducao
                {
                    IDEncomenda = encomenda.IDEncomenda,
                    NumeroOrdem = numero,
                    Estado = EstadoOrdemProducao.Pendente,
                    PaisDestino = encomenda.Cliente.Cidade ?? "Desconhecido",
                    DataCriacao = DateTime.Now
                });
            }

            _context.OrdemProducao.AddRange(ordens);

            encomenda.Estado = EstadoEncomenda.EmProducao;
            _context.Encomendas.Update(encomenda);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Iniciar(int id)
        {
            var ordem = await _context.OrdemProducao
                .Include(o => o.Encomenda)
                    .ThenInclude(e => e.Cliente)
                .Include(o => o.Encomenda)
                    .ThenInclude(e => e.ModeloMota)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);

            if (ordem == null || ordem.Estado != EstadoOrdemProducao.Pendente)
                return NotFound();

            ordem.Estado = EstadoOrdemProducao.EmProducao;

            var checklistsMontagem = await _context.Checklist.Where(c => c.Tipo == TipoChecklist.Montagem).ToListAsync();
            var checklistsControlo = await _context.Checklist.Where(c => c.Tipo == TipoChecklist.Controlo).ToListAsync();
            var checklistsEmbalagem = await _context.Checklist.Where(c => c.Tipo == TipoChecklist.Embalagem).ToListAsync();

            foreach (var c in checklistsMontagem)
                _context.ChecklistMontagem.Add(new ChecklistMontagem { IDChecklist = c.IDChecklist, IDOrdemProducao = ordem.IDOrdemProducao, Verificado = VerificadoChecklistMontagem.Nao });

            foreach (var c in checklistsControlo)
                _context.ChecklistControlo.Add(new ChecklistControlo { IDChecklist = c.IDChecklist, IDOrdemProducao = ordem.IDOrdemProducao, ControloFinal = ControloFinalChecklistControlo.Nao });

            foreach (var c in checklistsEmbalagem)
                _context.ChecklistEmbalagem.Add(new ChecklistEmbalagem { IDChecklist = c.IDChecklist, IDOrdemProducao = ordem.IDOrdemProducao, Incluido = IncluidoChecklistEmbalagem.Nao });

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FichaOP), new { id = ordem.IDOrdemProducao });
        }

        public async Task<IActionResult> Continuar(int id)
        {
            return await FichaOP(id);
        }

        public async Task<IActionResult> FichaOP(int id)
        {
            var ordem = await _context.OrdemProducao
                .Include(o => o.Encomenda).ThenInclude(e => e.Cliente)
                .Include(o => o.Encomenda.ModeloMota)
                .Include(o => o.ChecklistMontagem).ThenInclude(c => c.Checklist)
                .Include(o => o.ChecklistControlo).ThenInclude(c => c.Checklist)
                .Include(o => o.ChecklistEmbalagem).ThenInclude(c => c.Checklist)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);

            if (ordem == null) return NotFound();

            return View("FichaOP", ordem);
        }
        [HttpPost]
        public async Task<IActionResult> GuardarChecklists(int IDOrdemProducao,List<ChecklistMontagem> ChecklistMontagem,List<ChecklistControlo> ChecklistControlo,List<ChecklistEmbalagem> ChecklistEmbalagem)
        {
            var ordem = await _context.OrdemProducao.FindAsync(IDOrdemProducao);
            if (ordem == null)
                return NotFound();
                
            if (ordem.Estado == EstadoOrdemProducao.Concluida)
            {
                TempData["ErrorMessage"] = "Não é possível alterar uma ordem já concluída.";
                return RedirectToAction(nameof(FichaOP), new { id = IDOrdemProducao });
            }
            foreach (var item in ChecklistMontagem)
            {
                var entidade = await _context.ChecklistMontagem.FindAsync(item.IDChecklistMontagem);
                if (entidade != null) entidade.Verificado = item.Verificado;
            }

            foreach (var item in ChecklistControlo)
            {
                var entidade = await _context.ChecklistControlo.FindAsync(item.IDChecklistControlo);
                if (entidade != null) entidade.ControloFinal = item.ControloFinal;
            }

            foreach (var item in ChecklistEmbalagem)
            {
                var entidade = await _context.ChecklistEmbalagem.FindAsync(item.IDChecklistEmbalagem);
                if (entidade != null) entidade.Incluido = item.Incluido;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FichaOP), new { id = IDOrdemProducao });
        }

        // Add to OrdemProducaoController.cs
public async Task<IActionResult> Finalizar(int id)
{
    var ordem = await _context.OrdemProducao.FindAsync(id);
    if (ordem == null || ordem.Estado != EstadoOrdemProducao.EmProducao)
        return NotFound();

    ordem.Estado = EstadoOrdemProducao.Concluida;
    ordem.DataConclusao = DateTime.Now;
    
    _context.Update(ordem);
    await _context.SaveChangesAsync();
    
    // Check if all orders for this encomenda are completed
    var allOrders = await _context.OrdemProducao
        .Where(o => o.IDEncomenda == ordem.IDEncomenda)
        .ToListAsync();
        
    if (allOrders.All(o => o.Estado == EstadoOrdemProducao.Concluida))
    {
        var encomenda = await _context.Encomendas.FindAsync(ordem.IDEncomenda);
        if (encomenda != null)
        {
            encomenda.Estado = EstadoEncomenda.Concluida;
            _context.Update(encomenda);
            await _context.SaveChangesAsync();
        }
    }
    
    TempData["SuccessMessage"] = "Ordem de produção finalizada com sucesso!";
    return RedirectToAction(nameof(Index));
}
        private bool OrdemProducaoExists(int id)
        {
            return _context.OrdemProducao.Any(e => e.IDOrdemProducao == id);
        }
    }
}
