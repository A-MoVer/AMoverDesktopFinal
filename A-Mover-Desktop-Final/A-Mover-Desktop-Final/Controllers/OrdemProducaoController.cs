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
                .Include(op => op.ChecklistMontagem).ThenInclude(cm => cm.Checklist)
                .Include(op => op.ChecklistControlo).ThenInclude(cc => cc.Checklist)
                .Include(op => op.ChecklistEmbalagem).ThenInclude(ce => ce.Checklist)
                .FirstOrDefaultAsync(op => op.IDOrdemProducao == id);

            if (ordemProducao == null) return NotFound();

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

        public async Task<IActionResult> Continuar(int id) => await FichaOP(id);

        public async Task<IActionResult> FichaOP(int id)
        {
            var ordem = await _context.OrdemProducao
                .Include(o => o.Encomenda).ThenInclude(e => e.Cliente)
                .Include(o => o.Encomenda).ThenInclude(e => e.ModeloMota)
                .Include(o => o.ChecklistMontagem).ThenInclude(c => c.Checklist)
                .Include(o => o.ChecklistControlo).ThenInclude(c => c.Checklist)
                .Include(o => o.ChecklistEmbalagem).ThenInclude(c => c.Checklist)
                .Include(o => o.Mota)
                    .ThenInclude(m => m.MotasPecasSN)
                        .ThenInclude(mp => mp.Pecas)
                .Include(o => o.Mota)
                    .ThenInclude(m => m.ModeloMota)
                        .ThenInclude(mm => mm.PecasSN)
                            .ThenInclude(psn => psn.Pecas)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);


            if (ordem == null) return NotFound();
            return View("FichaOP", ordem);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarChecklists(int IDOrdemProducao, List<ChecklistMontagem> ChecklistMontagem, List<ChecklistControlo> ChecklistControlo, List<ChecklistEmbalagem> ChecklistEmbalagem)
        {
            var ordem = await _context.OrdemProducao.FindAsync(IDOrdemProducao);
            if (ordem == null) return NotFound();

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

        [HttpGet]
        public async Task<IActionResult> AdicionarMota(int id)
        {
            var ordem = await _context.OrdemProducao
                .Include(o => o.Encomenda)
                    .ThenInclude(e => e.ModeloMota)
                        .ThenInclude(m => m.PecasSN)
                            .ThenInclude(psn => psn.Pecas)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);

            if (ordem == null) return NotFound();

            // Se já existe mota, retornar para edição
            if (ordem.Mota != null)
            {
                return PartialView("_MotaFormulario", ordem.Mota);
            }

            // Criar nova mota com todas as peças do modelo
            var novaMota = new Mota
            {
                IDOrdemProducao = ordem.IDOrdemProducao,
                IDModelo = ordem.Encomenda.IDModelo,
                NumeroIdentificacao = $"MOTA-{ordem.NumeroOrdem}-001",
                Quilometragem = 0,
                Estado = EstadoMota.EmProdução,
                DataRegisto = DateTime.Now,
                MotasPecasSN = new List<MotasPecasSN>()
            };

            // Adicionar todas as peças do modelo
            if (ordem.Encomenda.ModeloMota.PecasSN != null)
            {
                foreach (var pecaModelo in ordem.Encomenda.ModeloMota.PecasSN)
                {
                    novaMota.MotasPecasSN.Add(new MotasPecasSN
                    {
                        IDPeca = pecaModelo.IDPeca,  // Use IDPeca from ModeloPecasSN
                        Pecas = pecaModelo.Pecas,    // Reference Pecas directly
                        NumeroSerie = ""             // Inicializa vazio
                    });
                }
            }

            return PartialView("_MotaFormulario", novaMota);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarMota(Mota mota)
        {
            var ordem = await _context.OrdemProducao
                .Include(o => o.Encomenda)
                    .ThenInclude(e => e.ModeloMota)
                        .ThenInclude(m => m.PecasSN)
                .Include(o => o.Mota)
                    .ThenInclude(m => m.MotasPecasSN)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == mota.IDOrdemProducao);

            if (ordem == null) return NotFound();

            if (ordem.Mota == null)
            {
                mota.IDModelo = ordem.Encomenda.IDModelo;
                mota.DataRegisto = DateTime.Now;
                mota.Estado = EstadoMota.EmProdução;

                ordem.Mota = mota;
                _context.Motas.Add(mota);
                await _context.SaveChangesAsync(); // <- Precisamos do ID da mota aqui primeiro

                // Buscar as peças do modelo
                var pecasModelo = await _context.ModeloPecasSN
                    .Include(p => p.Pecas)
                    .Where(p => p.IDModelo == mota.IDModelo)
                    .ToListAsync();

                // Criar as entradas MotasPecasSN
                foreach (var pecaModelo in pecasModelo)
                {
                    var nova = new MotasPecasSN
                    {
                        IDMota = mota.IDMota,
                        IDPeca = pecaModelo.IDPeca,
                        Pecas = pecaModelo.Pecas,
                        NumeroSerie = "" // deixar vazio para preenchimento manual
                    };
                    _context.MotasPecasSN.Add(nova);
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("Peças adicionadas à mota.");
            }
            else
            {
                // Atualizar apenas dados básicos
                ordem.Mota.NumeroIdentificacao = mota.NumeroIdentificacao;
                ordem.Mota.Cor = mota.Cor;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FichaOP), new { id = mota.IDOrdemProducao });
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPecasSN(int IDMota, List<MotasPecasSN> PecasSN)
        {
            var mota = await _context.Motas
                .Include(m => m.MotasPecasSN)
                .FirstOrDefaultAsync(m => m.IDMota == IDMota);

            if (mota == null)
                return NotFound();

            foreach (var peca in PecasSN)
            {
                var existente = mota.MotasPecasSN?.FirstOrDefault(x => x.IDPeca == peca.IDPeca);

                if (existente != null)
                {
                    existente.NumeroSerie = peca.NumeroSerie;
                }
                else
                {
                    _context.MotasPecasSN.Add(new MotasPecasSN
                    {
                        IDMota = IDMota,
                        IDPeca = peca.IDPeca,
                        NumeroSerie = peca.NumeroSerie
                    });
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(FichaOP), new { id = mota.IDOrdemProducao });
        }


        public async Task<IActionResult> Finalizar(int id)
        {
            var ordem = await _context.OrdemProducao.FindAsync(id);
            if (ordem == null || ordem.Estado != EstadoOrdemProducao.EmProducao)
                return NotFound();

            ordem.Estado = EstadoOrdemProducao.Concluida;
            ordem.DataConclusao = DateTime.Now;

            _context.Update(ordem);
            await _context.SaveChangesAsync();

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
