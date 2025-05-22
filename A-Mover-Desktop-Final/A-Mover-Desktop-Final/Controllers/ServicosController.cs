using A_Mover_Desktop_Final.Data;
using A_Mover_Desktop_Final.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace A_Mover_Desktop_Final.Controllers
{
    public class ServicosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ServicosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .ThenInclude(m => m.ModeloMota)
                .ToListAsync();
            return View(servicos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico
                .Include(s => s.Mota)
                    .ThenInclude(m => m.ModeloMota)
                .Include(s => s.PecasAlteradas)
                    .ThenInclude(p => p.MotasPecasSN)
                        .ThenInclude(mp => mp.Pecas)
                .FirstOrDefaultAsync(s => s.IDServico == id);

            if (servico == null) return NotFound();

            return View(servico);
        }
        public async Task<IActionResult> CalendarioIntervencoes()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .ToListAsync();

            return View(servicos);
        }
        [HttpGet]
        public async Task<IActionResult> GetIntervencoesAgendadas()
        {
            var servicos = await _context.Servico
                .Include(s => s.Mota)
                .Where(s => s.Estado != EstadoServico.Concluido)
                .ToListAsync();

            var eventos = servicos.Select(s => new
            {
                title = $"{s.Mota?.NumeroIdentificacao} - {s.Tipo}",
                start = s.DataServico.ToString("s"), // formato ISO 8601
                extendedProps = new
                {
                    vin = s.Mota?.NumeroIdentificacao,
                    tipoManutencao = s.Tipo.ToString(),
                    description = string.IsNullOrEmpty(s.Descricao) ? "Sem descrição" : s.Descricao
                }
            });

            return Json(eventos);
        }

        // GET: Servicos/AgendarIntervencao
        public IActionResult AgendarIntervencao()
        {
            ViewData["Motas"] = _context.Motas
                .Select(m => new SelectListItem
                {
                    Value = m.IDMota.ToString(),
                    Text = m.NumeroIdentificacao
                }).ToList();

            ViewData["TiposServico"] = Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().ToList();

            var servico = new Servico
            {
                DataServico = DateTime.Now.AddDays(1)
            };

            return View(servico);
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgendarIntervencao(Servico servico)
        {
            if (!ModelState.IsValid)
            {
                ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao", servico.IDMota);
                return View(servico);
            }

            servico.Estado = EstadoServico.Agendado;

            _context.Servico.Add(servico);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Intervenção agendada com sucesso.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> RegistarIntervencao()
        {
            ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao");
            return View();
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> RegistarIntervencao(Servico servico)
{
    // Verificar se a mota existe
    var motaExiste = await _context.Motas.AnyAsync(m => m.IDMota == servico.IDMota);
    if (!motaExiste)
    {
        ModelState.AddModelError("IDMota", "Mota não encontrada. Por favor, selecione uma mota válida.");
        ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao");
        return View(servico);
    }

    if (!ModelState.IsValid)
    {
        ViewData["IDMota"] = new SelectList(_context.Motas.Include(m => m.ModeloMota), "IDMota", "NumeroIdentificacao", servico.IDMota);
        return View(servico);
    }

    servico.Estado = EstadoServico.EmCurso;
    servico.DataConclusao = DateTime.MinValue;
    servico.DataServico = DateTime.Now;

    _context.Servico.Add(servico);
    await _context.SaveChangesAsync();

    TempData["SuccessMessage"] = "Intervenção iniciada com sucesso.";
    return RedirectToAction(nameof(Index));
}

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico.FindAsync(id);
            if (servico == null) return NotFound();

            ViewData["IDMota"] = new SelectList(_context.Motas, "IDMota", "NumeroIdentificacao", servico.IDMota);
            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IDServico,IDMota,Tipo,Descricao,Estado,DataServico,DataConclusao")] Servico servico)
        {
            if (id != servico.IDServico) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ServicoExists(servico.IDServico)) return NotFound();
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["IDMota"] = new SelectList(_context.Motas, "IDMota", "NumeroIdentificacao", servico.IDMota);
            return View(servico);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var servico = await _context.Servico
                .Include(s => s.Mota)
                .FirstOrDefaultAsync(s => s.IDServico == id);

            if (servico == null) return NotFound();

            return View(servico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var servico = await _context.Servico.FindAsync(id);
            if (servico != null)
            {
                _context.Servico.Remove(servico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ServicoExists(int id)
        {
            return _context.Servico.Any(e => e.IDServico == id);
        }

        // Método para buscar motas por número de identificação (VIN)
        [HttpGet]
        public async Task<IActionResult> ObterMotasPorIdentificacao(string termo)
        {
            if (string.IsNullOrEmpty(termo) || termo.Length < 2)
                return Json(new List<object>());

            var motas = await _context.Motas
                .Include(m => m.ModeloMota)
                .Where(m => m.NumeroIdentificacao.Contains(termo) && 
                            m.Estado == EstadoMota.Ativo)  // Apenas motas ativas
                .Take(10)  // Limitar resultados para melhor performance
                .Select(m => new
                {
                    id = m.IDMota,
                    texto = m.NumeroIdentificacao,
                    modelo = m.ModeloMota.Nome,
                    cor = m.Cor,
                    estado = m.Estado.ToString(),
                    infoAdicional = $"{m.ModeloMota.Nome} - {m.Cor}"
                })
                .ToListAsync();

            return Json(motas);
        }

        public async Task<IActionResult> IniciarServico(int id)
        {
            var servico = await _context.Servico.FindAsync(id);
            
            if (servico == null)
                return NotFound();
                
            if (servico.Estado != EstadoServico.Agendado)
            {
                TempData["ErrorMessage"] = "Apenas serviços agendados podem ser iniciados.";
                return RedirectToAction(nameof(Index));
            }
            
            servico.Estado = EstadoServico.EmCurso;
            servico.DataServico = DateTime.Now;
            
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Serviço iniciado com sucesso.";
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Servicos/GerirPecas/5
        public async Task<IActionResult> GerirPecas(int? id)
        {
            var servico = await _context.Servico
                .Include(s => s.Mota)
                    .ThenInclude(m => m.ModeloMota) // Adicione esta linha para carregar o modelo
                .Include(s => s.Mota.MotasPecasSN)
                    .ThenInclude(mp => mp.Pecas)
                .Include(s => s.PecasAlteradas)
                .FirstOrDefaultAsync(s => s.IDServico == id);
                
            if (servico == null)
                return NotFound();
                
            if (servico.Estado == EstadoServico.Concluido)
            {
                TempData["ErrorMessage"] = "Não é possível alterar peças de um serviço concluído.";
                return RedirectToAction(nameof(Details), new { id });
            }
            
            return View(servico);
        }

        // Método POST: Servicos/GerirPecas/5
// Método POST: Servicos/GerirPecas/5
[HttpPost]
public async Task<IActionResult> GerirPecas(int id, List<int> pecasParaSubstituir, List<string> novasSeriesNumericas, string NotasManutencao)
{
    var servico = await _context.Servico
        .Include(s => s.Mota)
            .ThenInclude(m => m.MotasPecasSN)
                .ThenInclude(mp => mp.Pecas)
        .FirstOrDefaultAsync(s => s.IDServico == id);
        
    if (servico == null)
    {
        return NotFound();
    }
    
    // Salvar as notas de manutenção
    servico.NotasServico = NotasManutencao;
    
    // Primeiro atualize o serviço
    _context.Update(servico);
    
    // Obter peças alteradas existentes para este serviço
    var pecasAlteradasExistentes = await _context.ServicosPecasAlteradas
        .Where(p => p.IDServico == id)
        .ToListAsync();
    
    // Remover registros existentes
    if (pecasAlteradasExistentes.Any())
    {
        _context.ServicosPecasAlteradas.RemoveRange(pecasAlteradasExistentes);
    }
    
    // Processar as peças selecionadas para substituição
    if (pecasParaSubstituir != null && pecasParaSubstituir.Count > 0)
    {
        for (int i = 0; i < pecasParaSubstituir.Count; i++)
        {
            var idPeca = pecasParaSubstituir[i];
            var novoNumeroSerie = (i < novasSeriesNumericas.Count) ? novasSeriesNumericas[i] : null;
            
            // Criar nova entrada para peça alterada
            var pecaAlterada = new ServicosPecasAlteradas
            {
                IDServico = id,
                IDMotasPecasSN = idPeca,
                Observacoes = $"Substituição: Novo número de série {novoNumeroSerie}"
            };
            
            _context.ServicosPecasAlteradas.Add(pecaAlterada);
            
            // Atualizar o número de série na tabela MotasPecasSN se fornecido
            if (!string.IsNullOrEmpty(novoNumeroSerie))
            {
                var motaPeca = await _context.MotasPecasSN.FindAsync(idPeca);
                if (motaPeca != null)
                {
                    motaPeca.NumeroSerie = novoNumeroSerie;
                    _context.Update(motaPeca);
                }
            }
        }
    }
    
    await _context.SaveChangesAsync();
    
    TempData["Sucesso"] = "Alterações nas peças e notas salvas com sucesso!";
    return RedirectToAction(nameof(Details), new { id = id });
}
        // POST: Servicos/ConcluirServico/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConcluirServico(int id)
        {
            var servico = await _context.Servico.FindAsync(id);
            
            if (servico == null)
                return NotFound();
                
            if (servico.Estado == EstadoServico.Concluido)
            {
                TempData["ErrorMessage"] = "Este serviço já está concluído.";
                return RedirectToAction(nameof(Index));
            }
            
            servico.Estado = EstadoServico.Concluido;
            servico.DataConclusao = DateTime.Now;
            
            _context.Update(servico);
            await _context.SaveChangesAsync();
            
            TempData["SuccessMessage"] = "Serviço concluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
    }
}