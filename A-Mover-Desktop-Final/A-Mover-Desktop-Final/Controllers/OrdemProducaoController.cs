using System.Text;
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
                    PaisDestino = encomenda.Cliente.Nome ?? "Desconhecido",
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
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);

            if (ordem == null) return NotFound();

            // Se já existe mota, retornar para edição
            if (ordem.Mota != null)
            {
                return RedirectToAction("EditarMota", new { id = ordem.Mota.IDMota });
            }

            // Criar nova mota
            var novaMota = new Mota
            {
                IDOrdemProducao = ordem.IDOrdemProducao,
                IDModelo = ordem.Encomenda.IDModelo,
                NumeroIdentificacao = GerarNumeroIdentificacao(), // ou método que você usa
                Quilometragem = 0,
                Estado = EstadoMota.EmProdução, // ou o estado inicial apropriado
                DataRegisto = DateTime.Now
            };

            // Carregar peças do modelo para exibição
            var pecasFixas = await _context.ModeloPecasFixas
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == novaMota.IDModelo)
                .ToListAsync();

            var pecasSN = await _context.ModeloPecasSN
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == novaMota.IDModelo)
                .ToListAsync();

            ViewBag.PecasFixas = pecasFixas;
            ViewBag.PecasSN = pecasSN;
            ViewBag.PecasInfo = new List<MotasPecasInfo>(); // Lista vazia para novas motas
            ViewBag.Modelo = ordem.Encomenda.ModeloMota; // Importante: passar o modelo para verificar o tipo

            return PartialView("_MotaFormulario", novaMota);
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
            var ordem = await _context.OrdemProducao
                .Include(o => o.Mota)
                    .ThenInclude(m => m.MotasPecasSN)
                .Include(o => o.ChecklistMontagem)
                .Include(o => o.ChecklistControlo)
                .Include(o => o.ChecklistEmbalagem)
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);

            if (ordem == null || ordem.Estado != EstadoOrdemProducao.EmProducao)
                return NotFound();

            // ❌ Verificação 1: Se a mota está associada
            if (ordem.Mota == null)
            {
                TempData["ErrorMessage"] = "A ordem precisa ter uma mota associada antes de ser finalizada.";
                return RedirectToAction(nameof(FichaOP), new { id });
            }

            // ❌ Verificação 2: Cor da mota preenchida
            if (string.IsNullOrWhiteSpace(ordem.Mota.Cor))
            {
                TempData["ErrorMessage"] = "A mota precisa ter uma cor definida para finalizar a ordem.";
                return RedirectToAction(nameof(FichaOP), new { id });
            }

            // ❌ Verificação 3: Todas as peças têm número de série preenchido
            if (ordem.Mota.MotasPecasSN == null || ordem.Mota.MotasPecasSN.Any(p => string.IsNullOrWhiteSpace(p.NumeroSerie)))
            {
                TempData["ErrorMessage"] = "Todas as peças da mota precisam ter número de série definido.";
                return RedirectToAction(nameof(FichaOP), new { id });
            }

            // ❌ Verificação 4: Checklists completos
            if (ordem.ChecklistMontagem.Any(c => c.Verificado != VerificadoChecklistMontagem.Sim) ||
                ordem.ChecklistControlo.Any(c => c.ControloFinal != ControloFinalChecklistControlo.Sim) ||
                ordem.ChecklistEmbalagem.Any(c => c.Incluido != IncluidoChecklistEmbalagem.Sim))
            {
                TempData["ErrorMessage"] = "Todos os itens dos checklists precisam estar concluídos para finalizar a ordem.";
                return RedirectToAction(nameof(FichaOP), new { id });
            }

            // ✅ Se passou todas as verificações, finaliza a OP
            ordem.Estado = EstadoOrdemProducao.Concluida;
            ordem.DataConclusao = DateTime.Now;

            // Atualizar o estado da mota para Ativo
            if (ordem.Mota != null)
            {
                ordem.Mota.Estado = EstadoMota.Ativo;
            }

            _context.Update(ordem);
            await _context.SaveChangesAsync();

            // Atualiza estado da encomenda se todas as OPs estiverem concluídas
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

        // Novo método para gerar número de identificação
        private string GerarNumeroIdentificacao()
        {
            // Prefixo AJP
            string prefixo = "AJP";
            
            // Caracteres permitidos para a parte aleatória
            string caracteresPermitidos = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            
            // Comprimento total menos o prefixo
            int comprimentoAleatorio = 17 - prefixo.Length;
            
            // Inicializar gerador de números aleatórios
            Random random = new Random();
            
            // Construir a parte aleatória
            var parteAleatoria = new StringBuilder(comprimentoAleatorio);
            for (int i = 0; i < comprimentoAleatorio; i++)
            {
                parteAleatoria.Append(caracteresPermitidos[random.Next(caracteresPermitidos.Length)]);
            }
            
            // Retornar o número de identificação completo
            return prefixo + parteAleatoria.ToString();
        }

        // Método para alterar o estado de uma ordem de produção
        [HttpPost]
        public async Task<IActionResult> AlterarEstado(int id, EstadoOrdemProducao novoEstado)
        {
            var ordemProducao = await _context.OrdemProducao
                .FirstOrDefaultAsync(o => o.IDOrdemProducao == id);
            
            if (ordemProducao == null)
            {
                return NotFound();
            }
            
            // Atualizar o estado da ordem de produção
            ordemProducao.Estado = novoEstado;
            
            // Se concluída, atualizar a data de conclusão
            if (novoEstado == EstadoOrdemProducao.Concluida && !ordemProducao.DataConclusao.HasValue)
            {
                ordemProducao.DataConclusao = DateTime.Now;
            }
            
            await _context.SaveChangesAsync();
            
            // Sincronizar o estado da encomenda
            await SincronizarEstadoEncomenda(ordemProducao.IDEncomenda);
            
            TempData["Sucesso"] = $"Estado da ordem atualizado para {novoEstado}";
            
            // Redirecionar para a página anterior
            string returnUrl = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            
            return RedirectToAction(nameof(Index));
        }

        // Método para sincronizar o estado da encomenda com base nas ordens de produção
        private async Task SincronizarEstadoEncomenda(int idEncomenda)
        {
            // Buscar a encomenda com suas ordens de produção
            var encomenda = await _context.Encomendas
                .Include(e => e.OrdemProducao)
                .FirstOrDefaultAsync(e => e.IDEncomenda == idEncomenda);

            if (encomenda == null || encomenda.OrdemProducao == null || !encomenda.OrdemProducao.Any())
                return;

            // Contar total de ordens e estados específicos
            int totalOrdens = encomenda.OrdemProducao.Count();
            int ordensPendentes = encomenda.OrdemProducao.Count(op => op.Estado == EstadoOrdemProducao.Pendente);
            int ordensEmProducao = encomenda.OrdemProducao.Count(op => op.Estado == EstadoOrdemProducao.EmProducao);
            int ordensConcluidas = encomenda.OrdemProducao.Count(op => op.Estado == EstadoOrdemProducao.Concluida);
            int ordensEmbaladas = encomenda.OrdemProducao.Count(op => op.Estado == EstadoOrdemProducao.Embalada);
            int ordensEnviadas = encomenda.OrdemProducao.Count(op => op.Estado == EstadoOrdemProducao.Enviada);

            // Determinar o novo estado da encomenda
            EstadoEncomenda novoEstado;

            if (ordensEnviadas == totalOrdens)
                novoEstado = EstadoEncomenda.Enviada;
            else if (ordensEmbaladas + ordensEnviadas == totalOrdens)
                novoEstado = EstadoEncomenda.Embalada;
            else if (ordensConcluidas + ordensEmbaladas + ordensEnviadas == totalOrdens)
                novoEstado = EstadoEncomenda.Concluida;
            else if (ordensEmProducao > 0)
                novoEstado = EstadoEncomenda.EmProducao;
            else
                novoEstado = EstadoEncomenda.Pendente;

            // Atualizar apenas se o estado for diferente
            if (encomenda.Estado != novoEstado)
            {
                encomenda.Estado = novoEstado;
                await _context.SaveChangesAsync();
                TempData["Sucesso"] = $"Estado da Encomenda #{encomenda.IDEncomenda} atualizado para {novoEstado}";
            }
        }

        // No método que cria a ordem de produção
        private void AssociarChecklists(OrdemProducao ordemProducao, int idModeloMota)
        {
            // Buscar checklists genéricos (IDModeloMota é null) e específicos para o modelo
            var checklists = _context.Checklist
                .Include(c => c.ChecklistModelos)
                .Where(c => !c.ChecklistModelos.Any() || c.ChecklistModelos.Any(cm => cm.IDModelo == idModeloMota))
                .ToList();

            // Criar instâncias para checklists de montagem
            foreach (var checklist in checklists.Where(c => c.Tipo == TipoChecklist.Montagem))
            {
                ordemProducao.ChecklistMontagem.Add(new ChecklistMontagem
                {
                    IDChecklist = checklist.IDChecklist,
                    IDOrdemProducao = ordemProducao.IDOrdemProducao
                });
            }

            // Criar instâncias para checklists de controlo
            foreach (var checklist in checklists.Where(c => c.Tipo == TipoChecklist.Controlo))
            {
                ordemProducao.ChecklistControlo.Add(new ChecklistControlo
                {
                    IDChecklist = checklist.IDChecklist,
                    IDOrdemProducao = ordemProducao.IDOrdemProducao
                });
            }

            // Criar instâncias para checklists de embalagem
            foreach (var checklist in checklists.Where(c => c.Tipo == TipoChecklist.Embalagem))
            {
                ordemProducao.ChecklistEmbalagem.Add(new ChecklistEmbalagem
                {
                    IDChecklist = checklist.IDChecklist,
                    IDOrdemProducao = ordemProducao.IDOrdemProducao
                });
            }
        }

        private bool OrdemProducaoExists(int id)
        {
            return _context.OrdemProducao.Any(e => e.IDOrdemProducao == id);
        }

        public async Task<IActionResult> EditarMota(int id)
        {
            var mota = await _context.Motas.FindAsync(id);
            if (mota == null)
            {
                return NotFound();
            }

            // Carregar informações do modelo
            var modeloMota = await _context.ModelosMota
                .FindAsync(mota.IDModelo);

            // Carregar peças do modelo
            var pecasFixas = await _context.ModeloPecasFixas
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == mota.IDModelo)
                .ToListAsync();

            var pecasSN = await _context.ModeloPecasSN
                .Include(p => p.Pecas)
                .Where(p => p.IDModelo == mota.IDModelo)
                .ToListAsync();

            // Carregar informações de peças já associadas a esta mota
            var pecasInfo = await _context.MotasPecasInfo
                .Where(p => p.IDMota == id)
                .ToListAsync();

            ViewBag.PecasFixas = pecasFixas;
            ViewBag.PecasSN = pecasSN;
            ViewBag.PecasInfo = pecasInfo;
            ViewBag.Modelo = modeloMota;

            return View(mota);
        }

        [HttpPost]
        public async Task<IActionResult> GuardarMota(Mota mota, Dictionary<int, string> PecasInfo, Dictionary<int, string> PecasSN)
        {
            // Salvar a mota
            if (mota.IDMota == 0)
            {
                mota.DataRegisto = DateTime.Now;
                mota.Estado = EstadoMota.EmProdução;
                mota.Quilometragem = 0;
                _context.Motas.Add(mota);
            }
            else
            {
                _context.Motas.Update(mota);
            }
            
            await _context.SaveChangesAsync();
            
            // Processar informações das peças fixas
            if (PecasInfo != null)
            {
                foreach (var pair in PecasInfo)
                {
                    int idPeca = pair.Key;
                    string info = pair.Value;
                    
                    var pecaInfo = await _context.MotasPecasInfo
                        .FirstOrDefaultAsync(p => p.IDMota == mota.IDMota && p.IDPeca == idPeca);
                        
                    if (pecaInfo != null)
                    {
                        pecaInfo.InformacaoAdicional = info;
                        _context.MotasPecasInfo.Update(pecaInfo);
                    }
                    else
                    {
                        _context.MotasPecasInfo.Add(new MotasPecasInfo
                        {
                            IDMota = mota.IDMota,
                            IDPeca = idPeca,
                            InformacaoAdicional = info
                        });
                    }
                }
            }
            
            // Processar informações das peças com número de série
            if (PecasSN != null)
            {
                // Lógica similar para salvar peças com S/N
            }
            
            await _context.SaveChangesAsync();
            
            return RedirectToAction("Details", new { id = mota.IDOrdemProducao });
        }
    }
}
